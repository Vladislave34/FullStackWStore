


import ProductList from "@/app/[lng]/components/ProductList";
import {storeApi} from "@/services/storeService";
import SearchBar from "@/app/[lng]/UI/SearchBar";
import SelectButton from "@/app/[lng]/UI/SelectButton";
import CustomSelect from "@/app/[lng]/UI/SelectButton";
import ProductsToolbar from "@/app/[lng]/components/ProductToolBar";
import {getT} from "next-i18next/server";
import CountBadge from "@/app/[lng]/UI/CountBadge";
interface PageProps {
    params: Promise<{ lng: string }>;
}

const Page = async ({ params }: PageProps) => {
    const { lng } = await params;
    const {t} = await getT('store_products', {lng});

    console.log(lng);
    return (
        <div className="p-8 flex flex-col gap-6 min-h-screen ">
            <div className="flex flex-row justify-between items-center">
                <div className="flex flex-col gap-1">
                    <p className="text-[var(--text)] text-3xl font-semibold">{t('title')}</p>
                    <p className="text-[var(--muted)] text-base"><CountBadge /> {t('subtitle')}</p>
                </div>
                <ProductsToolbar lng={lng} />
            </div>

            <ProductList locale={lng} />
        </div>
    );
};

export default Page;