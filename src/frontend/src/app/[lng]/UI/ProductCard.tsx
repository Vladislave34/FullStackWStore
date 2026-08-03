import {memo, useMemo} from "react";
import Image from "next/image";
import Icon from "@/app/[lng]/UI/Icon";
import SaleLabel from "@/app/[lng]/UI/SaleLabel";
import LikeLabel from "@/app/[lng]/UI/LikeLabel";
import IProductModel from "@/models/product/IProductModel";
import {useRouter} from "next/navigation";
import AddToCartForm from "@/app/[lng]/UI/forms/AddToCartForm";
import Modal from "@/app/[lng]/UI/Modal";
import useModal from "@/hooks/useModal";



const ProductCard = ({product, lng}: {product: IProductModel, lng: string}) => {
    const router = useRouter();
    if (!product.variants || product.variants.length === 0) return null;
    const {isOpen, openModal, closeModal} = useModal()
    const mainVariant = product.variants[0];
    const mainImage = mainVariant.images?.[0];
    const hasSale = useMemo(()=>{

        const saled = product.variants
            ?.filter(variant => variant.sale > 0 )
            .slice()
            .sort((a, b) => b.sale - a.sale);
        if (!saled || saled.length === 0) return "0";
        return saled[0].sale.toString() ?? "0";
    }, [product.variants]);
    const sale = hasSale;

    if (!mainImage) return null;



    return (

        <div
            style={{ background: "var(--card)" }}
            className="rounded-xl w-[calc(25%-12px)] overflow-hidden relative cursor-pointer"
            onClick={() => router.push(`/${lng}/products/${product.id}/productVariants/${mainVariant.id}`)}
        >
            <div className="flex justify-center items-center rounded-t-xl">
                <Image
                    src={mainImage}
                    width={600}
                    height={600}
                    alt={product.name ?? ""}
                    className="w-full h-52 object-cover"
                    unoptimized
                />
            </div>
            <div className="flex flex-row justify-between p-4">
                <div className="flex flex-col">
                    <p style={{ color: "var(--text)" }}>{product.name}</p>
                    <p style={{ color: "var(--muted)" }}>{product.category}</p>
                    <p style={{ color: "var(--price)" }} className="mt-2">
                        {mainVariant.price - ( mainVariant.price * (mainVariant.sale/100))}₴
                    </p>
                </div>

            </div>
            <SaleLabel sale={sale}  />
            <LikeLabel  id={product.id} />
        </div>


    );
};

export default memo(ProductCard);