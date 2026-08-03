import IProductVariantModel from "@/models/product/variant/IProductVariantModel";


export default interface IProductModel {
    id: string;
    name: string;
    nameUk: string;
    description: string;
    descriptionUk: string;
    category: string;
    store: string;
    gender: string;
    genderUk: string;
    isFavourite?: boolean;
    variants?: IProductVariantModel[];
}

