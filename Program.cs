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

// Session i�in gerekli olan Distributed Memory Cache servisini ekleyin
builder.Services.AddDistributedMemoryCache();

// Session ayarlar�n� ekleyin
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Oturum zaman a��m� s�resi
    options.Cookie.HttpOnly = true; // G�venlik i�in HTTP �zerinden eri�ilebilir
    options.Cookie.IsEssential = true; // Cookie esast�r ve consent gerektirmez
});


// Areas in altindaki razor sayfalarini kullanmak i�in
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

// Rollerin olu�turulmas� i�in gerekli RoleManager servisini al�n
var roleManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

// Rolleri olu�turun
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

// Session middleware'�n� ekleyin
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Rolleri olu�turan metod
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
    // Di�er rolleri de burada olu�turabilirsiniz
}
