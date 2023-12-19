using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.BaseRepo;
using DoAnTotNghiep.Repository.CandidatesRepo;
using DoAnTotNghiep.Repository.ContactRepo;
using DoAnTotNghiep.Repository.EmployerRepo;
using DoAnTotNghiep.Services.EmailServices;
using DoAnTotNghiep.Services.ImageServices;
using DoAnTotNghiep.Services.PaymentServices;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Licensing;

var builder = WebApplication.CreateBuilder(args);
 

builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Connect")));

//services
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IEmailServices, EmailServices>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
//repo
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<ICandidatesRepo, CandidatesRepocs>();
builder.Services.AddScoped<IEmployerRepository, EmployerRepository>();

// khai báo mã syncfusion phục vụ nhập/xuất file-extend
SyncfusionLicenseProvider.RegisterLicense("MTQwNUAzMTM4MmUzNDJlMzBGT29sdENza2kyME1jUHpPNVd5enVXY1AvNVZ1SVdPQlVMNUE4R1c1M0FvPQ==");

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie = new CookieBuilder
    {
        Name = ".MySession",
        HttpOnly = true,
        IsEssential = true,
    };
});
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{

    endpoints.MapControllerRoute(
        name: "error",
        pattern: "/Error/{statusCode}",
        defaults: new { controller = "Error", action = "HttpStatusCodeHandler" }
    );
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
