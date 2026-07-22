'use client'

import { useEffect, useRef, useState } from "react";
import Icon from "@/app/[lng]/UI/Icon";
import useModal from "@/hooks/useModal";
import Modal from "@/app/[lng]/UI/Modal";
import LoginForm from "@/app/[lng]/UI/forms/LoginForm";
import RegisterForm from "@/app/[lng]/UI/forms/RegisterForm";
import {CgProfile} from "react-icons/cg";
import {useGoogleLogin} from "@react-oauth/google";
import {loginSuccess} from "@/store/reducers/authSlice";
import {useLoginByGoogleMutation} from "@/services/authService";
import {useAppDispatch} from "@/hooks/redux";
import {useRouter} from "next/navigation";
import { useT } from 'next-i18next/client'



const GuestMenu = () => {
    const [open, setOpen] = useState(false);
    const menuRef = useRef<HTMLDivElement | null>(null);
    const modalLogin = useModal();
    const modalRegister = useModal();
    const[ loginByGoogle ] = useLoginByGoogleMutation();
    const dispatch = useAppDispatch();
    const navigate = useRouter();
    const { t } = useT('guest_menu');
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
                    {t('register')}
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
                    {t("login")}
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
            <Modal {...modalLogin} title={t("login")} size="sm">
                <LoginForm closeModal={modalLogin.closeModal} />
            </Modal>
            <Modal {...modalRegister} title={t('register')} size="md">
                <RegisterForm closeModal={modalRegister.closeModal} />
            </Modal>

        </>
    );
};

export default GuestMenu;