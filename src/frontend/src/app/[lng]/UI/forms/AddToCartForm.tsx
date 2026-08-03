import * as Yup from "yup";
import IAddCartItemModel from "@/models/cartitem/IAddCartItemModel";
import {cartApi} from "@/services/cartService";
import UniversalForm from "@/app/[lng]/UI/forms/abstract/UniversalForm";
import FormikNumberInput from "@/app/[lng]/UI/forms/inputs/FormikNumberInput";
import {cartItemApi} from "@/services/cartItemService";

const schema = Yup.object({
    quantity: Yup.number().min(1, "Мінімум 1").required(),
})

// const AddToCartForm = ({productVariantId, price, closeModal} : {productVariantId : string, price : number, closeModal: ()=>void}) => {
//     const [addCart] = cartApi.useAddCartMutation();
//     const [addToCart] = cartItemApi.useAddToCartMutation();
//     const {data} = cartApi.useHasCartQuery();
//
//
//     const initialValues : IAddCartItemModel = {
//         cartId: "",
//         productVariantId: productVariantId,
//         quantity: 1,
//         price: price,
//     }
//     const handleSubmit =
//         async (values: IAddCartItemModel) => {
//             const val : IAddCartItemModel = {
//                 cartId: values.cartId ?? '',
//                 productVariantId: values.productVariantId,
//                 quantity: values.quantity,
//                 price: price * values.quantity,
//             }
//             console.log(data, val);
//             if(data) {
//                 const {data:cart} = cartApi.useGetCartByUserQuery();
//                 val.cartId = cart!.id;
//                 console.log(cart);
//                 await addToCart(val).unwrap();
//             } else{
//                 console.log("dfbdfbdfbd");
//                 const id = await addCart().unwrap();
//                 val.cartId = id;
//                 await addToCart(val).unwrap();
//             }
//             closeModal();
//     }
//     return (
//         <UniversalForm
//             initialValues={initialValues}
//             onSubmit={handleSubmit}
//             validationSchema={schema}
//             title="Cart"
//             subtitle={"Add To Cart"}
//             submitLabel={"Add To Cart"}
//         >
//             <FormikNumberInput name="quantity" label="Quantity" />
//         </UniversalForm>
//     );
// };
//

const AddToCartForm = ({productVariantId, price, closeModal} : {productVariantId : string, price : number, closeModal: ()=>void}) => {
    // const [addCart] = cartApi.useAddCartMutation();
    const [addToCart] = cartItemApi.useAddToCartMutation();
    const {data: hasCart} = cartApi.useHasCartQuery();
    const {data: cart} = cartApi.useGetCartByUserQuery(undefined, { skip: !hasCart }); // <-- завжди викликаний, але пропускається запит, якщо кошика немає

    const initialValues: IAddCartItemModel = {
        cartId: "",
        productVariantId: productVariantId,
        quantity: 1,
        price: price,
    }

    const handleSubmit = async (values: IAddCartItemModel) => {
        const val: IAddCartItemModel = {
            cartId: values.cartId ?? '',
            productVariantId: values.productVariantId,
            quantity: values.quantity,
            price: price * values.quantity,
        }

        if (hasCart && cart) {
            val.cartId = cart.id;
            await addToCart(val).unwrap();
        }
        // else {
        //     const id = await addCart().unwrap();
        //     val.cartId = id;
        //     await addToCart(val).unwrap();
        // }
        closeModal();
    }

    return (
        <UniversalForm
            initialValues={initialValues}
            onSubmit={handleSubmit}
            validationSchema={schema}
            title="Cart"
            subtitle={"Add To Cart"}
            submitLabel={"Add To Cart"}
        >
            <FormikNumberInput name="quantity" label="Quantity" />
        </UniversalForm>
    );
};
export default AddToCartForm;