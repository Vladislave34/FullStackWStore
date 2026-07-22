'use client'

import AuthorizeMenu from "@/app/[lng]/components/AuthorizeMenu";
import GuestMenu from "@/app/[lng]/UI/GuestMenu";
import {useAppSelector} from "@/hooks/redux";
import IUser from "@/models/auth/IUser";
import {FC} from "react";
import {TranslationRecord} from "@/types/translations";



const Menu  = () => {
    const user = useAppSelector(state => state.authSlice.user);

    return (
        <>
            {user ?
                <AuthorizeMenu  user={user} />
                :
                <GuestMenu   />
            }
        </>
    );
};

export default Menu;