using System.Text.Json;
using DotnetPaginationMssql;
using DotnetPaginationMssql.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(@"DataSource=database.db;");
    options.UseLoggerFactory(
    LoggerFactory.Create(loggingBuilder => { loggingBuilder.AddConsole(); }));
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

if (true)
{
    using var serviceScope = app.Services.CreateScope();
    var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
    if (context == null) return;
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    using var r = new StreamReader("data.json");
    var json = r.ReadToEnd();
    var items = JsonSerializer.Deserialize<List<User>>(json);
    if (items != null) context.Users.AddRange(items);
    context.SaveChanges();
}
app.Run();
