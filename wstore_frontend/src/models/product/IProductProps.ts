export default interface IProductProps {
    storeId?: string;
    pageNumber: number;
    pageSize: number;
    locale: string;
    categoryId?: string;
    query?: string;
}