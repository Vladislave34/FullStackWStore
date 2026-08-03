'use client'


import React from "react";
import {orderApi} from "@/services/orderService";
import OrderRow from "@/app/[lng]/UI/OrderRow";
import Icon from "@/app/[lng]/UI/Icon";
import {useT} from "next-i18next/client";
import {usePathname} from "next/navigation";


const MyOrders = () => {
    const {t} = useT('profile')
    const {data} = orderApi.useGetMyOrdersQuery();
    const pathname = usePathname();
    const lng = pathname.split('/')[1] ?? "en";

    return (
        <div className="p-8 min-h-screen">
            <div className="flex flex-row gap-2 py-4 mb-4 justify-between items-center ">
                <span className='text-[var(--text)] text-3xl font-semibold'>{t("my_orders")}</span>
                <Icon width={36} height={36} >
                    <span className='text-[var(--text)] text-xl font-semibold'>{data?.length}</span>
                </Icon>
            </div>
            <div className="flex flex-col gap-3 w-full max-h-[calc(100vh-8rem)] overflow-y-auto ">
                {data?.map((order) => (<OrderRow order={order} key={order.id} lng={lng}  />))}

            </div>
        </div>
    );
};

export default MyOrders;