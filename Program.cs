using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DutchContextDb") ?? throw new InvalidOperationException("Connection string 'DutchContextConnection' not found.");

builder.Services.AddIdentity<StoreUser, IdentityRole>(cfg=>
{
    cfg.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<DutchContext>();
builder.Services.AddTransient <DutchSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IDutchRepository, DutchRepository>();
builder.Services.AddDbContext<DutchContext>(options =>
    options.UseSqlServer(connectionString));

var _config = builder.Configuration;

builder.Services.AddAuthentication()
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Unauthorized/";
            options.AccessDeniedPath = "/Account/Forbidden/";
        })
        .AddJwtBearer(cfg =>
        {
            cfg.SaveToken = true;
            cfg.RequireHttpsMetadata = false;
            cfg.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidIssuer = _config["Tokens:Issuer"],
                ValidAudience = _config["Tokens:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
            };
        });

builder.Services.AddTransient<IMailService, NullMailService>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation()
    .AddNewtonsoftJson(cfg => cfg.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "/seed")
{
    SeedData(app);
}

void SeedData(IHost? app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<DutchSeeder>();
        service.SeedAsync().Wait();
    }
}



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=App}/{action=Index}/{id?}");

app.Run();
