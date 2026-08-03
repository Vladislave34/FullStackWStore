'use client'
import {useRef, useState} from "react";
import { IoSend } from "react-icons/io5";
import {authApi} from "@/services/authService";

const EmailInput = () => {
    const emailRef = useRef<HTMLInputElement>(null);
    const [forgotPassword, {isLoading}] = authApi.useForgotPasswordMutation();
    const handleSubmit = async () => {
        const email = emailRef.current?.value.trim();

        if (!email) {
            alert("Введіть email");
            return;
        }


        await forgotPassword({email: email}).unwrap();

        // приклад очищення поля
        emailRef.current!.value = "";
    };

    return (
        <>
        <div className="relative w-full max-w-md">
            <input
                ref={emailRef}
                type="email"
                placeholder="example@gmail.com"
                onKeyDown={async (e) => {
                    if (e.key === "Enter") {
                        await handleSubmit();
                    }
                }}
                className="
                     bg-[var(--search)]
          border border-[var(--border)]
          rounded-[10px]
          px-4 py-[10px]
          text-sm text-[var(--placeholder)]
          placeholder:text-[#B4B2A9]
          outline-none
          transition-colors
          focus:border-[#888780]
          w-full
                "
            />

            <button
                type="button"
                onClick={handleSubmit}
                className="
                    absolute
                    right-0
                    top-1/2
                    -translate-y-1/2
                    w-10
                    h-10
                    rounded-[10px]
                    flex
                    items-center
                    justify-center
                    border border-2 border-[var(--border)]
                    transition-colors
                    text-white
                    hover:cursor-pointer
                "
            >
                <IoSend size={18} />
            </button>
        </div>
            {isLoading && (<span className="text-[13px] text-[#5F5E5A] mb-[6px]">Надсилання...</span>)}
        </>
    );
};

export default EmailInput;