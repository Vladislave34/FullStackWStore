export default interface ICartitemDetailModel {
    id: string;
    cartId: string;
    productVariantId: string;
    quantity: number;
    price: number;
    product: {
        id: string;
        name: string;
        nameUk: string;
        description: string;
        descriptionUk: string;
    },
    productVariant: {
        id: string;
        color: string;
        colorUk: string;
        size: string;
        price: number;
        sale: number;
        images: string[];
    }
}