import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";

import IAddOrderModel from "@/models/order/IAddOrderModel";
import Order from "@/models/order/IOrderModel";

export const orderApi = createApi({
    reducerPath: "orderApi",
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/Order/",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            headers.set("Content-Type", "application/json");
            return headers;
        },
    }),
    tagTypes: ["Order"],
    endpoints: (builder) => ({
        addOrder: builder.mutation<void, IAddOrderModel>({
            query: (model) => ({
                url: "AddOrder",
                method: "POST",
                body: model,
            }),
            invalidatesTags: ["Order"],
        }),
        getMyOrders: builder.query<Order[], void>({
            query: ()=>({
                url: 'GetMyOrders',
                method: "GET"
            }),
            providesTags: ['Order']
        }),
        getOrdersForStore: builder.query<Order[], void>({
            query: ()=>({
                url: "GetOrdersByStore",
                method: "GET"
            }),
            providesTags: ['Order']
        }),
        updateStatus: builder.mutation<void, {id: string, status: string}>({
            query: (model)=>({
                url: `UpdateOrderStatus/${model.id}`,
                method: "PUT",
                body: {status: model.status}
            }),
            invalidatesTags: ['Order']
        })
    }),
});