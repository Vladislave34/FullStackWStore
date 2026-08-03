import IncomeAreaChart from "@/app/[lng]/UI/IncomeAreaChart";
import CategoryIncomeChart from "@/app/[lng]/UI/CategoryIncomeChart";


const Page = async ({params} : {params: Promise<{lng: string}>}) => {
    const {lng} = await params;
    return (
        <div className="p-8 flex flex-col gap-4 justify-center  min-h-screen overflow-y-auto ">
            <IncomeAreaChart />
            <CategoryIncomeChart lng={lng} />
        </div>
    );
};

export default Page;
