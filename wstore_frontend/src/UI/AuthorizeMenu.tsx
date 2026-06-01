'use client'

import {FC, useEffect, useRef, useState} from "react";
import Image from "next/image";

import Icon from "@/UI/Icon";

import { TranslationRecord} from "@/types/translations";
import IUser from "@/models/auth/IUser";
import {logout} from "@/store/reducers/authSlice";
import {useAppDispatch} from "@/hooks/redux";
import {useRouter} from "next/navigation";
interface AuthorizeMenuProps {
    labels: TranslationRecord<'authorizemenu'>;
    user: IUser
}



const AuthorizeMenu  : FC<AuthorizeMenuProps> = ({labels, user}  ) => {
    const [open, setOpen] = useState(false);
    const menuRef = useRef<HTMLDivElement | null>(null);
    const dispatch = useAppDispatch();
    const router = useRouter()
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
                    <Icon height={40} width={40} isOpen={open}    >
                        <Image className="rounded-full object-cover w-7 h-7" src={user?.image ?? "/user.png"} alt="Cart" width={24} height={24} priority unoptimized />
                    </Icon>

                </button>


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
                        className="block px-4 py-3
                        text-[var(--text)] border-b border-[var(--border)]"
                    >{user.username}</div>
                    <div

                        className="
                        block px-4 py-3
                        text-[var(--text)]
                        hover:bg-[var(--accent-soft)]
                        transition hover:cursor-pointer
                    "
                        onClick={() => {}}
                    >
                        {labels.myorders}
                    </div>

                    <div
                        className="
                        block px-4 py-3
                        text-[var(--text)]
                        hover:bg-[var(--accent-soft)]
                        transition
                        hover:cursor-pointer
                    "
                        onClick={() => {
                            setOpen(false)
                            router.push("/profile")
                        }}
                    >
                        {labels.profile}
                    </div>

                    <div

                        className="
                        block px-4 py-3
                        text-[var(--text)]
                        hover:bg-[var(--accent-soft)]
                        transition hover:cursor-pointer
                    "
                        onClick={()=> dispatch(logout())}
                    >
                        {labels.logout}
                    </div>
                </div>
            </div>


        </>
    );
};

export default AuthorizeMenu;