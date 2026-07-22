'use client'
import {productApi} from "@/services/productService";
import {useSearchParams} from "next/navigation";
import ProductCard from "@/app/[lng]/UI/ProductCard";
import Pagination from "@/app/[lng]/components/Pagination";
import {createPages} from "../../../../util/pageCreator";


const ProductsGrid = ({lng} : {lng:string}) => {
    const nums : number[] = [];

    const searchParams = useSearchParams();
    const currentPage = Number(searchParams.get("page")) || 1;
    const hasSale = searchParams.get("hasSale") === "true" ? true : null;
    const query = searchParams.get("query");
    const genderId = searchParams.get("genderId");
    const categoryId = searchParams.get("categoryId");
    const colorId = searchParams.get("colorId");
    const sizeId = searchParams.get("sizeId");

    const {data , isLoading} = productApi.useGetProductsByParamsQuery(
        {
            categoryId: categoryId,
            genderId: genderId,
            colorId: colorId,
            sizeId: sizeId,
            query: query,
            hasSale: hasSale,
            pageNumber: currentPage,
            pageSize: 8,
            locale: lng
        }
    )
    if(data){createPages(nums, data.totalPages, currentPage)}


    return (
        <>
        <div className="flex flex-wrap gap-2">
            {data && data.data.map(product => <ProductCard key={product.id} product={product} lng={lng} />)}
        </div>
            <div className="flex justify-center">
                <Pagination currentPage={currentPage} nums={nums} />
            </div>

        </>
    );
};

export default ProductsGrid;