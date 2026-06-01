import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import ICategory from "@/models/ICategory";
import API_ENV from "@/env";
import {getLocale} from "@/utils/getLocale";


export const categoryApi = createApi({
    reducerPath: "categoryApi",
    baseQuery: fetchBaseQuery({
        baseUrl: `${API_ENV.API_BASE_URL}/api/Category/`,
    }),
    tagTypes: ["category"],
    endpoints: (build) => ({
        fetchAllCategories: build.query<ICategory[], string>({
            query: (locale) => ({
                url: "Categories",
                method: "GET",
                headers: {
                    "Accept-Language": locale,
                },
            }),
            providesTags : ["category"]
        }),
    }),
});
