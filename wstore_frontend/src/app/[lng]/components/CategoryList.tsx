'use client'

import {categoryApi} from "@/services/categoryService";

import Label from "@/app/[lng]/UI/Label";
import {FC} from "react";
import {usePathname, useRouter} from "next/navigation";
import {useSetParams} from "../../../../util/setParams";


type CategoryListProps = {
    all: string;
}

const CategoryList : FC<CategoryListProps> = ({all}) => {
    const pathname = usePathname();
    const locale = pathname.split('/')[1];
    const { data } = categoryApi.useFetchAllCategoriesQuery(locale!);
    const router = useRouter();
    const handleClick = (category: string | undefined) => {
        const selectedCategory = data?.find((x) => x.name === category);

        const params = new URLSearchParams();
        params.set("page", "1");
        if (selectedCategory?.id) {
            params.set("categoryId", String(selectedCategory.id));
        }

        router.push(`/${locale}/products/all?${params.toString()}`);
    };

    const handleAllClick = () => {
        router.push(`/${locale}/products/all?page=1`);
    };


    console.log(data);
    return (
        <div className="flex flex-row gap-4 p-4">
            <Label handleClick={handleAllClick}>{all}</Label>
            {data?.map((category) =>

                    <Label key={category.id} handleClick={handleClick} category={category.name} >
                        {locale === "en" ? category?.name : category?.name}
                    </Label>

            )}

        </div>
    );
};

export default CategoryList;