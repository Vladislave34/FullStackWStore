import Section from "@/app/[lng]/components/Section";
import FilterBar from "@/app/[lng]/components/FilterBar";


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