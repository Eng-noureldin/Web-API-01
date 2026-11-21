using Microsoft.EntityFrameworkCore;
using Session01_WebAPI.Data;
using Session01_WebAPI.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register DbContext with In-Memory Database for instant execution
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("WebAPISession01Db"));

var app = builder.Build();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!context.Categories.Any())
    {
        var cat1 = new Category { Name = "Smartphones" };
        var cat2 = new Category { Name = "Accessories" };
        context.Categories.AddRange(cat1, cat2);
        context.SaveChanges();

        context.Products.AddRange(
            new Product { Name = "iPhone 15 Pro", Price = 999.99m, Description = "Titanium body with A17 chip", CategoryId = cat1.Id },
            new Product { Name = "AirPods Pro 2", Price = 249.99m, Description = "Active noise cancelling earbuds", CategoryId = cat2.Id }
        );
        context.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
