using API.Errors;
using API.Helper;
using API.Middleware;
using Core.Interface;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StoreContext>(
    option => option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

//automapper config
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(MappingProfile));
//Use for validation errors 
builder.Services.Configure<ApiBehaviorOptions>(option => 
{
    option.InvalidModelStateResponseFactory = actionContext =>
    {
        var error = actionContext.ModelState
            .Where(e=> e.Value != null && e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value!.Errors)
            .Select(x => x.ErrorMessage).ToArray();

        var errorResponse = new ApiValidationErrorResponse
        {
            Errors = error
        };

        return new BadRequestObjectResult(errorResponse);  
    };
});

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularApp",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200",
                                    "https://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
            });
    });

// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     {
//         options.Cookie.SameSite = SameSiteMode.None;
//         options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//         options.Cookie.HttpOnly = true;
//         options.Events = new CookieAuthenticationEvents
//         {
//             OnRedirectToLogin = context =>
//             {
//                 context.Response.StatusCode = StatusCodes.Status401Unauthorized; // 🔥
//                 return Task.CompletedTask;
//             },
//             OnRedirectToAccessDenied = context =>
//             {
//                 context.Response.StatusCode = StatusCodes.Status403Forbidden; // 🔥
//                 return Task.CompletedTask;
//             }
//         };
//     });

// // Authorization
// builder.Services.AddAuthorization();

// builder.Services.ConfigureApplicationCookie(options =>
// {
//     options.Cookie.SameSite = SameSiteMode.None; // Allows cross-origin cookies
//     options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Requires HTTPS
//     options.Cookie.HttpOnly = true; 
//      options.Events = new CookieAuthenticationEvents
//         {
//             OnRedirectToLogin = context =>
//             {
//                 context.Response.StatusCode = StatusCodes.Status401Unauthorized; // 🔥
//                 return Task.CompletedTask;
//             },
//             OnRedirectToAccessDenied = context =>
//             {
//                 context.Response.StatusCode = StatusCodes.Status403Forbidden; // 🔥
//                 return Task.CompletedTask;
//             }
//         };
// });
// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//         .AddCookie();
// builder.Services.AddAuthorization();



// Data Protection
// builder.Services.AddDataProtection()
//     .PersistKeysToFileSystem(new DirectoryInfo(@"C:\keys"))
//     .SetApplicationName("SharedAppName");

builder.Services.AddAuthentication("Identity.Application")
    .AddCookie("Identity.Application", options =>
    {
        options.Cookie.Name = ".AspNetCore.Identity.Application";
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\Shared-Keys\"))
    .SetApplicationName("SharedCookieApp");
    
var app = builder.Build();

//My middle ware here
app.UseMiddleware<ExceptionMiddleware>();

//app.UseStatusCodePagesWithReExecute("/error/{0}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(),"Content")),
    RequestPath = "/Content"
});

app.UseCors("AllowAngularApp");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy();

app.MapControllers();

app.Run();
