import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import IAddAddressModel from "@/models/address/IAddAddressModel";
import IAddressModel from "@/models/address/IAddressModel";
import IUpdateAddressModel from "@/models/address/IUpdateAddressModel";



export const addressApi = createApi({
    reducerPath: 'addresses',
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/Address/",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ['Address'],
    endpoints: (build) =>({
        addAdrress: build.mutation<void, IAddAddressModel>({
            query: (model)=>({
                url: 'AddAddress',
                method: "POST",
                body: model
            }),
            invalidatesTags: ['Address']
        }),
        getAddressesByUser: build.query<IAddressModel[], void>({
            query: ()=>({
                url: "GetAddressesByUser",
                method: "GET"
            }),
            providesTags: ['Address']
        }),
        editAddress: build.mutation<void, IUpdateAddressModel>({
            query: (model)=>{
                const {id, ...rest} = model;
                return {
                    url: `UpdateAddress/${id}`,
                    method: "PUT",
                    body: rest
                }
            },
            invalidatesTags: ['Address']
        }),
        deleteAddress: build.mutation<void, string>({
            query: (id)=>({
                url: `DeleteAddress/${id}`,
                method: "Delete"
            }),
            invalidatesTags: ['Address']
        })
    })
})