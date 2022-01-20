using ActiveStudy.Domain.Materials.FlashCards;
using ActiveStudy.Domain.Materials.FlashCards.Progress;
using ActiveStudy.Storage.Mongo.Materials;
using ActiveStudy.Storage.Mongo.Materials.FlashCards;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace ActiveStudy.Api
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment hostEnvironment;

        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            this.configuration = configuration;
            this.hostEnvironment = hostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ActiveStudy API",
                    Version = "1.0.0"
                });
            });

            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = !hostEnvironment.IsDevelopment();
                    options.Authority = configuration["AUTHORITY"];
                    options.TokenValidationParameters.ValidateAudience = false;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Education/FlashCards", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "education:flash-cards");
                });
            });

            var mongoUrl = MongoUrl.Create(configuration["MONGO_CONNECTION"]);

            services.AddScoped(_ => new MaterialsContext(mongoUrl));
            services.AddScoped<IFlashCardsStorage, FlashCardsStorage>();
            services.AddScoped<FlashCardsService>();
            services.AddScoped<IProgressStorage, ProgressStorage>();
            services.AddScoped<LearningProgressService>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ActiveStudy V1");
                });

            app.UseRouting();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
