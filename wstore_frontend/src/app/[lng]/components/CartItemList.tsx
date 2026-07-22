'use client'
import {cartItemApi} from "@/services/cartItemService";
import CartItemRow from "@/app/[lng]/UI/CartItemRow";


const CartItemList = ({lng}: {lng: string}) => {
    const {data} = cartItemApi.useGetCartItemsByUserQuery();
    return (
        <div className="flex flex-col justify-start items-center gap-2 w-full flex-1 min-h-0 overflow-y-auto">
            {data?.map((item) => (
                <CartItemRow cartItem={item} key={item.id} lng={lng} />
            ))}
        </div>
    );
};

export default CartItemList;