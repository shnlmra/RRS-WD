

using Menu_Management.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MainPage}/{action=MainPage}/{id?}");

// Custom routes for MenuManagement views
app.MapControllerRoute(
    name: "addMenu",
    pattern: "menu-management/add-menu-modal",
    defaults: new { controller = "MenuManagement", action = "AddMenuModal" });

app.MapControllerRoute(
    name: "deleteMenu",
    pattern: "menu-management/delete-menu-modal",
    defaults: new { controller = "MenuManagement", action = "DeleteMenuModal" });

app.Run();
