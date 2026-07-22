import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import IGenderModel from "@/models/gender/IGenderModel";



export const genderApi = createApi({
    reducerPath: "gender",
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/Gender/",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ['Gender'],
    endpoints: (build) =>({
        getAllGenders: build.query<IGenderModel[], string>({
            query: (lng)=>({
                url: "GetGenders",
                method: "GET",
                headers: {
                    "Accept-Language": lng,
                },
            }),
            providesTags: ['Gender']
        })
    })
})