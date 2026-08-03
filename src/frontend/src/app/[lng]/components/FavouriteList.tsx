'use client'
import {favouriteApi} from "@/services/favouriteService";
import FavouritesRow from "@/app/[lng]/UI/FavouritesRow";


const FavouriteList = ({lng}: {lng:string}) => {
    const {data, isLoading} = favouriteApi.useGetFavouritesQuery(lng);
    console.log(data)
    return (
            <div className="flex flex-col gap-2 w-full max-h-[calc(100vh-8rem)] overflow-y-auto pr-2">
                {data?.map(product =>
                    <FavouritesRow product={product} lng={lng} key={product.id} />)
                }
            </div>
    );
};

export default FavouriteList;