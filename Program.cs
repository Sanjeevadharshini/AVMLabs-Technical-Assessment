using AVMLabs.Api.Data;
using AVMLabs.Api.DTOs.Common;
using AVMLabs.Api.Logging;
using AVMLabs.Api.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var message = string.Join(" ", context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));

        return new BadRequestObjectResult(ApiResponse<object>.FailureResponse(string.IsNullOrWhiteSpace(message) ? "Invalid request." : message));
    };
});

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<ErrorLogWriter>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    await DbSeeder.SeedAsync(context);
}

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "AVMLabs API v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();