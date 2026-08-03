import Hero from "@/app/[lng]/components/Hero";
import Section from "@/app/[lng]/components/Section";
import {getT} from "next-i18next/server";
import {Metadata} from "next";
import ProductsGrid from "@/app/[lng]/components/ProductsGrid";

// type PageProps = {
//     params: Promise<{
//         lng: string;
//     }>;
// };

type PageProps = { params: Promise<{ lng: string }> }

export async function generateMetadata({ params }: PageProps): Promise<Metadata> {
    const { lng } = await params
    const { t } = await getT('pages', { lng })
    return { title: t('home') }
}

export default async function HomePage({ params }: PageProps) {
    const { lng } = await params;
    // const { t } = await getT('nav', { lng });
    return (
        <div className='flex items-center justify-center flex flex-col'>
            {/*{t('sale')}*/}
            <Hero lng={lng} />
            <Section lng={lng} hasHeader={true} />

        </div>
    );
}
