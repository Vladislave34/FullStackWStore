'use client'
import ProductCardForStore from "@/app/[lng]/UI/ProductCardForStore";
import {productApi} from "@/services/productService";
import {useEffect, useState} from "react";
import Pagination from "@/app/[lng]/components/Pagination";
import {useAppDispatch, useAppSelector} from "@/hooks/redux";
import {useSearchParams} from "next/navigation";
import {createPages} from "../../../../util/pageCreator";
import {storeApi} from "@/services/storeService";
import {setCountForStore} from "@/store/reducers/productSlice";


const ProductList = ({locale}: {locale: string}) => {
    const {data: store} = storeApi.useGetStoreQuery();
    const dispatch = useAppDispatch();
    const  category = useAppSelector(x=>x.searchSlice.category);
    const query = useAppSelector(x=>x.searchSlice.query);
    const searchParams = useSearchParams();
    const currentPage = Number(searchParams.get("page")) || 1;

    const {data} = productApi.useGetProductsByStoreIdQuery(
        {
            pageNumber: currentPage, pageSize: 8,
            locale: locale, storeId: store?.id ?? "",
            categoryId: category?.id, query: query
        },
        {skip: !store?.id}
    );
    useEffect(() => {
        if (data) {
            dispatch(setCountForStore(data.data.length));
        }
    }, [data]);
    const nums : number[] = [];
    if (!data?.totalPages) {
        return null;
    }
    createPages(nums, data!.totalPages, currentPage)
    return (
        <>
        <div className="flex flex-col justify-center items-center gap-2 w-full counter">
            {data?.data.map(product =>
                <ProductCardForStore key={product.id} product={product} lng={locale} />)
            }
        </div>
        <Pagination
            nums={nums}
            currentPage={currentPage}
        />
        </>
    );
};

export default ProductList;