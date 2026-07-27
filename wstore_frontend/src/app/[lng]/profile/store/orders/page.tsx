'use client'
import Icon from "@/app/[lng]/UI/Icon";
import OrderRow from "@/app/[lng]/UI/OrderRow";
import React from "react";
import {orderApi} from "@/services/orderService";
import OrderRowForStore from "@/app/[lng]/UI/OrderRowForStore";
import {useT} from "next-i18next/client";


const Page = () => {
    const {t} = useT('profile')
    const {data} = orderApi.useGetOrdersForStoreQuery();
    const [editStatus] = orderApi.useUpdateStatusMutation();
    return (
        <div className="p-8 min-h-screen">
            <div className="flex flex-row gap-2 py-4 mb-4 justify-between items-center ">
                <span className='text-[var(--text)] text-3xl font-semibold'>{t('orders')}</span>
                <Icon width={36} height={36} >
                    <span className='text-[var(--text)] text-xl font-semibold'>{data?.length}</span>
                </Icon>
            </div>
            <div className="flex flex-col gap-3 w-full ">
                {data?.map((order) => (<OrderRowForStore order={order} editableStatus={true} onStatusChange={editStatus} key={order.id} />))}

            </div>
        </div>
    );
};

export default Page;