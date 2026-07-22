import BackgroundCarousel from "@/app/[lng]/components/BackgroundCarousel";
import ProductInfo from "@/app/[lng]/UI/ProductInfo";
import {productVariantApi} from "@/services/productVariantService";
import ProductPageBody from "@/app/[lng]/components/ProductPageBody";



export default async  function Page({params}: {params: Promise<{lng : string}>}) {
    const {lng} = await params


    return (
        <div className="flex justify-center items-center w-screen h-screen relative">
            <ProductPageBody lng={lng} />
        </div>
    );
};

