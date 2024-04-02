using Data.DataContext;
using HalloDoc.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;

using Services.Implementation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HalloDocDbContext>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IValidation, Validation>();
builder.Services.AddTransient<IPatientRequest, PatientRequest>();
builder.Services.AddTransient<IFamilyRequest, FamilyRequest>();
builder.Services.AddTransient<IConciergeRequest, ConciergeRequest>();
builder.Services.AddTransient<IBusinessRequest, BusinessRequest>();
builder.Services.AddTransient<IDashboard, Dashboard>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IDashboardData, DashboardData>();
builder.Services.AddScoped<ICaseActions, CaseActions>();
builder.Services.AddScoped<IJwtRepository, JwtRepository>();
builder.Services.AddScoped<IAuthorization, Authorization>();
builder.Services.AddScoped<IProviderServices, ProviderServices>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();


app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=AdminLogin}/{id?}");

app.Run();


//Scaffold - DbContext "User ID =postgres;Password=178@tatva;Server=localhost;Port=5432;Database=HalloDocDb;Integrated Security=true;Pooling=true;" Npgsql.EntityFrameworkCore.PostgreSQL - OutputDir "Entity" –context "HalloDocDbContext" –contextDir "DataContext" -f;