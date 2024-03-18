using API.Errors;
using API.Middleware;
using Core.Interface;
using Infrastructure.Data;
using Infrastructure.Services;
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

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
//automapper config
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//Use for validation errors 
builder.Services.Configure<ApiBehaviorOptions>(option => 
{
    option.InvalidModelStateResponseFactory = actionContext =>
    {
        var error = actionContext.ModelState
            .Where(e=>e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
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
    options.AddPolicy("AllowAngularOrigins",
    builder =>
    {
        builder.WithOrigins(
                            "http://localhost:4200"
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod();
    });
});

//builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("Cloudinary"));

var app = builder.Build();

//My middle ware here
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/error/{0}");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(),"Content")),
    RequestPath = "/Content"
});

app.UseCors("AllowAngularOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
