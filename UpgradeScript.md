# Upgrade Steps

## Test the ASP.NET app

1. Build and run the ASP.NET app to show its initial state. Try out CRUD operations with store items.

## Analyze and prepare (.NET Portability Analyzer)

1. Download the zip of the latest version of API Port: https://github.com/microsoft/dotnet-apiport/releases
    1. Don't worry about the 'alpha' naming
1. Extract the archive locally
1. Run API Port on the binary output of the web app
    1. `ApiPort.exe analyze -t .NET -t ".NET Standard" -r html -r excel -f D:\source\MJRousos\UpgradeSample\src\eShopLegacyMVC\bin`
1. Review the portability reports
    1. Notice that log4net is not 100% compatible, but that's ok because we can search NuGet.org and find a compatible version of the library

## Analyze and prepare (Upgrade Assistant analysis)

1. Install upgrade assistant
    1. `dotnet tool install -g upgrade-assistant`
1. Run upgrade-assistant's analyze command on the solution
    1. `upgrade-assistant analyze eShopLegacyMVC.sln`
1. Install a sarif viewer extension in VS Code or VS and open the sarif file to review it

## Upgrade project file format and dependencies (Upgrade Assistant upgrade)

1. We're ready to upgrade now. This could be done manually, but Upgrade Assistant can save us some time.
1. Show command line options: `upgrade-assistant upgrade --help`
1. `upgrade-assistant upgrade eShopLegacyMVC.sln`
    1. Mention the --non-interactive option and the reason it's not enabled by default
    1. While upgrading, have the solution open in VS Code (not VS) to observe the changes
    1. Note warnings which indicate items the developer will need to address manually (both in the package update and after source code updates)

## Additional project updates

1. At this point, the project file is nearly up-to-date there's just a small amount of clean-up still needed
1. Re-open the solution in VS
1. Notice that eShopLegacy.Utilities builds successfully
1. Open eShopLegacyMVC.csproj and remove Autofac.WebApi2 reference
1. Remove missing App_Data folder from sln
1. Remove web.config files from `<content>`
1. Remove global.asax.cs updates

## Fix-up controllers

1. The project files are now up-to-date  and some source updates are done, but other source updates are still needed. Some because UA can't handle them, some because it doesn't make them yet.
1. Build the project and note the errors
1. Open controllers and fix remaining build errors
    1. Note how few errors there are
    1. return HttpNotFound() -> return NotFound()
    1. SelectList -> Add namespace
    1. Cast HttpStatusCode fields to int
    1. Replace Bind(Include="..") with Bind(params string[])
    1. Request.Url.Scheme -> Request.Scheme
    1. Server.MapPath => IWebHostEnvironment.WebRootPath
1. At this point, the only build errors are in the global.asax.cs and App_Start items that need replaced

## Replace RouteConfig.cs

1. Compare route with what's in startup.cs and replace default controller name with 'Catalog'
1. Delete RouteConfig.cs

## Replace WebApiConfig.cs

1. Look at WebApiConfig.cs
1. Update Api/CatalogController.cs:
    1. Use `[Route("api/[controller]")]`
1. Update WebApi/FilesController.cs
    1. Use [Route("api/[controller]")]
1. Update WebApi/BrandsController.cs:
    1. Use [Route("api/[controller]")]
    1. IHttpActionResult => IActionResult
    1. ResponseMessage => NotFound() / Ok()
1. Delete WebApiConfig.cs

## Replace FilterConfig.cs

1. Look at FilterConfig.cs
1. Look at startup.cs and note where MVC services are registered and how filters can be added
1. TODO - do we need app.UseExceptionHandler (or a custom filter) to match ASP.NET behavior?
1. Delete FilterConfig.cs

## Replace BundleConfig.cs

TODO

## Replace global.asax.cs

TODO

## Fix-up views

TODO

## Address additional necessary source updates

1. Build the solution and note there are only a couple errors remaining.
1. Fix CatalogDBInitializer by using IWebHostEnvironment.ContentRoot from DI