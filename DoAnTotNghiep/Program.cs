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
using DoAnTotNghiep.Common;
using DoAnTotNghiep.Jobs;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using DoAnTotNghiep.Models.Enum;
using Syncfusion.XlsIO.Implementation.Security;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .Enrich.FromLogContext()
    .WriteTo.File(new JsonFormatter(), "logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Connect")));

// seeddata
builder.Services.AddScoped<SeedData>();

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Authority = "https://localhost:5001"; // Địa chỉ của IdentityServer
    options.ClientId = "mvc_client";
    options.ClientSecret = "mvc_secret";
    options.ResponseType = "code";
    options.SaveTokens = true;

    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");

    options.GetClaimsFromUserInfoEndpoint = true;
    options.ClaimActions.MapJsonKey("email", "email");

    options.Events = new OpenIdConnectEvents
    {
        OnUserInformationReceived = async context =>
        {
            var identity = (ClaimsIdentity)context.Principal.Identity;

            foreach (var claim in identity.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            var userEmail = identity.FindFirst("email")?.Value ??
                            identity.FindFirst(ClaimTypes.Email)?.Value ??
                            context.User.RootElement.GetString("email");

            Console.WriteLine($"User Email: {userEmail}");

            if (!string.IsNullOrEmpty(userEmail))
            {
                try
                {
                    using var scope = context.HttpContext.RequestServices.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                    var user = await dbContext.Accounts.FirstOrDefaultAsync(u => u.Email == userEmail);
                    if (user != null)
                    {
                        context.HttpContext.Session.SetString("Accountid", user.UserID.ToString());
                        context.HttpContext.Session.SetInt32("UserRole", (int)user.AccountRole);

                        if (user.AccountRole == AccountRole.EmployerFree || user.AccountRole == AccountRole.EmployerPaid)
                        {
                            context.HttpContext.Session.SetString("EmployerName", user.Email);
                        }
                        else if (user.AccountRole == AccountRole.CandidateFree || user.AccountRole == AccountRole.CandidatePaid)
                        {
                            context.HttpContext.Session.SetString("CandidateName", user.Email);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"User with email {userEmail} not found in database.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while processing user information: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Email not found in claims or user information.");
            }
        }
    };
});


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
builder.Logging.AddSerilog();
builder.Services.AddScoped<BackupRestoreService>();
builder.Services.AddHostedService<ScheduledBackupService>();

var app = builder.Build();
app.AddAutoMigration<DataContext>();
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
