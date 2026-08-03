import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";

import ISizeModel from "@/models/ISizeModel";


export const sizeApi = createApi({
    reducerPath: 'sizeApi',
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/Size/",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ['Size'],
    endpoints: (build) =>({
        getSizes: build.query<ISizeModel[], void>({
            query: ()=>({
                url: "GetSizes",
                method: "GET",
            }),
            providesTags: ['Size']
        })
    })

})
