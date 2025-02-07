using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzaMVCProject.Data;
using PizzaMVCProject.Data.Helpers;
using PizzaMVCProject.Interfaces;
using PizzaMVCProject.Models;
using PizzaMVCProject.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

IConfigurationRoot _confString = new ConfigurationBuilder().
    SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

builder.Services.AddDbContext<ApplicationContext>(opt => 
        opt.UseSqlServer(_confString.GetConnectionString("DefaultConnection")));

//builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 5;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

//Парсинг конфигурации почты и добавление в сервисы
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig!);


//Подключаем сервис для отправки почты
builder.Services.AddScoped<EmailSender>();


//Время существования токена, для восстановления пароля - 1 час.
builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(1));


builder.Services.AddScoped<ICategory, CategoryRepository>();
builder.Services.AddScoped<IProduct, ProductRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var context = services.GetRequiredService<ApplicationContext>();
        await DbInit.InitializeAsync(context, userManager, rolesManager);
        // тут инициализация категорий
       
        await DbInit.CreateSeedDataAsync(context, categories: new string[] {"1", "2", "3"});
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
