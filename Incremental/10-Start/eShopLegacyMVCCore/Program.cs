using eShopLegacyMVC.Models;
using eShopLegacyMVC.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSystemWebAdapters();
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<CatalogDBContext>();
builder.Services.AddSingleton<CatalogItemHiLoGenerator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSystemWebAdapters();

app.MapControllerRoute("Default", "{controller=Catalog}/{action=Index}/{id?}");
app.MapReverseProxy();

app.Run();
