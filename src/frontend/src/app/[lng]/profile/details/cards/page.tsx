"use client";

import React from "react";
import TitleButton from "@/app/[lng]/UI/TitleButton";
import useModal from "@/hooks/useModal";
import Modal from "@/app/[lng]/UI/Modal";
import PaymentCardForm from "@/app/[lng]/UI/forms/PaymentCardForm";
import {paymentApi} from "@/services/paymentService";
import Card from "@/app/[lng]/UI/Card";
import {useT} from "next-i18next/client";


const Page = () => {
    const {t} = useT('details');
    const {isOpen, openModal, closeModal} = useModal();
    const {data} = paymentApi.useGetCardsByUserQuery();
    return (
        <>
        <div className="p-8 min-h-screen w-full">
            <div className="flex flex-row gap-2 py-4 mb-4 justify-between items-center ">
                <span className='text-[var(--text)] text-3xl font-semibold'>{t('cards')}</span>
                <TitleButton func={()=>openModal()} title={t('add_card')} />
            </div>
            <div className="grid grid-cols-2">
                {data?.map((card) => (<Card card={card} key={card.id} />))}

            </div>

        </div>
            <Modal isOpen={isOpen} title={t('add_card')} size={'md'} closeModal={closeModal}>
                <PaymentCardForm closaModal={closeModal}  />
            </Modal>
        </>
    );
};

export default Page;