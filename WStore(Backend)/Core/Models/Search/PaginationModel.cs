namespace Core.Models.Search;

public class PaginationModel
{
    public int TotalCount { get; set; }
    public int TotalPage { get; set; }
    public int CurrentPage { get; set; }
    public int ItemsPerPage { get; set; }
}