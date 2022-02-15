# ASP.NET Upgrade Sample

This repo contains the legacy eShop sample (from https://github.com/dotnet-architecture/eShopModernizing/tree/main/eShopLegacyMVCSolution/src/eShopLegacyMVC) with a few minor changes to make the csproj not use `<PackageReference>`.

This solution can be used as a starting point for demonstrating upgrading an MVC app to ASP.NET Core.

This repo contains three versions of the eShop sample:

* The **[net472](net472)** folder contains the original application targeting .NET Framework 4.7.2
* The **[net6](net6)** folder contains a completely upgraded version of the application targeting .NET 6. Note that for this exercise, the solution was upgraded to .NET 6 as directly as possible without upgrading underlying architectural decisions. Specifically, that means:
  * The solution is using EntityFramework 6.4 (udated from 6.2) instead of Entity Framework Core. If this project is still in active development, it may make sense to also upgrade to Entity Framework Core but that's out-of-scope of an initial ASP.NET Core upgrade.
  * The solution is using WebOptimizer to serve bundled js and css insteaed of npm and WebPack. Using npm and WebPack would be a better long-term solution but involves further upgrading and refactoring of front-end components.
  * On the other hand, the Autofac dependency was removed because it was such light usage that it was very easy to use Microsoft.Extensions.DependencyInjection instead.
* The **[UpgradeAssistant-Output](UpgradeAssistant-Output)** folder contains the solution after running Upgrade Assistant version 0.3.310302 on the solution from the net472 folder and making no additional manual fixes.
