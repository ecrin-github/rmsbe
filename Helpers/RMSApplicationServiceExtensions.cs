

namespace RmsService.Extensions
{
    public static class ApplicationServiceExtensions
    {
        /*
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<RmsDbConnection>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("RmsDbConnectionString"));
            });

            services.AddScoped<IDtpRepository, DtpRepository>();
            services.AddScoped<IDupRepository, DupRepository>();
            
            services.AddScoped<IDataMapper, DataMapper>();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = IdentityConfigs.Oidcurl;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false
                    };
                });
            
            return services;
        }
            */
            }


}