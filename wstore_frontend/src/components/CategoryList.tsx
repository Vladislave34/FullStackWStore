'use client'

import {categoryApi} from "@/services/categoryService";

import Label from "@/UI/Label";
import {FC} from "react";
import {getLocale} from "@/utils/getLocale";

type CategoryListProps = {
    all: string;
}

const CategoryList : FC<CategoryListProps> = ({all}) => {
    const locale = getLocale();
    const { data } = categoryApi.useFetchAllCategoriesQuery(locale!);
    console.log(locale);

    console.log(data);
    return (
        <div className="flex flex-row gap-4 p-4">
            <Label>{all}</Label>
            {data?.map((category) =>

                    <Label key={category.id}>
                        {locale === "en" ? category?.name : category?.name}
                    </Label>

            )}

        </div>
    );
};

export default CategoryList;