import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import IColorModel from "@/models/IColorModel";


export const colorApi = createApi({
    reducerPath: 'colorApi',
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/Color/",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ['Color'],
    endpoints: (build) =>({
        getColors: build.query<IColorModel[], string>({
            query: (lng)=>({
                url: "GetColors",
                method: "GET",
                headers: {
                    "Accept-Language": lng,
                },
            }),
            providesTags: ['Color']
        })
    })

})
