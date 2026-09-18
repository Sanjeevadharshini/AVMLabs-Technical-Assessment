using AVMLabs.Mvc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("AVMLabsApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
});

builder.Services.AddScoped<ApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Home/PageNotFound");

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Reports}/{action=Dashboard}/{id?}")
    .WithStaticAssets();


app.Run();
