import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import IJwtResponse from "@/models/auth/IJwtResponse";
import ICreateStore from "@/models/store/ICreateStore";
import {serialize} from "object-to-formdata";
import IEditStoreModel from "@/models/store/IEditStoreModel";
import IStore from "@/models/store/IStore";


export const storeApi = createApi({
    reducerPath: 'storeApi',
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/store",
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
                url: "/StoreByUserId",
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

                return { url: "/Add", method: "POST", body: formData };
            },
            invalidatesTags: ["Store"]
        }),
        editStore: build.mutation<void, IEditStoreModel>({
            query: (model) => {
                const formData = new FormData();
                formData.append("name", model.name);
                formData.append("description", model.description);
                if (model.images?.length) {
                    model.images.forEach(file => formData.append("images", file));
                }
                for (const [key, value] of formData.entries()) {

                    console.log(key, value);

                }


                return { url: `/Update/${model.id}`, method: "PUT", body: formData, formData: true };
            },
            invalidatesTags: ["Store"]
        })
    }),
    refetchOnFocus: true,
    refetchOnReconnect: true,
})