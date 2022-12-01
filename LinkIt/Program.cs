using LinkIt.Models.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(System.Net.IPAddress.Any, 5160);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LinkItContext>(
    options => options.UseMySql(
        builder.Configuration.GetConnectionString("MyDb"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.6.7-mariadb")
        ).UseLazyLoadingProxies()
    );

builder.Services.AddSession();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Link/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Link}/{action=Index}/{id?}");

app.Run();
