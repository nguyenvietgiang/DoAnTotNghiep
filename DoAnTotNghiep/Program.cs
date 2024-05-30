using DNTCaptcha.Core;
using DoAnTotNghiep.Middleware;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.AccountRepo;
using DoAnTotNghiep.Repository.BaseRepo;
using DoAnTotNghiep.Repository.CandidatesRepo;
using DoAnTotNghiep.Repository.CommentRepo;
using DoAnTotNghiep.Repository.ContactRepo;
using DoAnTotNghiep.Repository.DisscussRepo;
using DoAnTotNghiep.Repository.EmployerRepo;
using DoAnTotNghiep.Repository.FollowRepo;
using DoAnTotNghiep.Repository.ImageGaleryRepo;
using DoAnTotNghiep.Repository.JobApplyFormRepo;
using DoAnTotNghiep.Repository.JobRepo;
using DoAnTotNghiep.Repository.PayRepo;
using DoAnTotNghiep.Repository.PolicyRepo;
using DoAnTotNghiep.Repository.SurveyRepo;
using DoAnTotNghiep.Services.EmailServices;
using DoAnTotNghiep.Services.ExportServices;
using DoAnTotNghiep.Services.ImageServices;
using DoAnTotNghiep.Services.OnlineCountServices;
using DoAnTotNghiep.Services.PaymentServices;
using DoAnTotNghiep.Services.VNpayServices;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using Syncfusion.Licensing;
using Serilog.Formatting.Json;
using DoAnTotNghiep.Repository.OnlineResumeRepo;
using DoAnTotNghiep.RealTime;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File(new JsonFormatter(), "logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Connect")));

//services
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IEmailServices, EmailServices>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IExcelExportService, ExcelExportService>();
builder.Services.AddSingleton<IVnPayService, VnPayService>();
builder.Services.AddScoped<IOnlineUsersService, OnlineUsersService>();

//repo
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<ICandidatesRepo, CandidatesRepocs>();
builder.Services.AddScoped<IEmployerRepository, EmployerRepository>();
builder.Services.AddScoped<IDiscussRepository, DiscussRepository>();
builder.Services.AddScoped<IJobPostingRepository, JobPostingRepository>();
builder.Services.AddScoped<IJobApplyFormRepository, JobApplyFormRepository>();
builder.Services.AddScoped<IImageGaleryRepository, ImageGaleryRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ISurveyRepo<Survey>, SurveyRepo>();
builder.Services.AddScoped<ISurveyRepo<Question>, QuestionRepo>();
builder.Services.AddScoped<ISurveyRepo<Option>, OptionRepo>();
builder.Services.AddScoped<IPayRepository, PayRepository>();
builder.Services.AddScoped<IOnlineResumeRepository, OnlineResumeRepository>();
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
builder.Services.AddSignalR();


builder.Services.AddDNTCaptcha(options => { options.UseCookieStorageProvider().ShowThousandsSeparators(false);
    options.WithEncryptionKey("JobFinder2024");
});

builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            });
builder.Services.AddSignalR();

var app = builder.Build();
app.UseMiddleware<OnlineUsersMiddleware>();
//app.UseMiddleware<SessionToClaimsPrincipalMiddleware>();
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

    // Default route for Home/Index
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{language=vie}/{controller=Home}/{action=Index}/{id?}"
    );
});
app.MapHub<ChatHub>("/chathub");
// Remove the previous MapControllerRoute for the default route

app.Run();
