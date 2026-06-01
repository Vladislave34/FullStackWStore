import SecHead from "@/components/SecHead";
import ProductList from "@/components/ProductList";
import {useTranslations} from "next-intl";


const Section = () => {
    const t = useTranslations("products")
    return (
        <div className="w-full p-4">
            <SecHead
                title={t("title")}
                seeAll={t("seeAll")}
            />
            <ProductList />
        </div>
    );
};

export default Section;