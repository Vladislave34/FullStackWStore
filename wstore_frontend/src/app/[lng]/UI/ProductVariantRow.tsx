'use client'
import IProductVariantModel from "@/models/product/variant/IProductVariantModel";
import {useState} from "react";
import {IoIosArrowForward} from "react-icons/io";
import Icon from "@/app/[lng]/UI/Icon";
import {MdDelete, MdEdit} from "react-icons/md";
import {productVariantApi} from "@/services/productVariantService";
import {useRouter} from "next/navigation";


const ProductVariantRow = ({productVariant, lng, productId} : {productVariant: IProductVariantModel, lng: string, productId: string}) => {
    const [isOpenId, setIsOpenId] = useState(false);
    const [copied, setCopied] = useState(false);
    const [pos, setPos] = useState({ x: 0, y: 0 });
    const [deleteProductVariant] = productVariantApi.useDeleteProductVariantMutation();
    const handleMouseMove = (e: React.MouseEvent) => {
        setPos({ x: e.clientX, y: e.clientY });
    };
    const router = useRouter();

    const handleCopy = (e: React.MouseEvent) => {
        e.stopPropagation();
        navigator.clipboard.writeText(productVariant.id);
        setCopied(true);
        setTimeout(() => setCopied(false), 2000);
    };
    return (
        <div className="flex flex-row gap-2 bg-[var(--surface)] rounded-xl overflow-hidden border-[var(--border)] border-2 p-4 justify-between items-center">
            <div className="flex flex-row gap-4 items-center">
            <span
                onClick={()=>setIsOpenId(prev=>!prev)}
            >
                {isOpenId ?
                    <>
                      <span
                          className="flex flex-row items-center gap-1 cursor-pointer"

                          onMouseMove={handleMouseMove}
                      >
                          <span onClick={handleCopy}>{productVariant.id}</span>
                          <IoIosArrowForward size={16} className="rotate-180" />
                      </span>

                        {typeof window !== "undefined" && (
                            <div
                                className="fixed pointer-events-none z-50 text-xs bg-black text-white px-2 py-1 rounded"
                                style={{ left: pos.x + 12, top: pos.y + 12 }}
                            >
                                {copied ? "Copied!" : "Copy"}
                            </div>
                        )}
                    </>
                    :
                    <span className="flex flex-row items-center ">
                                {productVariant.id.slice(0, 3)+"..."}
                        <IoIosArrowForward size={16}  />
                            </span>

                }
            </span>
            <span>{lng === "en" ? productVariant.colorName : productVariant.colorNameUk}</span>
            <span>{productVariant.sizeName}</span>
            <span>{productVariant.price}</span>
            </div>
            <div className="flex flex-row gap-4 items-center">
                <Icon height={32} width={32}  >
                    <MdEdit size={22} color="var(--accent-mid)" onClick={()=>router.push(`/${lng}/profile/store/products/${productId}/productVariants/${productVariant.id}`)} />
                </Icon>
                <Icon height={32} width={32}   >
                    <MdDelete size={22} color="var(--accent-mid)" onClick={() => deleteProductVariant(productVariant.id)} />
                </Icon>

            </div>
        </div>
    );
};

export default ProductVariantRow;