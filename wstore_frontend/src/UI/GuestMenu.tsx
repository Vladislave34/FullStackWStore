'use client'

import { useEffect, useRef, useState } from "react";
import Image from "next/image";

import Icon from "@/UI/Icon";
import useModal from "@/hooks/useModal";
import Modal from "@/UI/Modal";
import LoginForm from "@/UI/forms/LoginForm";
import {TranslationProps} from "@/types/translations";
import RegisterForm from "@/UI/forms/RegisterForm";
import {CgProfile} from "react-icons/cg";
import {useGoogleLogin} from "@react-oauth/google";
import {loginSuccess} from "@/store/reducers/authSlice";
import {useLoginByGoogleMutation} from "@/services/authService";
import {useAppDispatch} from "@/hooks/redux";
import {useRouter} from "next/navigation";



const GuestMenu = ({labels} : TranslationProps<'guestmenu'>) => {
    const [open, setOpen] = useState(false);
    const menuRef = useRef<HTMLDivElement | null>(null);
    const modalLogin = useModal();
    const modalRegister = useModal();
    const[ loginByGoogle ] = useLoginByGoogleMutation();
    const dispatch = useAppDispatch();
    const navigate = useRouter();
    const loginUseGoogle = useGoogleLogin({
        onSuccess: async (tokenResponse) =>
        {
            console.log("tokenResponse", tokenResponse.access_token);
            try {
                const response = await loginByGoogle(tokenResponse.access_token).unwrap();



                dispatch(loginSuccess(response));
                navigate.push('/')
            } catch (error) {
                console.error("Google логін не вдалий:", error);
            }
        },
    });
    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (
                menuRef.current &&
                !menuRef.current.contains(event.target as Node)
            ) {
                setOpen(false);
            }
        };

        document.addEventListener("mousedown", handleClickOutside);
        return () =>
            document.removeEventListener("mousedown", handleClickOutside);
    }, []);

    return (
        <>
        <div ref={menuRef} className="relative">

            <button
                onClick={() => setOpen(prev => !prev)}
                className="
                    w-10 h-10 rounded-full
                    flex items-center justify-center
                    hover:bg-[var(--hover)]
                    transition
                "
            >
                <Icon height={40} width={40} isOpen={open}   >
                    {/*<Image src="/user.png" alt="Cart" width={24} height={24} priority />*/}
                    <CgProfile size={27} color="var(--accent-mid)" />

                </Icon>

            </button>

            {/* dropdown */}
            <div
                className={`
                    absolute right-0 top-12
                    w-48
                    rounded-xl
                    border border-[var(--border)]
                    bg-[var(--surface)]
                    shadow-lg
                    overflow-hidden
                    transition-all duration-200
                    z-50
                    ${
                    open
                        ? "opacity-100 visible translate-y-0"
                        : "opacity-0 invisible -translate-y-2"
                }
                `}
            >
                <div

                    className="
                        block px-4 py-3
                        text-[var(--text)]
                        hover:bg-[var(--accent-soft)]
                        transition hover:cursor-pointer
                    "
                    onClick={() => modalRegister.openModal()}
                >
                    {labels.register}
                </div>

                <div
                    className="
                        block px-4 py-3
                        text-[var(--text)]
                        hover:bg-[var(--accent-soft)]
                        transition hover:cursor-pointer
                    "
                    onClick={() => modalLogin.openModal()}
                >
                    {labels.login}
                </div>

                <div

                    className="
                        block px-4 py-3
                        text-[var(--text)]
                        hover:bg-[var(--accent-soft)]
                        transition hover:cursor-pointer
                    "
                    onClick={(event) => {
                        event.preventDefault();
                        loginUseGoogle();
                    }}
                >
                    Google
                </div>
            </div>
        </div>
            <Modal {...modalLogin} title="Логін" size="sm">
                <LoginForm closeModal={modalLogin.closeModal} />
            </Modal>
            <Modal {...modalRegister} title="Реєстрація" size="md">
                <RegisterForm closeModal={modalRegister.closeModal} />
            </Modal>

        </>
    );
};

export default GuestMenu;