namespace DotnetPaginationMssql.Models;

public class PaginationResponseModel
{
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}
