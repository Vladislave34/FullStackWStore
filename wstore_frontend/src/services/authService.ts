import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import API_ENV from "@/env";
import {serialize} from "object-to-formdata";
import IRegisterModel from "@/models/auth/IRegisterModel";
import ILoginModel from "@/models/auth/ILoginModel";
import IJwtResponse from "@/models/auth/IJwtResponse";
import IEditProfileModel from "@/models/auth/IEditProfileModel";

export const authApi = createApi({
    reducerPath: 'authApi',
    baseQuery: fetchBaseQuery({
        baseUrl: `/api/User`,
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ['Auth'],
    endpoints: (build) => ({
        register: build.mutation<IJwtResponse, IRegisterModel>({
            query: (model)=>{
                const formData = serialize(model)
                return {
                    url: "/register",
                    method: "POST",
                    body: formData,
                }
            },
            invalidatesTags: ["Auth"]
        }),
        login: build.mutation<IJwtResponse, ILoginModel>({
            query: (model)=>{

                return{
                    url: "/login",
                    method: "POST",
                    body: model,
                }
            },
            invalidatesTags: ["Auth"]
        }),
        editProfile: build.mutation<IJwtResponse, IEditProfileModel>({
            query: (model) => {
                const formData = serialize(model)
                return {
                    url: "/EditProfile",
                    method: "POST",
                    body: formData,
                }

            },
            invalidatesTags: ["Auth"],
        }),
        loginByGoogle: build.mutation<IJwtResponse, string>({
            query: (formData) => ({
                url: '/GoogleLogin',
                method: 'POST',
                body: {idToken : formData},
            }),
            invalidatesTags: ['Auth'],
        })
    })
})

export const { useRegisterMutation, useLoginMutation, useEditProfileMutation, useLoginByGoogleMutation } = authApi;