import IProductModel from "@/models/product/IProductModel";
import ScaleImage from "@/app/[lng]/UI/ScaleImage";


const FavouritesRow = ({product, lng}: {product: IProductModel, lng:string}) => {
    if (!product.variants || product.variants.length === 0) return null;
    const mainVariant = product?.variants[0];
    return (
        <div className="flex flex-row gap-2 bg-[var(--surface)]
         rounded-xl overflow-hidden border-[var(--border)]
          border-2 p-6 justify-between items-center w-full
          flex-shrink-0
          ">
            <div className="flex flex-row gap-4 items-center text-lg">
                <span>{product.name}</span>
                <span>{product.category}</span>
                <span>{lng === "en" ? product.gender : product.genderUk}</span>

            </div>
            <div className="flex flex-row gap-4 items-center">

                {mainVariant?.images.map((image, i) => (
                    <ScaleImage key={i} image={image} />
                ))}

            </div>
        </div>
    );
};

export default FavouritesRow;