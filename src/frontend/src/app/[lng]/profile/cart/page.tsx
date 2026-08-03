
import CartItemList from "@/app/[lng]/components/CartItemList";
import OrderButton from "@/app/[lng]/UI/OrderButton";
import {getT} from "next-i18next/server";
import {Metadata} from "next";
type PageProps = { params: Promise<{ lng: string }> }

export async function generateMetadata({ params }: PageProps): Promise<Metadata> {
    const { lng } = await params
    const { t } = await getT('pages', { lng })
    return { title: t('cart') }
}

const Page = async ({params}: {params: Promise<{lng:string}>}) => {
    const {lng} = await params;
    const {t} = await getT("profile", {lng});
    return (
        <div className="h-screen flex flex-col p-8 overflow-hidden">
            <div className="flex flex-row gap-2 py-4 mb-4 justify-between items-center shrink-0">
                <span className='text-[var(--text)] text-3xl font-semibold'>{t('cart')}</span>
                <OrderButton />
            </div>
            <CartItemList lng={lng} />
        </div>
    );
};

export default Page;