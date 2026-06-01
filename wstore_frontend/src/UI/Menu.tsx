'use client'

import AuthorizeMenu from "@/UI/AuthorizeMenu";
import GuestMenu from "@/UI/GuestMenu";
import {useAppSelector} from "@/hooks/redux";
import IUser from "@/models/auth/IUser";
import {FC} from "react";
import {TranslationRecord} from "@/types/translations";

interface MenuProps {
    guestMenu: TranslationRecord<'guestmenu'>;
    authorizeMenu: TranslationRecord<'authorizemenu'>;
}

const Menu  : FC<MenuProps> = ({guestMenu, authorizeMenu}) => {
    const user = useAppSelector(state => state.authSlice.user);
    console.log(user?.image);
    return (
        <>
            {user ?
                <AuthorizeMenu labels={authorizeMenu} user={user} />
                :
                <GuestMenu labels={guestMenu}  />
            }
        </>
    );
};

export default Menu;