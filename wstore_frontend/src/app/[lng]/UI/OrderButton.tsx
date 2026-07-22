'use client'
import React, {useState} from 'react';
import OrderModal from "@/app/[lng]/UI/OrderModal";

const OrderButton = () => {
    const [isModalOpen, setIsModalOpen] = useState(false);

    return (
        <>
            <button
                onClick={() => setIsModalOpen(true)}
                className="
                            h-[42px] px-4
                            bg-[var(--btn)] text-white text-sm font-semibold
                            rounded-lg hover:opacity-90 transition-opacity
                            flex items-center gap-2 whitespace-nowrap
                        "
            >
                Замовити
            </button>

            <OrderModal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)} />
        </>
    );
};

export default OrderButton;