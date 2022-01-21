using System.Linq.Expressions;
using DotnetPaginationMssql.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetPaginationMssql.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "Get User")]
    public IActionResult Get([FromQuery] PaginationRequestModel page)
    {
        var query = _context.Set<User>() as IQueryable<User>;
        if (!string.IsNullOrEmpty(page.Search))
        {
            query = query.Where(
            e => e.FirstName.Contains(page.Search) || e.LastName.Contains(page.Search));
        }
        var total = query.Count();
        var totalPage = (int) Math.Ceiling(total / (double) page.PageSize);
        if (page.CurrentPage > totalPage)
        {
            page.CurrentPage = totalPage;
        }
        var skip = (page.CurrentPage - 1) * page.PageSize;
        query = query.Skip(skip).Take(page.PageSize);
        if (!string.IsNullOrEmpty(page.Sort))
        {
            Expression<Func<User, string>> keySelector = page.Sort switch
            {
                "lastName" => user => user.LastName,
                "email" => user => user.Email,
                "gender" => user => user.Gender,
                "ipAddress" => user => user.IpAddress,
                _ => user => user.FirstName
            };
            query = page.Direction == "desc"
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }
        var pagination = new PaginationResponseModel
        {
            CurrentPage = page.CurrentPage,
            TotalItems = total,
            TotalPages = (int) Math.Ceiling(total / (double) page.PageSize),
            PageSize = page.PageSize
        };
        return Success(query, pagination);
    }

    [NonAction]
    private IActionResult Success(dynamic data, PaginationResponseModel? pagination) =>
        Ok(new {Status = "S", pagination, data});
}
