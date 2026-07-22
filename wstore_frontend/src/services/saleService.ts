import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import ISaleModel from "@/models/sale/ISaleModel";

export const saleApi = createApi({
    reducerPath: "sale",
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/Sales/",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ['Sale'],
    endpoints: (build) =>({
        getAllSales: build.query<ISaleModel[], void>({
            query: ()=>({
                url: 'GetSales',
                method: "GET"
            }),
            providesTags: ['Sale']
        })

    })
})