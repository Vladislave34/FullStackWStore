import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import IProductModel from "@/models/product/IProductModel";
import IProductProps from "@/models/product/IProductProps";
import IPageResult from "@/models/IPageResult";
import IAddProduct from "@/models/product/IAddProduct";
import ISearchModel from "@/models/product/ISearchModel";


export const productApi = createApi({
    reducerPath: 'productApi',
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/Product/",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ["Product", "StoreProduct"],
    endpoints: (build) => ({
        getProducts: build.query<IPageResult<IProductModel>, IProductProps>({
            query: (model) =>({
                url: "GetProducts",
                method: "GET",
                params: {
                    pageNumber: model.pageNumber,
                    pageSize: model.pageSize
                },
                headers: {
                    "Accept-Language": model.locale,
                }

            }),
            providesTags: ["Product"]
        }),
        getProductsByParams: build.query<IPageResult<IProductModel>, ISearchModel>({
            query: (model) => {
                const params: Record<string, string | number | boolean> = {
                    pageNumber: model.pageNumber,
                    pageSize: model.pageSize,
                };

                if (model.categoryId) params.categoryId = model.categoryId;
                if (model.genderId) params.genderId = model.genderId;
                if (model.sizeId) params.sizeId = model.sizeId;
                if (model.colorId) params.colorId = model.colorId;
                if (model.query) params.query = model.query;
                if (model.hasSale) params.hasSale = model.hasSale;

                return {
                    url: "GetProductsByParams",
                    method: "GET",
                    params,
                    headers: {
                        "Accept-Language": model.locale,
                    },
                };
            },
            providesTags: ["Product"]
        }),
        getProductsByStoreId: build.query<IPageResult<IProductModel>, IProductProps>({
            query: (model) =>({
                url: `GetProductsByStoreId/${model.storeId}`,
                method: "GET",
                params: {
                    categoryId: model.categoryId,
                    pageNumber: model.pageNumber,
                    pageSize: model.pageSize,
                    query: model.query
                },
                headers: {
                    "Accept-Language": model.locale,
                }
            }),
            providesTags: ["StoreProduct"]
        }),
        createProduct: build.mutation<void, IAddProduct>({
            query: (model)=>({
                url: "AddProduct",
                method: "POST",
                body : model
            }),
            invalidatesTags: ["StoreProduct"]

        }),
        deleteProduct: build.mutation<void, {id: string}>({
            query: (model) =>({
                url: `RemoveProduct/${model.id}`,
                method: "DELETE",
            }),
            invalidatesTags: ["StoreProduct", "Product"]
        }),
        editProduct: build.mutation<void, {id: string, model: IAddProduct}>({
            query: (model)=>({
                url: `UpdateProduct/${model.id}`,
                method: "PUT",
                body: model.model
            }),
            invalidatesTags: ["StoreProduct", "Product"]
        }),
        getProductById: build.query<IProductModel, {id:string, lng:string}>({
            query: (model)=>({
                url: `GetProductById/${model.id}`,
                method: 'GET',
                headers: {
                    "Accept-Language": model.lng,
                }
            })
        })
    })
})