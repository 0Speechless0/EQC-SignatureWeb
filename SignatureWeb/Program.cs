using EQC.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using SignatureWeb;
using SignatureWeb.Data;
using SignatureWeb.Services;
using SignatureWeb.Shared.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Azure.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<APIService>();
builder.Services.AddControllers();
builder.Services.AddDbContextFactory<MyDbContext>(item => item.UseSqlServer(builder.Configuration.GetConnectionString("MyDbContext")));
builder.Services.AddSingleton<ConstCheckSignatureService>();
builder.Services.AddSingleton<MySqlConnection>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.MapControllers();


app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


//app.Use(async (context, next) =>
//{
//    APIService apiService = new APIService();
//    string virtualPath = builder.Configuration["virtualPath"].ToString();
//    if (context.Request.Path.Value.StartsWith($"{virtualPath}/page"))
//    {
//        string token = context.Session.GetString("token") ?? context.Request.Query["token"].ToString();
//        if (!apiService.checkTokenVaild(token) )
//        {
//            await context.Response.WriteAsync("authentication failed !");
//        }
//        else
//        {
//            await next.Invoke();
//        }
//    }
//    else
//    {
//        await next.Invoke();
//    }



//    // Do logging or other work that doesn't write to the Response.
//});

app.Run();
