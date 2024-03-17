using HalloDoc.DbEntity.Data;
using HalloDoc.Repositories.Implementation;
using HalloDoc.Repositories.Interfaces;
using HalloDoc.Service.Implementation;
using HalloDoc.Service.Interface;
using HalloDoc.services.Implementation;
using HalloDoc.services.Interface;
using HalloDoc.Services;
using HalloDoc.Services.Interfaces;
using Microsoft.AspNetCore.Session;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IPatient, Patient>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAAdmin, AAdmin>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IJwtToken, JwtToken>();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Patient/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=PatientSite}/{id?}");

app.Run();


