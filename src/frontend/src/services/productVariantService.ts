import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import IAddProductVarinat from "@/models/product/variant/IAddProductVarinat";
import {string} from "yup";
import IProductVariantModel from "@/models/product/variant/IProductVariantModel";
import IEditProductVariantModel from "@/models/product/variant/IEditProductVariantModel";
import IPageResult from "@/models/IPageResult";
import IProductVariantProps from "@/models/product/variant/IProductVariantProps";
import ICartitemDetailModel from "@/models/cartitem/ICartitemDetailModel";


export const productVariantApi= createApi({
    reducerPath: 'productVariant',
    baseQuery: fetchBaseQuery({
        baseUrl: "/api/ProductVariant/",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem('accessToken');
            if (token) headers.set('Authorization', `Bearer ${token}`);
            return headers;
        },
    }),
    tagTypes: ['productVariant'],
    endpoints: (build)=>({
        addProductVariant: build.mutation<void, IAddProductVarinat>({
            query: (model) =>{
                const formData = new FormData();
                formData.append("productId", model.productId);
                formData.append("colorId", model.colorId);
                formData.append("sizeId", model.sizeId);

                formData.append("price", model.price.toString());
                if (model.saleId) {
                    formData.append("saleId", model.saleId);
                }
                if(model.images){
                    model.images.forEach(img=>formData.append("images", img))
                }
                return{
                    url: "AddProductVariant",
                    method: "POST",
                    body: formData
                }
            },
            invalidatesTags: ['productVariant']
        }),
        getProductVariantByProductId: build.query<IProductVariantModel[], string>({
            query: (productId)=>({
                url: `GetProductVariants/${productId}`,
                method: "GET",

            }),
            providesTags: ['productVariant']
        }),
        deleteProductVariant: build.mutation<void, string>({
            query: (id)=>({
                url: `DeleteProductVariant/${id}`,
                method: "DELETE"
            }),
            invalidatesTags: ['productVariant']
        }),
        getProductVariantById: build.query<IProductVariantModel, string>({
            query: (id)=>({
                url: `GetProductVariantById/${id}`,
                method: "GET"
            }),
            providesTags: ['productVariant']
        }),
        editProductVariant: build.mutation<void, IEditProductVariantModel>({
            query: (model) => {
                const formData = new FormData();


                formData.append("productId", model.productId);
                formData.append("colorId", model.colorId);
                formData.append("sizeId", model.sizeId);
                formData.append("price", model.price.toString());
                if (model.saleId) {
                    formData.append("saleId", model.saleId);
                }
                model.images.forEach((file) => {
                    formData.append("images", file);
                });

                return {
                    url: `UpdateProductVariant/${model.id}`,
                    method: "PUT",
                    body: formData,
                };
            },
            invalidatesTags: ["productVariant"],
        })
    })
})