export default interface IAddProductVarinat {
    productId: string;
    colorId: string;
    sizeId: string;
    price: number;
    saleId?: string | null;
    images:  File[] | null;
}