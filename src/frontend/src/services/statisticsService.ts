import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import ISizeModel from "@/models/ISizeModel";
import IStatisticsModel from "@/models/statistics/IStatisticsModel";
import IStatisticByCategoryModel from "@/models/statistics/IStatisticByCategoryModel";


export const statisticsApi = createApi({
    reducerPath: 'Statistic',
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/Statistic/",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ['Statistic', 'StatisticByCategory'],
    endpoints: (build) =>({
        getStatistics: build.query<IStatisticsModel[], number>({
            query: (days)=>({
                url: "GetStatisticsByTime",
                method: "GET",
                params: {days: days}
            }),
            providesTags: ['Statistic']
        }),
        getStatisticsByCategory: build.query<IStatisticByCategoryModel[], void>({
            query: ()=>({
                url: "GetStatisticsByCategory",
                method: "GET",
            })
        })

    })

})