import ICartitemDetailModel from "@/models/cartitem/ICartitemDetailModel";
import ScaleImage from "@/app/[lng]/UI/ScaleImage";
import CheckBox from "@/app/[lng]/UI/CheckBox";


const CartItemRow = ({cartItem, lng}: {cartItem : ICartitemDetailModel, lng:string}) => {
    const product  = cartItem.product;
    const productVariant = cartItem.productVariant;
    return (
        <div className="flex flex-row gap-2 bg-[var(--surface)] rounded-xl overflow-hidden border-[var(--border)] border-2 p-6 justify-between items-center w-full flex-shrink-0">
            <div className="flex flex-row gap-4 items-center text-lg">
                <span>{product.name}</span>
                <span>{lng === "en" ? productVariant.color : productVariant.colorUk}</span>
                <span>{productVariant.size}</span>
                <span>{cartItem.price}грн</span>
                <span>{cartItem.quantity}</span>
            </div>
            <div className="flex flex-row gap-4 items-center">

                {productVariant?.images.map((image) => (
                    <ScaleImage key={image} image={image}/>
                ))}
                <CheckBox id={cartItem.id} />
            </div>
        </div>
    );
};

export default CartItemRow;