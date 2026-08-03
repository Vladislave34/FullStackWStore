'use client'
import React from 'react';
import CreateStoreArea from "@/app/[lng]/UI/CreateStoreArea";
import {useSelector} from "react-redux";
import {useAppSelector} from "@/hooks/redux";
import useModal from "@/hooks/useModal";
import CreateStoreTranslation from "@/app/[lng]/UI/CreateStoreTranslation";


const Store = () => {
    const user = useAppSelector(state => state.authSlice.user);

    return (
        <div className="p-8 flex  justify-center  items-center w-[85%]">
            {user?.roles.includes("StoreOwner") ?  "welcome"  :  <CreateStoreArea /> }
            {/*<CreateStoreArea />*/}
        </div>
    );
};

export default Store;