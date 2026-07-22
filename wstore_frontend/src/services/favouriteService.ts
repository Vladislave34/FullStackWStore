import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import IProductModel from "@/models/product/IProductModel";


export const favouriteApi = createApi({
    reducerPath: 'favourite',
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/Favourite/",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ['favourite'],
    endpoints: build => ({
        addFavourites: build.mutation<void, string>({
            query: (id) => ({
                url: `AddFavourite/${id}`,
                method: "POST"
            }),
            invalidatesTags: (result, error, id) => [
                { type: 'favourite', id },
                { type: 'favourite', id: 'LIST' }
            ]
        }),
        removeFavourites: build.mutation<void, string>({
            query: (id) => ({
                url: `RemoveFavourite/${id}`,
                method: "DELETE"
            }),
            invalidatesTags: (result, error, id) => [
                { type: 'favourite', id },
                { type: 'favourite', id: 'LIST' }
            ]
        }),
        isFavourite: build.query<boolean, string>({
            query: (id) => ({
                url: `IsFavourite/${id}`,
                method: "GET"
            }),
            providesTags: (result, error, id) => [{ type: 'favourite', id }]
        }),
        getFavourites: build.query<IProductModel[], string>({
            query: (lng) => ({
                url: "GetFavourites",
                method: 'GET',
                headers: {
                    "Accept-Language": lng,
                }
            }),
            providesTags: (result) =>
                result
                    ? [
                        ...result.map(({ id }) => ({ type: 'favourite' as const, id })),
                        { type: 'favourite', id: 'LIST' }
                    ]
                    : [{ type: 'favourite', id: 'LIST' }]
        })
    })
})