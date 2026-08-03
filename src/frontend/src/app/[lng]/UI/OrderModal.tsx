'use client'
import React, {useState} from 'react';
import {useAppSelector} from "@/hooks/redux";
import {addressApi} from "@/services/addressService";
import {paymentApi} from "@/services/paymentService";
import {orderApi} from "@/services/orderService";
import {cartApi} from "@/services/cartService";
import {data} from "framer-motion/m";
import {useT} from "next-i18next/client"; // припущення: назва сервісу карток


interface OrderModalProps {
    isOpen: boolean;
    onClose: () => void;
}

const OrderModal = ({isOpen, onClose}: OrderModalProps) => {
    // const {data : cart} = cartApi.useGetCartByUserQuery()
    // console.log("cart", cart);
    // const cartId = cart?.id;
    const {t} = useT('order_modal')
    const cartItemIds = useAppSelector(state => state.cartItemSlice.cartItemIds);

    const {data: addresses, isLoading: addressesLoading} = addressApi.useGetAddressesByUserQuery();
    const {data: cards, isLoading: cardsLoading} = paymentApi.useGetCardsByUserQuery();

    const [selectedAddressId, setSelectedAddressId] = useState<string | null>(null);
    const [selectedPaymentId, setSelectedPaymentId] = useState<string | null>(null);

    const [addOrder, {isLoading: isSubmitting}] = orderApi.useAddOrderMutation();

    if (!isOpen) return null;

    const canSubmit = !!selectedAddressId && !!selectedPaymentId && !isSubmitting;

    const handleSubmit = async () => {
        if (!selectedAddressId || !selectedPaymentId) return;

        try {
            console.log({

                cartItemIds,
                addressId: selectedAddressId,
                paymentId: selectedPaymentId,
            });
            await addOrder({

                cartItemIds,
                addressId: selectedAddressId,
                paymentId: selectedPaymentId,
            }).unwrap();

            onClose();
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <div
            className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4"
            onClick={onClose}
        >
            <div
                className="bg-[var(--bg)] rounded-2xl shadow-xl w-full max-w-md p-6 max-h-[85vh] overflow-y-auto"
                onClick={(e) => e.stopPropagation()}
            >
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-xl font-semibold text-[var(--text)]">{t('checkoutTitle')}</h2>
                    <button onClick={onClose} className="text-[var(--text)] opacity-60 hover:opacity-100">
                        ✕
                    </button>
                </div>

                {/* Вибір адреси */}
                <div className="mb-6">
                    <h3 className="text-sm font-semibold text-[var(--text)] mb-2">{t('shippingAddress')}</h3>
                    {addressesLoading && <p className="text-sm opacity-60">{t('loading')}</p>}
                    <div className="flex flex-col gap-2">
                        {addresses?.map((address) => (
                            <label
                                key={address.id}
                                className={`border rounded-lg p-3 cursor-pointer flex items-center gap-2 transition-colors
                                ${selectedAddressId === address.id
                                    ? "border-[var(--btn)] bg-[var(--btn)]/10"
                                    : "border-gray-300"}`}
                            >
                                <input
                                    type="radio"
                                    name="address"
                                    checked={selectedAddressId === address.id}
                                    onChange={() => setSelectedAddressId(address.id)}
                                />
                                <span className="text-sm text-[var(--text)]">
                                {address.country}, {address.city}, {address.street} {address.houseNumber}
                            </span>
                            </label>
                        ))}
                        {!addressesLoading && addresses?.length === 0 && (
                            <p className="text-sm opacity-60">{t('noSavedAddresses')}</p>
                        )}
                    </div>
                </div>

                {/* Вибір картки */}
                <div className="mb-6">
                    <h3 className="text-sm font-semibold text-[var(--text)] mb-2">{t('paymentMethod')}</h3>
                    {cardsLoading && <p className="text-sm opacity-60">{t('loading')}</p>}
                    <div className="flex flex-col gap-2">
                        {cards?.map((card) => (
                            <label
                                key={card.id}
                                className={`border rounded-lg p-3 cursor-pointer flex items-center gap-2 transition-colors
                                ${selectedPaymentId === card.id
                                    ? "border-[var(--btn)] bg-[var(--btn)]/10"
                                    : "border-gray-300"}`}
                            >
                                <input
                                    type="radio"
                                    name="payment"
                                    checked={selectedPaymentId === card.id}
                                    onChange={() => setSelectedPaymentId(card.id)}
                                />
                                <span className="text-sm text-[var(--text)]">
                                •••• {card.number?.slice(-4)}
                            </span>
                            </label>
                        ))}
                        {!cardsLoading && cards?.length === 0 && (
                            <p className="text-sm opacity-60">{t('noSavedCards')}</p>
                        )}
                    </div>
                </div>

                <button
                    onClick={handleSubmit}
                    disabled={!canSubmit}
                    className="w-full h-[42px] bg-[var(--btn)] text-white text-sm font-semibold rounded-lg
                           hover:opacity-90 transition-opacity disabled:opacity-40 disabled:cursor-not-allowed"
                >
                    {isSubmitting ? t('placingOrder') : t('confirmOrder')}
                </button>
            </div>
        </div>
    );
};

export default OrderModal;