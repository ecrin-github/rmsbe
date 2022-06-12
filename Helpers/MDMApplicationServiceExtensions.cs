
namespace MdmService.Extensions
{
    public static class ApplicationServiceExtensions
    {
        /*
public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
{

services.AddDbContext<MdmDbConnection>(options =>
{
    options.UseNpgsql(config.GetConnectionString("MdmDbConnectionString"));
});

services.AddScoped<IStudyRepository, StudyRepository>();
services.AddScoped<IObjectRepository, ObjectRepository>();

services.AddScoped<IAuditService, AuditService>();
services.AddScoped<IUserIdentityService, UserIdentityService>();

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