using Microsoft.EntityFrameworkCore;
using MVCTutorial.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configurando o banco de dados
builder.Services.AddDbContext<ConnectionDB>(options =>
    options.UseSqlServer("Server=Quesia_NOTE;Database=MVCTutorial;Integrated Security=True;TrustServerCertificate=True;"));

// Add services to the container.
builder.Services.AddControllersWithViews();

//Habilitando o uso do HttpContext.Session. para o Login
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); // Cache da sessão em memória
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo de expiração da sessão
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// quando compilado direciona para este endpoint
app.MapGet("/", () => Results.Redirect("/Teste/Login"));

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Teste/AddInfo");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
