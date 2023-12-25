using CagriMerkezi2.Models;
using CagriMerkezi2.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<UygulamaDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// benim olusturdugum dbcontexti kullan dedik
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<UygulamaDbContext>().AddDefaultTokenProviders();

// Session için gerekli olan Distributed Memory Cache servisini ekleyin
builder.Services.AddDistributedMemoryCache();

// Session ayarlarýný ekleyin
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Oturum zaman aþýmý süresi
    options.Cookie.HttpOnly = true; // Güvenlik için HTTP üzerinden eriþilebilir
    options.Cookie.IsEssential = true; // Cookie esastýr ve consent gerektirmez
});


// Areas in altindaki razor sayfalarini kullanmak için
builder.Services.AddRazorPages();

builder.Services.AddScoped<IBirimRepository, BirimRepository>();
builder.Services.AddScoped<IDepartmanRepository, DepartmanRepository>();
builder.Services.AddScoped<ISikayetRepository, SikayetRepository>();
builder.Services.AddScoped<ICalisanRepository, CalisanRepository>();
builder.Services.AddScoped<ICagriMerkeziRepository, CagriMerkeziRepository>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ISikayetDurumRepository, SikayetDurumRepository>();
builder.Services.AddScoped<IKullaniciRepository, KullaniciRepository>();

var app = builder.Build();

// Rollerin oluþturulmasý için gerekli RoleManager servisini alýn
var roleManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

// Rolleri oluþturun
await CreateRoles(roleManager);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session middleware'ýný ekleyin
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Rolleri oluþturan metod
async Task CreateRoles(RoleManager<IdentityRole> roleManager)
{
    if (!await roleManager.RoleExistsAsync(KullaniciRolleri.Role_Admin))
    {
        await roleManager.CreateAsync(new IdentityRole(KullaniciRolleri.Role_Admin));
    }
    if (!await roleManager.RoleExistsAsync(KullaniciRolleri.Role_Calisan))
    {
        await roleManager.CreateAsync(new IdentityRole(KullaniciRolleri.Role_Calisan));
    }
    // Diðer rolleri de burada oluþturabilirsiniz
}
