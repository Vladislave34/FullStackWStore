"use client";

import React from "react";
import TitleButton from "@/app/[lng]/UI/TitleButton";
import useModal from "@/hooks/useModal";
import Modal from "@/app/[lng]/UI/Modal";
import PaymentCardForm from "@/app/[lng]/UI/forms/PaymentCardForm";
import {paymentApi} from "@/services/paymentService";
import Card from "@/app/[lng]/UI/Card";


const Page = () => {
    const {isOpen, openModal, closeModal} = useModal();
    const {data} = paymentApi.useGetCardsByUserQuery();
    return (
        <>
        <div className="p-8 min-h-screen w-full">
            <div className="flex flex-row gap-2 py-4 mb-4 justify-between items-center ">
                <span className='text-[var(--text)] text-3xl font-semibold'>Cards</span>
                <TitleButton func={()=>openModal()} title={"Add Card"} />
            </div>
            <div className="grid grid-cols-2">
                {data?.map((card) => (<Card card={card} key={card.id} />))}

            </div>

        </div>
            <Modal isOpen={isOpen} title={"Add Card"} size={'md'} closeModal={closeModal}>
                <PaymentCardForm closaModal={closeModal}  />
            </Modal>
        </>
    );
};

export default Page;