using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CharityFinder.Models;
using CharityFinder.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddDbContext<RazorPagesMovieContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("RazorPagesMovieContext") ?? throw new InvalidOperationException("Connection string 'RazorPagesMovieContext' not found.")));

builder.Services.AddHttpClient<ApiClient>();

var app = builder.Build();


// Configure the HTTP request pipeline.
// sets exception route to /Error
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();  // HTTP requests -> HTTPS
app.UseStaticFiles();       // enables use of static(html, css, images, js) files

app.UseRouting();           // adds route-matching to middleware pipeline

app.UseAuthorization();     // authorizes user to access secure resources

app.MapRazorPages();        // sets up endpoint routing for Razor pages

app.Run();
