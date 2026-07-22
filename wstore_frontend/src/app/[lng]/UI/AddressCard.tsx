'use client'
import React from 'react';
import {FiMapPin} from "react-icons/fi";
import {MdDelete, MdEdit} from "react-icons/md";
import Icon from "@/app/[lng]/UI/Icon";
import UseModal from "@/hooks/useModal";
import Modal from "@/app/[lng]/UI/Modal";
import EditAddressForm from "@/app/[lng]/UI/forms/EditAddressForm";
import {addressApi} from "@/services/addressService";
import IAddressModel from "@/models/address/IAddressModel";
import {useT} from "next-i18next/client";

interface AddressCardProps {
    address: IAddressModel;
    isSelected?: boolean;
    onSelect?: (id: string) => void;
}

const AddressCard = ({address, isSelected, onSelect}: AddressCardProps) => {
    const [deleteAddress] = addressApi.useDeleteAddressMutation();
    const {isOpen, openModal, closeModal} = UseModal();
    const {t} = useT('address_card');

    return (
        <>
            <div
                onClick={() => onSelect?.(address.id)}
                className={`
                    group relative flex items-start gap-4 p-4 rounded-2xl
                    bg-[var(--card)] border transition-colors
                    ${isSelected ? "border-[var(--btn)]" : "border-[var(--border)]"}
                    ${onSelect ? "cursor-pointer hover:border-[var(--accent-mid)]" : ""}
                `}
            >
                <div
                    className="
                        flex items-center justify-center shrink-0
                        w-10 h-10 rounded-full bg-[var(--accent-soft)]
                    "
                >
                    <FiMapPin size={18} className="text-[var(--accent)]"/>
                </div>

                <div className="flex-1 min-w-0">
                    <p className="text-[15px] font-medium text-[var(--text)] truncate">
                        {address.street}, {address.houseNumber}
                    </p>
                    <p className="text-[13px] text-[var(--muted)] mt-0.5 truncate">
                        {address.city}, {address.country}
                    </p>
                </div>

                <div
                    className="
                        flex items-center gap-1 shrink-0
                        opacity-0 group-hover:opacity-100 transition-opacity
                    "
                >
                    <Icon width={36} height={36}>
                        <MdEdit
                            size={18}
                            color="var(--accent-mid)"
                            onClick={(e) => {
                                e.stopPropagation();
                                openModal();
                            }}
                            aria-label={t('edit')}
                        />
                    </Icon>
                    <Icon width={36} height={36}>
                        <MdDelete
                            size={18}
                            color="var(--accent-mid)"
                            onClick={(e) => {
                                e.stopPropagation();
                                deleteAddress(address.id);
                            }}
                            aria-label={t('delete')}
                        />
                    </Icon>
                </div>

                {isSelected && (
                    <span
                        className="
                            absolute top-3 right-3 w-2 h-2 rounded-full
                            bg-[var(--price)]
                        "
                    />
                )}
            </div>

            <Modal isOpen={isOpen} closeModal={closeModal} size='md' title={t('edit_title')}>
                <EditAddressForm closeModal={closeModal} address={address}/>
            </Modal>
        </>
    );
};

export default AddressCard;