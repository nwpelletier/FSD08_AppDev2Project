using FSD08_AppDev2Project.Models;
using FSD08_AppDev2Project.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddDbContext<AppDev2DbContext>();

builder.Services.AddDbContext<AppDev2DbContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddDbContext<AppDev2DbContext>(options => 
//     options.UseSqlServer(
//         builder.Configuration.GetConnectionString("DefaultConnection"))
//         .EnableRetryOnFailure());

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<AppDev2DbContext>();


builder.Services.Configure<IdentityOptions>(options =>{
    //Password settings
    options.Password.RequiredLength = 6;
    //Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //User settings
    options.User.RequireUniqueEmail = false;
    //SignIn options
    options.SignIn.RequireConfirmedEmail = false;
    
});

builder.Services.ConfigureApplicationCookie(options => {
    //Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(25);

    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/AccessDenied";
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
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
