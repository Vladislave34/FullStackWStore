'use client'
import useModal from "@/hooks/useModal";
import Modal from "@/UI/Modal";
import CreateStoreForm from "@/UI/forms/CreateStoreForm";
import {TranslationProps, TranslationRecord} from "@/types/translations";
import {FC} from "react";
interface CreateStoreAreaProps {
    transform?: TranslationRecord<"storecreateform">
}



const CreateStoreArea : FC<CreateStoreAreaProps> = ({transform}) => {
    const modal = useModal();
    return (
        <>
        <div className=" flex flex-col gap-4 p-8 w-1/2 border-x-2 border-[var(--border)] ">
            <div className="flex flex-row items-center gap-1  ">
                <div className="text-base font-semibold text-[var(--tag-text)]">
                    Seller Platform
                </div>
                <div className="flex-1 h-0.5 border-b-2 border-[var(--border)] translate-y-[1px]" />
            </div>
            <div
                className="flex flex-col justify-start  text-6xl font-bold text-[var(--accent)]
              border-b-2 border-[var(--border)] pb-3
             ">
                <div>Start</div>
                <div>Earn</div>
                <div className="text-[var(--accent-soft)]">Money</div>
            </div>
            <div className="text-[var(--tag-text)] text-base w-3/4">
                Open your store in minutes. No upfront fees. Get paid directly to your account.
            </div>
            <div className="">
                <button
                    className="px-6 py-2.5  rounded-lg text-[14px] bg-[var(--tag)] w-1/3
                                    font-semibold transition-opacity cursor-pointer text-[var(--tag-text)]
                                    border border-[var(--border)] disabled:opacity-50
                                     disabled:cursor-not-allowed hover:bg-blue-500/10 hover:border-blue-500/20 hover:text-blue-500 hover:shadow-sm"
                    onClick={()=>modal.openModal()}
                >
                    Create Store
                </button>
            </div>
        </div>
            <Modal title={"Create store"} isOpen={modal.isOpen} closeModal={modal.closeModal}>
                <CreateStoreForm closeModal={modal.closeModal} trans={transform}  />
            </Modal>
        </>
    );
};

export default CreateStoreArea;