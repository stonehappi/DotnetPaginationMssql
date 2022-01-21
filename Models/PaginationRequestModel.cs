namespace DotnetPaginationMssql.Models;

public class PaginationRequestModel
{
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public string? Direction { get; set; }
    public int PageSize { get; set; } = 10;
    private int _currentPage = 1;

    public int CurrentPage
    {
        get => _currentPage;
        set => _currentPage = value < 1 ? 1 : value;
    }
}
