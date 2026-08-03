import ICartItemModel from "@/models/cartitem/ICartItemModel";

export default interface ICartModel {
    id: string;
    cartItems: ICartItemModel[]
}