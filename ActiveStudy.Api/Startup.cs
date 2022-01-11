using ActiveStudy.Domain.Materials.FlashCards;
using ActiveStudy.Storage.Mongo.Materials;
using ActiveStudy.Storage.Mongo.Materials.FlashCards;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace ActiveStudy.Api
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment environment;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
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
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.RequireHttpsMetadata = !environment.IsDevelopment();
                    options.ApiName = "api";
                    options.Authority = configuration["AUTHORITY"];
                });

            var mongoUrl = MongoUrl.Create(configuration["MONGO_CONNECTION"]);

            services.AddScoped(_ => new MaterialsContext(mongoUrl));
            services.AddScoped<IFlashCardsStorage, FlashCardsStorage>();
            services.AddScoped<FlashCardsService>();

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
