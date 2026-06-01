'use client'
import ProductCard from "@/UI/ProductCard";
import {useAppSelector} from "@/hooks/redux";



const ProductList = () => {
    const data = useAppSelector(state => state.authSlice.user);
    console.log(data)
    return (
        <div className="mt-4 flex flex-wrap gap-4">
            <ProductCard />
            <ProductCard />
            <ProductCard />
            <ProductCard />

        </div>
    );
};

export default ProductList;