using eShopLegacyMVC.Models;
using eShopLegacyMVC.Models.Infrastructure;
using eShopLegacyMVC.Services;
using log4net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Data.Entity;
using System.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.OutputFormatters.Insert(0, new XmlSerializerOutputFormatter());
});

// Enable caching for session state
builder.Services.AddDistributedMemoryCache();

// Enable WebOptimizer for bundling and minification
builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.AddJavaScriptBundle("/bundles/jquery", "Scripts/jquery-3.3.1.js").UseContentRoot();
    pipeline.AddJavaScriptBundle("/bundles/jqueryval", "Scripts/jquery.validate.*").UseContentRoot();
    pipeline.AddJavaScriptBundle("/bundles/modernizr", "Scripts/modernizr-*").UseContentRoot();
    pipeline.AddJavaScriptBundle("/bundles/bootstrap", 
        "Scripts/bootstrap.js",
        "Scripts/respond.js").UseContentRoot();

    pipeline.AddCssBundle("/Content/css",
        "Content/bootstrap.css",
        "Content/custom.css",
        "Content/base.css",
        "Content/site.css").UseContentRoot();
});

var mockData = bool.Parse(builder.Configuration["UseMockData"]);
if (mockData)
{
    builder.Services.AddScoped<ICatalogService, CatalogServiceMock>();
}
else
{
    builder.Services.AddScoped<ICatalogService, CatalogService>();
}

builder.Services.AddScoped<CatalogDBContext>();
builder.Services.AddScoped<CatalogDBInitializer>();
builder.Services.AddSingleton<CatalogItemHiLoGenerator>();

// Enable session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

ConfigDatabase(mockData, app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Application_BeginRequest
app.Use(async (context, next) =>
{
    //set the property to our new object
    LogicalThreadContext.Properties["activityid"] = new ActivityIdHelper();

    LogicalThreadContext.Properties["requestinfo"] = new WebRequestInfo(context);

    log.Debug("WebApplication_BeginRequest");

    await next();
});

// Use WebOptimizer in place of previous bundling/minification
// Note that it must come before UseStaticFiles
app.UseWebOptimizer();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

// Session_Start
app.Use(async (context, next) =>
{
    if (context.Session.IsAvailable && !context.Session.Keys.Contains("SessionStartTime"))
    {
        context.Session.SetString("SessionStartTime", DateTime.Now.ToString());
    }

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}/{id?}");

app.Run();

static void ConfigDatabase(bool mockData, IServiceProvider services)
{
    if (!mockData)
    {
        using var scope = services.CreateScope();
        Database.SetInitializer<CatalogDBContext>(scope.ServiceProvider.GetRequiredService<CatalogDBInitializer>());
    }
}

class ActivityIdHelper
{
    public override string ToString()
    {
        if (Trace.CorrelationManager.ActivityId == Guid.Empty)
        {
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
        }

        return Trace.CorrelationManager.ActivityId.ToString();
    }
}

class WebRequestInfo
{
    private readonly HttpContext context;

    public WebRequestInfo(HttpContext context)
    {
        this.context = context;
    }

    public override string ToString()
    {
        return context.Request?.GetDisplayUrl() + ", " + context?.Request?.Headers["User-Agent"].ToString();
    }
}