"use client";

import {useEffect, useState} from "react";
import SearchBar from "@/app/[lng]/UI/SearchBar";
import CustomSelect from "@/app/[lng]/UI/SelectButton";
import Modal from "@/app/[lng]/UI/Modal";
import CreateProductForm from "@/app/[lng]/UI/forms/CreateProductForm";
import {categoryApi} from "@/services/categoryService";

import {useAppDispatch} from "@/hooks/redux";
import {setCategoryForSearch} from "@/store/reducers/searchSlice";
import {useT} from "next-i18next/client";



export default function ProductsToolbar({lng}: { lng: string }) {
    const dispatch = useAppDispatch();
    const [category, setCategory] = useState<string | null>(null);
    const [isAddModalOpen, setIsAddModalOpen] = useState(false);
    const {data} = categoryApi.useFetchAllCategoriesQuery(lng);
    const {t} = useT('store_products');
    const categoties =  data?.map(category => category.name) ?? [""]
    useEffect(() => {
        dispatch(setCategoryForSearch(data?.find(x=> x.name === category)));
    }, [category]);
    return (
        <>
            <div className="flex flex-row items-center gap-3">
                <SearchBar isStoreOwner={true} />

                <div className="w-48">
                    <CustomSelect
                        label={t('select')}
                        name="category"
                        options={categoties}
                        value={category}
                        onChange={setCategory}
                    />
                </div>

                <button
                    type="button"
                    onClick={() => setIsAddModalOpen(true)}
                    className="
                        h-[42px] px-4
                        bg-[var(--btn)] text-white text-sm font-semibold
                        rounded-lg hover:opacity-90 transition-opacity
                        flex items-center gap-2 whitespace-nowrap
                    "
                >
                    + {t('add_product')}
                </button>
            </div>

            {isAddModalOpen && (
                <Modal isOpen={isAddModalOpen} closeModal={() => setIsAddModalOpen(false)} >
                    <CreateProductForm lng={lng} closeModal={() => setIsAddModalOpen(false)}  />
                </Modal>
            )}
        </>
    );
}