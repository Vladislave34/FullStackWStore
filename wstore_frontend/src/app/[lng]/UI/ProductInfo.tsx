// "use client";
//
// import {useEffect, useMemo, useState} from "react";
// import IProductModel from "@/models/product/IProductModel";
// import IProductVariantModel from "@/models/product/variant/IProductVariantModel";
// import {useDispatch} from "react-redux";
// import {useAppDispatch} from "@/hooks/redux";
// import {setProductVariantId} from "@/store/reducers/productVariantSlice";
// import {usePathname, useRouter} from "next/navigation";
//
//
//
//
// // парсить розміри типу XXS/XS/S/M/L/XL/XXL/XXXL, або числові "42"/"44"
// const parseSize = (s: string) => {
//     const match = s.match(/^(X*)(S|M|L)$/i);
//     if (!match) return null;
//     const [, xs, base] = match;
//     return { base: base.toUpperCase(), xCount: xs.length };
// };
//
// const compareSizes = (a: string, b: string) => {
//     const pa = parseSize(a);
//     const pb = parseSize(b);
//
//     if (pa && pb) {
//         const rank = (p: NonNullable<typeof pa>) => {
//             if (p.base === "S") return -p.xCount;
//             if (p.base === "M") return 100;
//             return 200 + p.xCount; // "L"
//         };
//         return rank(pa) - rank(pb);
//     }
//
//     const na = Number(a);
//     const nb = Number(b);
//     if (!isNaN(na) && !isNaN(nb)) return na - nb;
//
//     return a.localeCompare(b);
// };
//
// const ProductInfo = ({ data, lng }: { data: IProductModel; lng?: string }) => {
//     const dispatch = useAppDispatch();
//     const router = useRouter();
//     const pathname = usePathname();
//     const isUk = lng !== "en";
//     const variants = data?.variants ?? [];
//
//     const colorKey = (v: IProductVariantModel) => (isUk ? v.colorNameUk : v.colorName);
//
//     // всі кольори з variants, дедуплікація + стабільне сортування за назвою
//     const allColors = useMemo(() => {
//         const seen = new Map<string, IProductVariantModel>();
//         variants.forEach((v) => {
//             const key = colorKey(v);
//             if (!seen.has(key)) seen.set(key, v);
//         });
//         return Array.from(seen.entries()).sort(([a], [b]) =>
//             a.localeCompare(b, isUk ? "uk" : "en")
//         );
//     }, [variants, isUk]);
//
//     // всі розміри з variants, без хардкоду, з коректним сортуванням
//     const allSizes = useMemo(() => {
//         const unique = Array.from(new Set(variants.map((v) => v.sizeName)));
//         return unique.sort(compareSizes);
//     }, [variants]);
//
//     // ручний вибір користувача (undefined = ще не обирав)
//     const [manualColor, setManualColor] = useState<string | undefined>();
//     const [manualSize, setManualSize] = useState<string | undefined>();
//
//     // ефективні значення — завжди похідні від актуальних variants,
//     // тому не "заморожуються", коли дані приїжджають асинхронно
//     const selectedColor = manualColor ?? allColors[0]?.[0];
//     const selectedSize = manualSize ?? allSizes[0];
//
//     // розміри, доступні для ОБРАНОГО кольору
//     const sizesForSelectedColor = useMemo(
//         () =>
//             new Set(
//                 variants.filter((v) => colorKey(v) === selectedColor).map((v) => v.sizeName)
//             ),
//         [variants, selectedColor, isUk]
//     );
//
//     // кольори, доступні для ОБРАНОГО розміру
//     const colorsForSelectedSize = useMemo(
//         () =>
//             new Set(
//                 variants.filter((v) => v.sizeName === selectedSize).map((v) => colorKey(v))
//             ),
//         [variants, selectedSize, isUk]
//     );
//
//     const activeVariant = useMemo(
//         () =>
//             variants.find(
//                 (v) => colorKey(v) === selectedColor && v.sizeName === selectedSize
//             ),
//         [variants, selectedColor, selectedSize, isUk]
//     );
//     useEffect(() => {
//         if (!activeVariant) return;
//
//         const segments = pathname.split("/");
//         const currentLast = segments[segments.length - 1];
//
//         if (currentLast === activeVariant.id) return;
//
//         segments[segments.length - 1] = activeVariant.id;
//         router.replace(segments.join("/"), { scroll: false });
//     }, [activeVariant, pathname, router]);
//
//     const handleColorClick = (color: string) => {
//         setManualColor(color);
//         const availableSizes = new Set(
//             variants.filter((v) => colorKey(v) === color).map((v) => v.sizeName)
//         );
//         if (!selectedSize || !availableSizes.has(selectedSize)) {
//             const fallback = allSizes.find((s) => availableSizes.has(s));
//             setManualSize(fallback);
//         }
//     };
//
//     const handleSizeClick = (size: string) => {
//         setManualSize(size);
//         const availableColors = new Set(
//             variants.filter((v) => v.sizeName === size).map((v) => colorKey(v))
//         );
//         if (!selectedColor || !availableColors.has(selectedColor)) {
//             const fallback = allColors.find(([c]) => availableColors.has(c))?.[0];
//             setManualColor(fallback);
//         }
//     };
//
//     const finalPrice = activeVariant
//         ? activeVariant.price - (activeVariant.sale ?? 0)
//         : undefined;
//     const hasSale = !!activeVariant?.sale;
//
//     return (
//         <div
//             className="absolute bottom-1/5 right-1/8 z-999 w-[380px]
//         bg-[var(--card)] rounded-xl p-6 flex flex-col gap-4 shadow-lg"
//         >
//             {/* Назва + ціна */}
//             <div className="flex items-start justify-between gap-4">
//                 <h2 className="font-bold uppercase tracking-tight text-lg leading-snug">
//                     {isUk ? data?.nameUk : data?.name}
//                 </h2>
//                 <div className="flex flex-col items-end shrink-0">
//                     {hasSale && (
//                         <span className="text-sm line-through text-[var(--muted-foreground)]">
//               {activeVariant?.price} грн
//             </span>
//                     )}
//                     <span className="font-bold text-lg whitespace-nowrap">
//             {finalPrice} грн
//           </span>
//                 </div>
//             </div>
//
//             {/* Опис */}
//             <p className="text-sm text-[var(--muted-foreground)] line-clamp-2">
//                 {isUk ? data?.descriptionUk : data?.description}
//             </p>
//
//             {/* Кольори */}
//             {allColors.length > 0 && (
//                 <div className="flex flex-col gap-2">
//                     <div className="flex gap-2">
//                         {/* Кольори */}
//                         {allColors.map(([label, variant]) => {
//                             const unavailable = !colorsForSelectedSize.has(label);
//                             return (
//                                 <button
//                                     key={label}
//                                     type="button"
//                                     onClick={() => handleColorClick(label)}
//                                     className={`w-10 h-10 rounded border-2 overflow-hidden relative
//         ${selectedColor === label ? "border-blue-500" : "border-transparent"}
//         ${unavailable ? "opacity-30" : ""}`}
//                                     aria-label={label}
//                                 >
//                                     {variant.images?.[0] ? (
//                                         // eslint-disable-next-line @next/next/no-img-element
//                                         <img src={variant.images[0]} alt={label} className="w-full h-full object-cover" />
//                                     ) : (
//                                         <div className="w-full h-full bg-[var(--muted)]" />
//                                     )}
//                                 </button>
//                             );
//                         })}
//                     </div>
//                     <span className="text-xs uppercase tracking-wide text-[var(--muted-foreground)]">
//             {selectedColor}
//           </span>
//                 </div>
//             )}
//
//             <div className="h-px bg-[var(--border)]" />
//
//             {/* Розміри */}
//             {allSizes.length > 0 && (
//                 <div className="flex flex-col gap-2">
//           <span className="font-semibold uppercase text-sm">
//               Розміри
//           </span>
//                     <div className="flex gap-3">
//                         {/* Розміри */}
//                         {allSizes.map((size) => {
//                             const unavailable = !sizesForSelectedColor.has(size);
//                             return (
//                                 <button
//                                     key={size}
//                                     type="button"
//                                     onClick={() => handleSizeClick(size)}
//                                     className={`font-semibold text-sm ${
//                                         selectedSize === size ? "underline underline-offset-4" : "text-[var(--muted-foreground)]"
//                                     } ${unavailable ? "opacity-30" : ""}`}
//                                 >
//                                     {size}
//                                 </button>
//                             );
//                         })}
//                     </div>
//                 </div>
//             )}
//
//             {/* Кнопка */}
//             <button
//                 type="button"
//                 disabled={!activeVariant}
//                 className="mt-2 bg-[var(--foreground)] text-[var(--background)]
//           font-bold uppercase tracking-wide py-3 rounded-md
//           disabled:opacity-40 transition-opacity"
//             >
//                 Додати у кошик
//             </button>
//         </div>
//     );
// };
//
// export default ProductInfo;

"use client";

import {useEffect, useMemo, useState} from "react";
import IProductModel from "@/models/product/IProductModel";
import IProductVariantModel from "@/models/product/variant/IProductVariantModel";
import {useAppDispatch} from "@/hooks/redux";
import {usePathname, useRouter} from "next/navigation";
import {cartApi} from "@/services/cartService";
import {cartItemApi} from "@/services/cartItemService";
import IAddCartItemModel from "@/models/cartitem/IAddCartItemModel";
import useModal from "@/hooks/useModal";
import Modal from "@/app/[lng]/UI/Modal";
import AddToCartForm from "@/app/[lng]/UI/forms/AddToCartForm";


// парсить розміри типу XXS/XS/S/M/L/XL/XXL/XXXL, або числові "42"/"44"
const parseSize = (s: string) => {
    const match = s.match(/^(X*)(S|M|L)$/i);
    if (!match) return null;
    const [, xs, base] = match;
    return { base: base.toUpperCase(), xCount: xs.length };
};

const compareSizes = (a: string, b: string) => {
    const pa = parseSize(a);
    const pb = parseSize(b);

    if (pa && pb) {
        const rank = (p: NonNullable<typeof pa>) => {
            if (p.base === "S") return -p.xCount;
            if (p.base === "M") return 100;
            return 200 + p.xCount; // "L"
        };
        return rank(pa) - rank(pb);
    }

    const na = Number(a);
    const nb = Number(b);
    if (!isNaN(na) && !isNaN(nb)) return na - nb;

    return a.localeCompare(b);
};

const ProductInfo = ({
                         data,
                         lng,
                         initialVariantId,
                     }: {
    data: IProductModel;
    lng?: string;
    initialVariantId?: string;
}) => {

    const router = useRouter();
    const pathname = usePathname();
    const isUk = lng !== "en";
    const variants = data?.variants ?? [];
    const {isOpen, openModal, closeModal}  =  useModal();
    const colorKey = (v: IProductVariantModel) => (isUk ? v.colorNameUk : v.colorName);

    // всі кольори з variants, дедуплікація + стабільне сортування за назвою
    const allColors = useMemo(() => {
        const seen = new Map<string, IProductVariantModel>();
        variants.forEach((v) => {
            const key = colorKey(v);
            if (!seen.has(key)) seen.set(key, v);
        });
        return Array.from(seen.entries()).sort(([a], [b]) =>
            a.localeCompare(b, isUk ? "uk" : "en")
        );
    }, [variants, isUk]);

    // всі розміри з variants, без хардкоду, з коректним сортуванням
    const allSizes = useMemo(() => {
        const unique = Array.from(new Set(variants.map((v) => v.sizeName)));
        return unique.sort(compareSizes);
    }, [variants]);

    // гарантовано валідний варіант "за замовчуванням" — беремо реальний
    // variant (по можливості той, що прийшов у URL), а не колір і розмір
    // окремо — інакше комбінація "перший колір + перший розмір" може
    // просто не існувати серед variants
    const defaultVariant = useMemo(() => {
        if (!variants.length) return undefined;
        if (initialVariantId) {
            const found = variants.find((v) => v.id === initialVariantId);
            if (found) return found;
        }
        return variants[0];
    }, [variants, initialVariantId]);

    // ручний вибір користувача (undefined = ще не обирав)
    const [manualColor, setManualColor] = useState<string | undefined>();
    const [manualSize, setManualSize] = useState<string | undefined>();

    // при зміні товару скидаємо ручний вибір, щоб не тягнути
    // колір/розмір попереднього товару
    useEffect(() => {
        setManualColor(undefined);
        setManualSize(undefined);
    }, [data?.id]);

    // ефективні значення — похідні від актуальних variants,
    // з фолбеком на гарантовано валідну пару з defaultVariant
    const selectedColor =
        manualColor ?? (defaultVariant ? colorKey(defaultVariant) : allColors[0]?.[0]);
    const selectedSize = manualSize ?? defaultVariant?.sizeName ?? allSizes[0];

    // розміри, доступні для ОБРАНОГО кольору
    const sizesForSelectedColor = useMemo(
        () =>
            new Set(
                variants.filter((v) => colorKey(v) === selectedColor).map((v) => v.sizeName)
            ),
        [variants, selectedColor, isUk]
    );

    // кольори, доступні для ОБРАНОГО розміру
    const colorsForSelectedSize = useMemo(
        () =>
            new Set(
                variants.filter((v) => v.sizeName === selectedSize).map((v) => colorKey(v))
            ),
        [variants, selectedSize, isUk]
    );

    const activeVariant = useMemo(
        () =>
            variants.find(
                (v) => colorKey(v) === selectedColor && v.sizeName === selectedSize
            ),
        [variants, selectedColor, selectedSize, isUk]
    );

    useEffect(() => {
        if (!activeVariant) return;

        const segments = pathname.split("/");
        const currentLast = segments[segments.length - 1];

        if (currentLast === activeVariant.id) return;

        segments[segments.length - 1] = activeVariant.id;
        router.replace(segments.join("/"), { scroll: false });
    }, [activeVariant, pathname, router]);

    const handleColorClick = (color: string) => {
        setManualColor(color);
        const availableSizes = new Set(
            variants.filter((v) => colorKey(v) === color).map((v) => v.sizeName)
        );
        if (!selectedSize || !availableSizes.has(selectedSize)) {
            const fallback = allSizes.find((s) => availableSizes.has(s));
            setManualSize(fallback);
        }
    };

    const handleSizeClick = (size: string) => {
        setManualSize(size);
        const availableColors = new Set(
            variants.filter((v) => v.sizeName === size).map((v) => colorKey(v))
        );
        if (!selectedColor || !availableColors.has(selectedColor)) {
            const fallback = allColors.find(([c]) => availableColors.has(c))?.[0];
            setManualColor(fallback);
        }
    };

    const finalPrice = activeVariant
        ? activeVariant.price - ((activeVariant.price * (activeVariant.sale/100)))
        : undefined;
    const hasSale = !!activeVariant?.sale;

    // const handleAddToCart = async () => {
    //     const {data} = cartApi.useHasCartQuery();
    //     if(data) {
    //         const {data: cart} = cartApi.useGetCartByUserQuery();
    //         const value : IAddCartItemModel = {
    //             cartId: cart.id,
    //             p
    //         }
    //     } else{
    //         await addCart().unwrap()
    //     }
    // }

    return (
        <>
        <div
            className="absolute bottom-1/5 right-1/8 z-999 w-[380px]
        bg-[var(--card)] rounded-xl p-6 flex flex-col gap-4 shadow-lg"
        >
            {/* Назва + ціна */}
            <div className="flex items-start justify-between gap-4">
                <h2 className="font-bold uppercase tracking-tight text-lg leading-snug">
                    {isUk ? data?.nameUk : data?.name}
                </h2>
                <div className="flex flex-col items-end shrink-0">
                    {hasSale && (
                        <span className="text-sm line-through text-[var(--muted-foreground)]">
              {activeVariant?.price} грн
            </span>
                    )}
                    <span className="font-bold text-lg whitespace-nowrap">
            {finalPrice} грн
          </span>
                </div>
            </div>

            {/* Опис */}
            <p className="text-sm text-[var(--muted-foreground)] line-clamp-2">
                {isUk ? data?.descriptionUk : data?.description}
            </p>

            {/* Кольори */}
            {allColors.length > 0 && (
                <div className="flex flex-col gap-2">
                    <div className="flex gap-2">
                        {allColors.map(([label, variant]) => {
                            const unavailable = !colorsForSelectedSize.has(label);
                            return (
                                <button
                                    key={label}
                                    type="button"
                                    onClick={() => handleColorClick(label)}
                                    className={`w-10 h-10 rounded border-2 overflow-hidden relative
        ${selectedColor === label ? "border-blue-500" : "border-transparent"}
        ${unavailable ? "opacity-30" : ""}`}
                                    aria-label={label}
                                >
                                    {variant.images?.[0] ? (
                                        // eslint-disable-next-line @next/next/no-img-element
                                        <img src={variant.images[0]} alt={label} className="w-full h-full object-cover" />
                                    ) : (
                                        <div className="w-full h-full bg-[var(--muted)]" />
                                    )}
                                </button>
                            );
                        })}
                    </div>
                    <span className="text-xs uppercase tracking-wide text-[var(--muted-foreground)]">
            {selectedColor}
          </span>
                </div>
            )}

            <div className="h-px bg-[var(--border)]" />


            {allSizes.length > 0 && (
                <div className="flex flex-col gap-2">
          <span className="font-semibold uppercase text-sm">
              Розміри
          </span>
                    <div className="flex gap-3">
                        {allSizes.map((size) => {
                            const unavailable = !sizesForSelectedColor.has(size);
                            return (
                                <button
                                    key={size}
                                    type="button"
                                    onClick={() => handleSizeClick(size)}
                                    className={`font-semibold text-sm ${
                                        selectedSize === size ? "underline underline-offset-4" : "text-[var(--muted-foreground)]"
                                    } ${unavailable ? "opacity-30" : ""}`}
                                >
                                    {size}
                                </button>
                            );
                        })}
                    </div>
                </div>
            )}

            {/* Кнопка */}
            <button
                type="button"
                disabled={!activeVariant}
                className="mt-2 bg-[var(--foreground)] text-[var(--background)]
          font-bold uppercase tracking-wide py-3 rounded-md
          disabled:opacity-40 transition-opacity hover:cursor-pointer"
                onClick={() => openModal()}
            >
                Додати у кошик
            </button>
        </div>
            <Modal isOpen={isOpen} closeModal={closeModal} title={"Cart"} size='sm'  >

                <AddToCartForm productVariantId={activeVariant!.id} price={finalPrice!} closeModal={closeModal} />
            </Modal>
        </>
    );
};

export default ProductInfo;