namespace Core.Models.Search;

public class SearchResult<T>
{
    public List<T> Items { get; set; }
    public PaginationModel Pagination { get; set; }
}