import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import IAddPaymentModel from "@/models/payment/IAddPaymentModel";
import IPaymentModel from "@/models/payment/IPaymentModel";
import IUpdatePaymentModel from "@/models/payment/IUpdatePaymentModel";


export const paymentApi = createApi({
    reducerPath: 'payments',
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/Payment/",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ['Payment'],
    endpoints: (build) =>({
        addCard: build.mutation<void, IAddPaymentModel>({
            query: (model)=>({
                url: 'AddCard',
                method: "POST",
                body: model
            }),
            invalidatesTags: ['Payment']
        }),
        getCardsByUser: build.query<IPaymentModel[], void>({
            query: ()=>({
                url: "GetCardsByUser",
                method: "GET"
            }),
            providesTags: ['Payment']
        }),
        editCard: build.mutation<void, IUpdatePaymentModel>({
            query: (model)=>{
                const {id, ...rest} = model;
                return {
                    url: `UpdateCard/${id}`,
                    method: "PUT",
                    body: rest
                }
            },
            invalidatesTags: ['Payment']
        }),
        deleteCard: build.mutation<void, string>({
            query: (id)=>({
                url: `DeleteCard/${id}`,
                method: "Delete"
            }),
            invalidatesTags: ['Payment']
        })
    })
})