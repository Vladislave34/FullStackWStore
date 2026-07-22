export default interface IAddOrderModel {
    // cartId: string | undefined;
    cartItemIds: string[];
    addressId: string;
    paymentId: string;
}