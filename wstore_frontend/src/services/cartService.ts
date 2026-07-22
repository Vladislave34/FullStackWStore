import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import ICartModel from "@/models/cart/ICartModel";


export const cartApi  = createApi({
    reducerPath: 'cart',
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/Cart/",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ["Cart"],
    endpoints: (build) => ({
        addCart: build.mutation<string, void>({
            query: ()=>({
                url: "AddCart",
                method: "POST"
            }),
            invalidatesTags: ['Cart']
        }),
        hasCart: build.query<boolean, void>({
            query: ()=>({
                url: 'HasCart',
                method: "GET"
            }),
            providesTags: ['Cart']
        }),
        getCartByUser: build.query<ICartModel, void>({
            query: ()=>({
                url: 'GetCartByUser',
                method: "GET"
            }),
            providesTags: ['Cart']
        })
    })
})