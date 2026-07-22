export default interface IPageResult<T> {
    data: T[];
    totalCount: number;
    currentPage: number;
    pageSize: number;
    totalPages: number;
}