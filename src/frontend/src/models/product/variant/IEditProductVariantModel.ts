export default interface IEditProductVariantModel {
    id: string;
    productId: string;
    colorId: string;
    sizeId: string;
    price: number;
    saleId?: string | null;
    images: File[];
}