"use client";

import { usePathname } from "next/navigation";
import { useMemo } from "react";
import {productApi} from "@/services/productService";
import ProductInfo from "@/app/[lng]/UI/ProductInfo";
import BackgroundCarousel from "@/app/[lng]/components/BackgroundCarousel";

const ProductPageBody = ({ lng }: { lng: string }) => {
    const pathname = usePathname();
    const paths = pathname.split("/");

    const id = paths[paths.length - 3];
    const variantId = paths[paths.length - 1];

    const { data } = productApi.useGetProductByIdQuery({ id, lng });

    const activeVariant = useMemo(() => {
        if (!data?.variants?.length) return undefined;
        return (
            data.variants.find((v) => v.id === variantId) ?? data.variants[0]
        );
    }, [data, variantId]);

    if (!data) return null; // ще завантажується

    if (!activeVariant) {
        return <ProductInfo data={data} lng={lng} />; // товар без варіантів
    }

    return (
        <>
            <ProductInfo data={data} lng={lng} initialVariantId={variantId} />
            <BackgroundCarousel data={activeVariant} />
        </>
    );
};

export default ProductPageBody;