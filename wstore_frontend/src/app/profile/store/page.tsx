'use client'
import React from 'react';
import CreateStoreArea from "@/UI/CreateStoreArea";
import {useSelector} from "react-redux";
import {useAppSelector} from "@/hooks/redux";
import useModal from "@/hooks/useModal";
import CreateStoreTranslation from "@/UI/CreateStoreTranslation";


const Store = () => {
    const user = useAppSelector(state => state.authSlice.user);

    return (
        <div className="p-6 flex  justify-center  items-center w-[85%]">
            {user?.roles.includes("StoreOwner") ?  "welcome"  :  <CreateStoreArea /> }
            {/*<CreateStoreArea />*/}
        </div>
    );
};

export default Store;