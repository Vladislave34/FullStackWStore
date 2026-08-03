"use client";
import TitleButton from "@/app/[lng]/UI/TitleButton";
import Card from "@/app/[lng]/UI/Card";
import Modal from "@/app/[lng]/UI/Modal";
import PaymentCardForm from "@/app/[lng]/UI/forms/PaymentCardForm";
import React from "react";
import useModal from "@/hooks/useModal";
import AddAddressForm from "@/app/[lng]/UI/forms/AddAdrressForm";
import {addressApi} from "@/services/addressService";
import AddressCard from "@/app/[lng]/UI/AddressCard";
import {useT} from "next-i18next/client";


const Page = () => {
    const {t} = useT('details');
    const {isOpen, openModal, closeModal} = useModal();
    const {data} = addressApi.useGetAddressesByUserQuery();

    return (
        <>
            <div className="p-8 min-h-screen">
                <div className="flex flex-row gap-2 py-4 mb-4 justify-between items-center ">
                    <span className='text-[var(--text)] text-3xl font-semibold'>{t('address')}</span>
                    <TitleButton func={()=>openModal()} title={t("add_address")} />
                </div>
                <div className="grid grid-cols-2">
                    {data?.map((adrress) => (<AddressCard address={adrress} key={adrress.id} />))}

                </div>

            </div>
            <Modal isOpen={isOpen} title={t("add_address")}  size={'md'} closeModal={closeModal} >
                <AddAddressForm closeModal={closeModal}   />
            </Modal>
        </>
    );
};

export default Page;