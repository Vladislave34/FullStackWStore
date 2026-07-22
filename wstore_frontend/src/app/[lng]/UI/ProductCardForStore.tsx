"use client"

import {FC, useState} from "react";
import { FaCircleArrowRight} from "react-icons/fa6";
import IProductModel from "@/models/product/IProductModel";
import {IoIosArrowForward} from "react-icons/io";
import Button from "@/app/[lng]/UI/Button";
import ProductVariantAddFrom from "@/app/[lng]/UI/forms/ProductVariantAddFrom";
import {useT} from "next-i18next/client";
import {productVariantApi} from "@/services/productVariantService";
import ProductVariantRow from "@/app/[lng]/UI/ProductVariantRow";
import Pagination from "@/app/[lng]/components/Pagination";
import {createPages} from "../../../../util/pageCreator";
import {productApi} from "@/services/productService";
import useModal from "@/hooks/useModal";
import EditProductForm from "@/app/[lng]/UI/forms/EditProductForm";
import Modal from "@/app/[lng]/UI/Modal";

interface ProductCardForStoreProps {
    product: IProductModel
    lng: string
}

const ProductCardForStore : FC<ProductCardForStoreProps> = ({product, lng}) => {
    const [isOpen, setIsOpen] = useState(false);
    const [isOpenId, setIsOpenId] = useState(false);
    const [copied, setCopied] = useState(false);
    const [pos, setPos] = useState({ x: 0, y: 0 });

    // ✅ локальна пагінація для варіантів
    const [variantPage, setVariantPage] = useState(1);

    const [deleteProduct] = productApi.useDeleteProductMutation();
    const {data} = productVariantApi.useGetProductVariantByProductIdQuery(product.id);

    const {t} = useT('product_card_for_store');
    const {
        isOpen: isOpenModalEditProduct,
        closeModal: closeModalEditProduct,
        openModal: openModalEditProduct
    } = useModal();
    const {
        isOpen: isOpenModalAddVariant,
        closeModal: closeModalAddVariant,
        openModal: openModalAddVariant
    } = useModal();

    const handleMouseMove = (e: React.MouseEvent) => {
        setPos({ x: e.clientX, y: e.clientY });
    };

    const handleCopy = () => {
        navigator.clipboard.writeText(product.id);
        setCopied(true);
        setTimeout(() => setCopied(false), 2000);
    };



    return (
        <>
            <div className="w-full bg-[var(--surface)] rounded-xl overflow-hidden border-[var(--border)] border-2">
                <div className="p-6 flex flex-row items-start justify-between">
                    <div className="flex flex-row gap-2">
                        <p onClick={() => setIsOpenId(prev => !prev)}>
                            {isOpenId ? (
                                <>
                                <span
                                    className="flex flex-row items-center gap-1 cursor-pointer"
                                    onMouseMove={handleMouseMove}
                                >
                                    <span onClick={handleCopy}>{product.id}</span>
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
                            ) : (
                                <span className="flex flex-row items-center">
                                {product.id.slice(0, 3) + "..."}
                                    <IoIosArrowForward size={16} />
                            </span>
                            )}
                        </p>
                        <p>{product.name}</p>
                        <p>{product.category}</p>
                        <p>{lng === "en" ? product.gender : product.genderUk}</p>
                    </div>
                    <div onClick={() => setIsOpen(prev => !prev)}>
                        <FaCircleArrowRight
                            size={24}
                            color={"var(--accent)"}
                            className={`transition-transform duration-300 ${isOpen ? "rotate-90" : "rotate-0"}`}
                        />
                    </div>
                </div>

                <div className={`grid transition-all duration-300 ${isOpen ? "grid-rows-[1fr]" : "grid-rows-[0fr]"}`}>
                    <div className="overflow-hidden">
                        <div className="px-6 py-3 pb-6 flex flex-row justify-between">
                            <Button handleClick={() => openModalAddVariant()}>
                                {t('add_variant')}
                            </Button>
                            <Button handleClick={() => openModalEditProduct()}>
                                {t('edit_product')}
                            </Button>
                            <Button handleClick={async () => await deleteProduct({id: product.id}).unwrap()}>
                                {t('delete_product')}
                            </Button>
                        </div>

                        <div className="flex flex-col gap-1 px-6 py-3 pb-6 max-h-64 overflow-y-auto scrollbar-thin scrollbar-thumb-[var(--border)] scrollbar-track-transparent">
                            {data?.map(productVariant =>
                                <ProductVariantRow
                                    productVariant={productVariant}
                                    key={productVariant.id}
                                    lng={lng}
                                    productId={product.id}
                                />
                            )}
                        </div>
                    </div>
                </div>
            </div>

            <Modal isOpen={isOpenModalEditProduct} closeModal={closeModalEditProduct}>
                <EditProductForm lng={lng} closeModal={closeModalEditProduct} product={product} />
            </Modal>
            <Modal isOpen={isOpenModalAddVariant} closeModal={closeModalAddVariant}>
                <ProductVariantAddFrom lng={lng} closeModal={closeModalAddVariant} productId={product.id} />
            </Modal>
        </>
    );
};

export default ProductCardForStore;