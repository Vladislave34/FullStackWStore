import Section from "@/app/[lng]/components/Section";
import FilterBar from "@/app/[lng]/components/FilterBar";
import {Metadata} from "next";
import {getT} from "next-i18next/server";

type PageProps = { params: Promise<{ lng: string }> }

export async function generateMetadata({ params }: PageProps): Promise<Metadata> {
    const { lng } = await params
    const { t } = await getT('pages', { lng })
    return { title: t('all') }
}

const Page = async ({params}: {params: Promise<{lng:string}>}) => {
    const {lng} = await params;
    return (
        <div className="mt-22">
            <FilterBar lng={lng} />
            <Section lng={lng} hasHeader={false} />
        </div>
    );
};

export default Page;