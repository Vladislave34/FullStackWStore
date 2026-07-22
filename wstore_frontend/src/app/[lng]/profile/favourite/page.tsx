import FavouriteList from "@/app/[lng]/components/FavouriteList";
import Icon from "@/app/[lng]/UI/Icon";

import React from "react";


const Favourite = async ({params} : {params : Promise<{lng:string}>}) => {
    const {lng} = await params;
    return (
        <div className="p-8 min-h-screen">
            <div className="flex flex-row gap-2 py-4 mb-4 justify-between items-center ">
                <span className='text-[var(--text)] text-3xl font-semibold'>Favourites</span>
            </div>
            <div className="flex flex-col gap-3 w-full ">
                <FavouriteList lng={lng} />
            </div>
        </div>

    );
};

export default Favourite;