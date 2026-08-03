import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import IJwtResponse from "@/models/auth/IJwtResponse";
import ICreateStore from "@/models/store/ICreateStore";
import {serialize} from "object-to-formdata";
import IEditStoreModel from "@/models/store/IEditStoreModel";
import IStore from "@/models/store/IStore";


export const storeApi = createApi({
    reducerPath: 'storeApi',
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/Store",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ['Store'],
    endpoints: (build) => ({
        getStore: build.query<IStore, void>({
            query: ()=>({
                url: "/GetStoreByUserId",
                method: "GET",
            }),
            providesTags: ['Store']
        }),
        createStore: build.mutation<IJwtResponse, ICreateStore>({
            query: (model) =>{
                const formData = new FormData();
                formData.append("name", model.name);
                formData.append("description", model.description);

                if (model.images?.length) {
                    model.images.forEach(file => formData.append("images", file));
                }

                return { url: "/AddStore", method: "POST", body: formData };
            },
            invalidatesTags: ["Store"]
        }),
        editStore: build.mutation<void, IEditStoreModel>({
            query: (model) => {
                const formData = new FormData();
                formData.append("name", model.name);
                formData.append("description", model.description);


                // model.existingImages?.forEach(url => formData.append("existingImages", url));


                model.images?.forEach(file => formData.append("images", file));

                return { url: `/UpdateStore/${model.id}`, method: "PUT", body: formData, formData: true };
            },
            invalidatesTags: ["Store"]
        })
    }),
    refetchOnFocus: true,
    refetchOnReconnect: true,
})