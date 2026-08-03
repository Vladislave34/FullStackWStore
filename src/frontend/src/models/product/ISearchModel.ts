export default interface ISearchModel {
    categoryId: string | null;
    genderId: string | null;
    colorId: string | null;
    sizeId: string | null;
    query: string | null;
    hasSale: boolean | null;
    pageNumber: number;
    pageSize: number;
    locale: string;
}