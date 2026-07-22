import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import IAddCartItemModel from "@/models/cartitem/IAddCartItemModel";
import ICartitemDetailModel from "@/models/cartitem/ICartitemDetailModel";


export const cartItemApi = createApi({
    reducerPath: 'cartItem',
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/CartItem/",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ["CartItem"],
    endpoints: (build) => ({
        addToCart: build.mutation<void, IAddCartItemModel>({
            query: (model)=>({
                url: 'AddCartItem',
                method: 'POST',
                body: model
            }),
            invalidatesTags: ['CartItem']
        }),
        getCartItemsByUser: build.query<ICartitemDetailModel[], void>({
            query: ()=>({
                url: "GetAllCartItemsByUser",
                method: "GET"
            }),
            providesTags: ['CartItem']
        })
    })
})