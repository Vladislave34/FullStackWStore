import SecHead from "@/app/[lng]/components/SecHead";
import ProductsGrid from "@/app/[lng]/components/ProductsGrid";

import {getT} from "next-i18next/server";




const Section = async ({lng, hasHeader}: {lng:string, hasHeader: boolean}) => {
    // const t = useTranslations("products")
    const { t } = await getT('sec_head', {lng})
    return (
        <div className="w-full p-4">
            {hasHeader &&
            <SecHead
            // title={t("title")}
            // seeAll={t("seeAll")}
            title={t('title')}
            seeAll={t('subtitle')}
            lng={lng}
            />
            }

            <div className="mt-2"><ProductsGrid lng={lng} /></div>

        </div>
    );
};

export default Section;