using Microsoft.EntityFrameworkCore;
using RRS.Data;
using RRS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("EriceConnection")));

builder.Services.AddScoped<PayMongoService>();
builder.Services.AddHttpClient<PayMongoService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<PayMongoService>();
builder.Services.AddScoped<IEmailService, EmailService>();



// Build the application
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configure routes properly
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Uncomment this if you want a specific route for the AboutUs controller
//app.MapControllerRoute(
//    name: "Home",
//    // pattern: "{controller=Home}/{action=Index}/{id?}");
//    pattern: "{controller=Customer}/{action=Layout}/{id?}");


app.Run();
