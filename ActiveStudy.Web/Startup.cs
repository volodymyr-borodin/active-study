using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ActiveStudy.AspNetCore.Identity.Mongo;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Identity;
using ActiveStudy.Domain.Crm.Relatives;
using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Students;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Domain.Materials.FlashCards;
using ActiveStudy.Domain.Materials.FlashCards.Progress;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Domain.Materials.TestWorks.Results;
using ActiveStudy.Storage.Mongo;
using ActiveStudy.Storage.Mongo.Crm;
using ActiveStudy.Storage.Mongo.Identity;
using ActiveStudy.Storage.Mongo.Materials;
using ActiveStudy.Storage.Mongo.Materials.FlashCards;
using ActiveStudy.Storage.Mongo.Materials.TestWorks;
using ActiveStudy.Web.Models;
using ActiveStudy.Web.Services;
using ActiveStudy.Web.Services.Email;
using ActiveStudy.Web.Services.Email.Smtp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using static ActiveStudy.Web.Consts;

namespace ActiveStudy.Web;

public class Startup
{
    private const string ResourcesPath = "Resources";
    private static readonly IList<CultureInfo> SupportedCultures = new List<CultureInfo>
    {
        new CultureInfo("uk-UA"),
        new CultureInfo("en-US")
    };
    private static RequestCulture DefaultRequestCulture => new RequestCulture(SupportedCultures.First().Name);

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto;
        });

        switch (Configuration["EMAIL_SENDER"])
        {
            case "SMTP":
                services.AddScoped<IEmailService>(_ => new SmtpEmailService(
                    new SmtpClientConfiguration(
                        Configuration["SMTP_HOST"],
                        Configuration.GetValue<int>("SMTP_POST"),
                        Configuration.GetValue<bool>("SMTP_SSL_ENABLED"),
                        Configuration["SMTP_USERNAME"],
                        Configuration["SMTP_PASSWORD"],
                        Configuration["SMTP_DISPLAY_NAME"],
                        Configuration["SMTP_EMAIL"])));
                break;
            case "CONSOLE":
                services.AddScoped<IEmailService, NullEmailService>();
                break;
            default:
                throw new Exception("Unknown EMAIL_SENDER");
        }

        services.AddScoped<NotificationManager>();

        var mongoUrl = MongoUrl.Create(Configuration["MONGO_CONNECTION"]);

        services.AddIdentity<ActiveStudyUserEntity, ActiveStudyRoleEntity>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;

                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddMongoDbStores<IdentityContext>(mongoUrl)
            .AddDefaultTokenProviders();

        services.AddScoped<UserManager>();
        services.AddScoped<RoleManager>();
        services.AddScoped<IAccessResolver, AccessResolver>();

        services.AddLocalization(options => options.ResourcesPath = ResourcesPath);
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = DefaultRequestCulture;
            options.SupportedCultures = SupportedCultures;
            options.SupportedUICultures = SupportedCultures;
        });

        services.AddScoped<CurrentUserProvider>();

        // common
        services.AddScoped(_ => new CommonContext(mongoUrl));
        services.AddScoped<ISubjectStorage, InMemorySubjectStorage>();
        services.AddScoped<ICountryStorage, InMemoryCountryStorage>();
        services.AddScoped<IAuditStorage, AuditStorage>();

        // crm
        services.AddScoped(_ => new CrmContext(mongoUrl));
        services.AddScoped<ISchoolStorage, SchoolStorage>();
        services.AddScoped<IClassStorage, ClassStorage>();
        services.AddScoped<ITeacherStorage, TeacherStorage>();
        services.AddScoped<IStudentStorage, StudentStorage>();
        services.AddScoped<IRelativesStorage, RelativesStorage>();
        services.AddScoped<ISchedulerStorage, SchedulerStorage>();

        services.AddScoped<DemoSchool>();

        // materials
        services.AddScoped(_ => new MaterialsContext(mongoUrl));
        services.AddScoped<ITestWorksStorage, TestWorksStorage>();
        services.AddScoped<ITestWorkResultsStorage, TestWorkResultsStorage>();
        services.AddScoped<TestWorksService>();

        services.AddScoped<IFlashCardsStorage, FlashCardsStorage>();
        services.AddScoped<FlashCardsService>();
        services.AddScoped<IProgressStorage, ProgressStorage>();
        services.AddScoped<LearningProgressService>();

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
        });

        services.AddControllersWithViews(options => options.UseDateOnlyTimeOnlyStringConverters())
            .AddRazorOptions(options => { options.ViewLocationFormats.Add("/{0}.cshtml"); })
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization(options => {
                options.DataAnnotationLocalizerProvider = (_, factory) =>
                    factory.Create(typeof(SharedResource));
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseForwardedHeaders();
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = DefaultRequestCulture,
            SupportedCultures = SupportedCultures,
            SupportedUICultures = SupportedCultures
        });

        app.UseStaticFiles();

        app.UseRequestLocalization();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapAreaControllerRoute(
                areaName: Area.Crm,
                name: "school",
                pattern: "{area:exists}/{controller=School}/{action=List}/{id?}");
            endpoints.MapAreaControllerRoute(
                areaName: Area.EducationMaterials,
                name: "materials",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
        });

        applicationLifetime.ApplicationStarted.Register(() =>
        {
            using var scope = app.ApplicationServices.CreateScope();
            var materialsContext = scope.ServiceProvider.GetRequiredService<MaterialsContext>();

            var index = Builders<UserCardProgressEntity>.IndexKeys.Combine(
                Builders<UserCardProgressEntity>.IndexKeys.Ascending(progress => progress.UserId),
                Builders<UserCardProgressEntity>.IndexKeys.Ascending(progress => progress.TermId));

            materialsContext.UserCardProgress.Indexes.CreateOneAsync(new CreateIndexModel<UserCardProgressEntity>(index));
        });
    }
}