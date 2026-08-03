'use client'
import CustomSelect from "@/app/[lng]/UI/SelectButton";
import {categoryApi} from "@/services/categoryService";
import React, {useEffect, useMemo, useState} from "react";
import {useSetParams} from "../../../../util/setParams";
import {genderApi} from "@/services/genderService";
import {colorApi} from "@/services/colorService";
import {sizeApi} from "@/services/sizeService";
import Icon from "@/app/[lng]/UI/Icon";
import {FaFilter} from "react-icons/fa";
import CheckBox from "@/app/[lng]/UI/CheckBox";
import CustomCheckbox from "@/app/[lng]/UI/CustomCheckbox";
import {useT} from "next-i18next/client";


const FilterBar = ({lng}: {lng:string}) => {
    const {t} = useT('filter');
    const [isOpen, setIsOpen] = useState(false);
    const setParams = useSetParams();
    const [category, setCategory] = useState<string | null>(null);
    const [gender, setGender] = useState<string | null>(null);
    const [color, setColor] = useState<string | null>(null);
    const [size, setSize] = useState<string | null>(null);
    const [hasSale, setHasSale] = useState<boolean>(false);
    const {data : cats} = categoryApi.useFetchAllCategoriesQuery(lng);
    const {data : genders} =genderApi.useGetAllGendersQuery(lng);
    const {data : colors} = colorApi.useGetColorsQuery(lng);
    const {data : sizes} = sizeApi.useGetSizesQuery();


    const categories =  cats?.map(category => category.name) ?? [""]
    const genders_ =  genders?.map(gender => gender.name) ?? [""]
    const colors_ =  colors?.map(color => color.name) ?? [""]
    const sizes_ =  sizes?.map(size => size.name) ?? [""]

    useEffect(() => {
        if (!cats) return;

       if(!category) setParams({
           categoryId:  null,
       });


        const selectedCategory = cats.find(x => x.name === category);

        setParams({
            categoryId: selectedCategory?.id ?? null,
        });
    }, [category, cats]);
    useEffect(() => {
        if (!genders) return;

        if(!category) setParams({
            genderId:  null,
        });


        const selectedGender = genders.find(x => x.name === gender);

        setParams({
            genderId: selectedGender?.id ?? null,
        });
    }, [gender, genders]);
    useEffect(() => {
        if (!colors) return;

        if(!color) setParams({
            colorId:  null,
        });


        const selectedColor = colors.find(x => x.name === color);

        setParams({
            colorId: selectedColor?.id ?? null,
        });
    }, [color, colors]);
    useEffect(() => {
        if (!sizes) return;

        if(!size) setParams({
            sizeId:  null,
        });


        const selectedSize = sizes.find(x => x.name === size);

        setParams({
            sizeId: selectedSize?.id ?? null,
        });
    }, [size, sizes]);
    useEffect(() => {
        setParams({
            hasSale: hasSale ? "true" : null,
        });
    }, [hasSale]);


    return (
        // <div className="w-full p-4 flex flex-row gap-2 justify-end ">
        //     <div className="w-48">
        //         <CustomSelect
        //             label={"Category"}
        //             name="category"
        //             options={categories}
        //             value={category}
        //             onChange={setCategory}
        //         />
        //     </div>
        //     <div className="w-48">
        //         <CustomSelect
        //             label={"Gender"}
        //             name="gender"
        //             options={genders_}
        //             value={gender}
        //             onChange={setGender}
        //         />
        //     </div>
        //     <div className="w-48">
        //         <CustomSelect
        //             label={"Color"}
        //             name="color"
        //             options={colors_}
        //             value={color}
        //             onChange={setColor}
        //         />
        //     </div>
        //     <div className="w-48">
        //         <CustomSelect
        //             label={"Size"}
        //             name="size"
        //             options={sizes_}
        //             value={size}
        //             onChange={setSize}
        //         />
        //     </div>
        // </div>
        <div className="relative w-full flex items-center justify-between p-4 min-h-[56px]">
            <span className='text-[var(--text)] text-3xl font-semibold'>{t('products')}</span>
            <div>
            <div
                className={`absolute right-16 flex flex-row gap-2 transition-all duration-300 ease-in-out flex flex-row items-center
          ${isOpen
                    ? "translate-x-0 opacity-100 pointer-events-auto"
                    : "translate-x-8 opacity-0 pointer-events-none"}`}
            >
                <div className="w-48">
                    <CustomSelect label={t('category')} name="category" options={categories} value={category} onChange={setCategory} />
                </div>
                <div className="w-48">
                    <CustomSelect label={t('gender')} name="gender" options={genders_} value={gender} onChange={setGender} />
                </div>
                <div className="w-48">
                    <CustomSelect label={t('color')} name="color" options={colors_} value={color} onChange={setColor} />
                </div>
                <div className="w-48">
                    <CustomSelect label={t('size')} name="size" options={sizes_} value={size} onChange={setSize} />
                </div>
                <div>
                    <CustomCheckbox
                        id="sale-filter"
                        name="bordered-checkbox"
                        value="true"
                        label={t('sale')}
                        checked={hasSale}
                        onChange={setHasSale}
                    />                </div>
            </div>

                <button
                    onClick={() => setIsOpen((p) => !p)}
                    className="z-10 rounded-full p-1 hover:bg-black/5 transition-colors"
                >
                    <Icon width={36} height={36} isOpen={isOpen}>
                        <FaFilter/>
                    </Icon>
                </button>
            </div>
        </div>
    );
};

export default FilterBar;