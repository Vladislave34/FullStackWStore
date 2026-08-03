'use client'
import ProductVariantEditForm from "@/app/[lng]/UI/forms/ProductVariantEditForm";
import {useParams, usePathname} from "next/navigation";
import {productVariantApi} from "@/services/productVariantService";
import {useAppDispatch} from "@/hooks/redux";


const Page = () => {

    const {productId , productVariantId} = useParams();
    const{data} = productVariantApi.useGetProductVariantByIdQuery(productVariantId as string) ;
    const [edit] = productVariantApi.useEditProductVariantMutation();
    const pathname = usePathname();
    const lng = pathname.split("/")[1];

    if (!data) return null;


    return (
        <ProductVariantEditForm
            productId={productId as string}
            productVariant={data}
            onSubmit={async (values) => await edit(values).unwrap()}
            lng={lng}
        />
    );
};

export default Page;