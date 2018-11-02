dotnet new classlib -n %1.Common
dotnet add %1.Common package Yunyong.Core
dotnet add %1.Common package Yunyong.EventBus
del %1.Common\Class1.cs

dotnet new classlib -n %1.Data
dotnet add %1.Data package Yunyong.Core
dotnet add %1.Data package Microsoft.EntityFrameworkCore
del %1.Data\Class1.cs

dotnet new classlib -n %1.Models
dotnet add %1.Models reference %1.Common
del %1.Models\Class1.cs

dotnet new classlib -n %1.ViewModels
dotnet add %1.ViewModels package Yunyong.Core
dotnet add %1.ViewModels reference %1.Common
del %1.ViewModels\Class1.cs

dotnet new classlib -n %1.Services.Abstractions
dotnet add %1.Services.Abstractions reference %1.ViewModels
del %1.Services.Abstractions\Class1.cs

dotnet new classlib -n %1.Events
dotnet add %1.Events package Yunyong.EventBus
dotnet add %1.Events reference %1.ViewModels
del %1.Events\Class1.cs

dotnet new classlib -n %1.EventHandlers
dotnet add %1.EventHandlers package Yunyong.Core
dotnet add %1.EventHandlers package Yunyong.EventBus
dotnet add %1.EventHandlers reference %1.Common
dotnet add %1.EventHandlers reference %1.Events
dotnet add %1.EventHandlers reference %1.Models
del %1.EventHandlers\Class1.cs

dotnet new classlib -n %1.Services
dotnet add %1.Services package Yunyong.DataExchange
dotnet add %1.Services package Yunyong.SqlUtils
dotnet add %1.Services reference %1.Services.Abstractions
dotnet add %1.Services reference %1.Events
dotnet add %1.Services reference %1.Models
del %1.Services\Class1.cs


dotnet new web -n %1.Identity.WebAPI
dotnet add %1.Identity.WebAPI package IdentityServer4
dotnet add %1.Identity.WebAPI package Yunyong.Core
dotnet add %1.Identity.WebAPI package Yunyong.EventBus.EasyNetQ
dotnet add %1.Identity.WebAPI package Swashbuckle.AspNetCore
dotnet add %1.Identity.WebAPI package System.Text.Encoding.CodePages
dotnet add %1.Identity.WebAPI package Pomelo.EntityFrameworkCore.MySql
dotnet add %1.Identity.WebAPI package Yunyong.Mvc
dotnet add %1.Identity.WebAPI package Yunyong.SqlUtils

dotnet add %1.Identity.WebAPI reference %1.Data
dotnet add %1.Identity.WebAPI reference %1.EventHandlers
dotnet add %1.Identity.WebAPI reference %1.Services


dotnet new classlib -n %1.Platform.Controllers
dotnet add %1.Platform.Controllers reference %1.Services.Abstractions
del %1.Platform.Controllers\Class1.cs

dotnet new web -n %1.Platform.WebAPI
dotnet add %1.Platform.WebAPI package Yunyong.Core
dotnet add %1.Platform.WebAPI package Yunyong.EventBus.EasyNetQ
dotnet add %1.Platform.WebAPI package Swashbuckle.AspNetCore
dotnet add %1.Platform.WebAPI package System.Text.Encoding.CodePages
dotnet add %1.Platform.WebAPI package Yunyong.Cache.Register
dotnet add %1.Platform.WebAPI package Yunyong.Mvc
dotnet add %1.Platform.WebAPI package Yunyong.SqlUtils
dotnet add %1.Platform.WebAPI package IdentityServer4.AccessTokenValidation
dotnet add %1.Platform.WebAPI reference %1.Platform.Controllers
dotnet add %1.Platform.WebAPI reference %1.Services

dotnet new sln -n %1.Fantasy

dotnet sln add %1.Identity.WebAPI
dotnet sln add %1.Common
dotnet sln add %1.Data
dotnet sln add %1.Models
dotnet sln add %1.Services
dotnet sln add %1.Services.Abstractions
dotnet sln add %1.ViewModels
dotnet sln add %1.EventHandlers
dotnet sln add %1.Events
dotnet sln add %1.Platform.Controllers
dotnet sln add %1.Platform.WebAPI