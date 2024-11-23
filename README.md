# WebStorageSystem

## Requirements
.NET Core 3.1

MS SQL Server

SendGrid API key for sending emails (if you wanna use email sending)

## How To Run
Set connection string in appsettings.json

Set name of connection string in Startup.cs at line 42

Setup SendGrid API KEY and email in appsettings.json

Setup Admin account in appsettings.json (or leave default values)

## Dependencies

### Server-side Dependencies
AutoMapper.Extensions.Microsoft.DependencyInjection@8.1.1

BuildBundlerMinifier@3.2.449

LinqKit.Microsoft.EntityFrameworkCore@5.1.5

Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore@3.1.23

Microsoft.AspNetCore.Identity.EntityFrameworkCore@3.1.32

Microsoft.AspNetCore.Identity.UI@3.1.32

Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation@3.1.23

Microsoft.EntityFrameworkCore.Design@5.0.15

Microsoft.EntityFrameworkCore.Sqlite@3.1.32

Microsoft.EntityFrameworkCore.SqlServer@5.0.15

Microsoft.EntityFrameworkCore.SqlServer.Design@1.1.6

Microsoft.EntityFrameworkCore.Tools@5.0.15

Microsoft.VisualStudio.Web.BrowserLink@2.2.0

Microsoft.VisualStudio.Web.CodeGeneration.Design@3.1.5

NetBarcode@1.7.2

SendGrid@9.29.3

SendGrid.Extensions.DependencyInjection@1.0.1

Swashbuckle.AspNetCore@6.3.0

System.Text.Json@8.0.4

Z.EntityFramework.Plus.EFCore@5.103.1

### Client-side Dependencies
jquery@3.7.1

jquery-validate@1.21.0

jquery-validation-unobtrusive@4.0.0

popper.js@2.5.2

tippy.js@6.2.6

twitter-bootstrap@4.5.3

select2@4.0.13

luxon@2.1.1

dataTables.js@2.1.5

dataTables.bootstrap4.js@2.1.5

dataTables.responsive.js@3.0.3

responsive.bootstrap4.js@3.0.3
