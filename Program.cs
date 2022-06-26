using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

using rmsbe.Helpers;
using rmsbe.Helpers.Interfaces;
using rmsbe.DataLayer;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Services;
using rmsbe.Services.Interfaces;

/****************************************************************************************************
 * Set up file based configuration environment. 
****************************************************************************************************/

var configFiles = new ConfigurationBuilder()
    .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
    .Build();

/****************************************************************************************************
 * Establish web app builder.
****************************************************************************************************/

var builder = WebApplication.CreateBuilder(args);

/****************************************************************************************************
 * Add services to the builder. 
 * Include configuration details, as defined above. AddControllers will enable core ASP.NetCore API 
 * controller capability. AddEndpointsApiExplorer will enable exploration of endpoints
 * Add CORS will enable CORS to be added in the pipeline
 ****************************************************************************************************/

builder.Configuration.AddConfiguration(configFiles);    
builder.Services.AddControllers();           
builder.Services.AddEndpointsApiExplorer();  
builder.Services.AddCors();

/****************************************************************************************************
 * Setting for the release build for server
 * If a proxy is being used then its IP address needs to be added to the KnownProxies collection in
 * the service configuration, to allow later use of the Forward headers part of the pipeline..
 * But not clear why a proxy server is actually required - at least at this stage...
****************************************************************************************************/
            
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("51.210.99.16"));
});

/****************************************************************************************************
 * Addition of authentication services
 * Previous code below but does not seem compatible with .Net 6.0
 * Needs further investigation...
****************************************************************************************************/

/*
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = IdentityConfigs.Oidcurl;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false
        };
    });
*/

/****************************************************************************************************
 * Set up Swagger documentation generation parameters within the Services
 * Configuring Swagger details at https://aka.ms/aspnetcore/swashbuckle
****************************************************************************************************/

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "The RMS REST API ", Version = "v1" });
    c.EnableAnnotations();
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    // c.AddSecurityDefinition("Bearer", securitySchema);   -- To enable later
    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    };
    // c.AddSecurityRequirement(securityRequirement);      -- To enable later
});

/****************************************************************************************************
 * This added to the default Kestrel configuration to allow synchronous IO (in recent years disabled
 * by default).Not clear why this change was made in the original code.
 * To try running without to see if any problems...
****************************************************************************************************/

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});


/****************************************************************************************************
 * This service added to allow the host scheme (http or https) and host URL to be identified at
 * any point. It is used within a static helper method that creates URLs for paged request responses,
 * that identifies the URL for the first, last, previous and next page and returns them to the front
 * end as [art of the API response object.

****************************************************************************************************/

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUriService>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext?.Request;
    var uri = string.Concat(request?.Scheme, "://", request?.Host.ToUriComponent());
    return new UriService(uri);
});

/****************************************************************************************************
 * Add in dependency injection of the main classes used in the app.
 * Initial three are added as singletons (in effect become static classes).
 * The Lookup service can then act as an in-memory cache for lookup data.
 * The others are 'Add Scoped', causing regeneration oin each separate API call.
****************************************************************************************************/

builder.Services.AddSingleton<ICreds, Creds>();
builder.Services.AddSingleton<ILookupRepository, LookupRepository>();
builder.Services.AddSingleton<ILookupService, LookupService>();

builder.Services.AddScoped<IStudyService, StudyService>();
builder.Services.AddScoped<IStudyRepository, StudyRepository>();
builder.Services.AddScoped<IObjectService, ObjectService>();
builder.Services.AddScoped<IObjectRepository, ObjectRepository>();
builder.Services.AddScoped<IDtpService, DtpService>();
builder.Services.AddScoped<IDupService, DupService>();
builder.Services.AddScoped<IDtpRepository, DtpRepository>();
builder.Services.AddScoped<IDupRepository, DupRepository>();

/****************************************************************************************************
 * This switch is available in later versions of npgsql, as these introduced a breaking change in
 * the handling of datetime data - especially with regard to time zones. Not clear why it was
 * deemed necessary in the context of the RMS backend....
****************************************************************************************************/

// AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

/****************************************************************************************************
// run the build command to create the web app
// and then configure the HTTP request pipeline.
****************************************************************************************************/

var app = builder.Build();

/****************************************************************************************************
 * Put a global exception handler at the beginning of the pipeline (so all following errors
 * are handled). If not in development (when the full stack is displayed - see below) the handler
 * in the listed ExceptionMiddleware class will log the error and return a simple error message./
****************************************************************************************************/

app.UseMiddleware<ExceptionMiddleware>();

/****************************************************************************************************
 * Setting for the release build for server.
 * But unclear why an additional proxy server is seen as necessary...
 *  The proxy IP is set up in the services KnownProxies collection. ForwardedHeaders then need to be 
 *  established in the pipeline to ensure that header information is not stripped from requests as 
 *  they pass down the pipeline
 * 
 *  X-Forwarded-For (XFF)	
 *  Holds information about the client that initiated the request and subsequent proxies in a chain of 
 *  proxies. This parameter may contain IP addresses and, optionally, port numbers. The last proxy in 
 *  the chain isn't in the list of parameters. The last proxy's IP address, and optionally a port 
 *  number, are available as the remote IP address at the transport layer.
 
 *  X-Forwarded-Proto (XFP)	
 *  The value of the originating scheme, HTTP or HTTPS. The value may also be a list of schemes if 
 *  the request has traversed multiple proxies.
****************************************************************************************************/

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

/****************************************************************************************************
 * Establish some development-only dependent parts of the pipeline
 * This includes setting a full stack dump for errors, and setting up the Swagger UI.
 * The css and js injection is used to customise the Swagger header
****************************************************************************************************/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "The Repository Management System REST API";
        c.RoutePrefix = "api/rest/documentation";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "The RMS REST API (v.1)");
        c.InjectStylesheet("/StaticContent/swagger-custom/swagger-custom-styles.css");
        c.InjectJavascript("/StaticContent/swagger-custom/swagger-custom-script.js");
    });
    app.UseDeveloperExceptionPage();                   
}

/****************************************************************************************************
 * Redirect a response to the client if a request is forwarded
 * through an insecure or HTTP configured network, to HTTPS equivalent.
****************************************************************************************************/

app.UseHttpsRedirection();

/****************************************************************************************************
 * Set up this folder for static content (used for Swagger header files, including ECRIN logo).
 * Putting the files in a named folder allows the logo to be set as an embedded resource
 * (a feature not available in wwwroot) so there is no need to use a web URL to reference it.
 ****************************************************************************************************/

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "StaticContent")),
    RequestPath = "/StaticContent"
});


/****************************************************************************************************
 * UseRouting() marks the position in the middleware pipeline where a routing decision is made
 * through matching request to endpoints. In other words, when the endpoint is selected but not,
 * at this stage, actioned. Metadata is added to the request.
****************************************************************************************************/

app.UseRouting();

/****************************************************************************************************
 * Call to Authentication and Authorization middleware must go after UseRouting, so that route
 * information is available for authentication decisions, and before UseEndpoints so that users
 * are authenticated before accessing the endpoints.
 ****************************************************************************************************/

// app.UseAuthentication();  

// app.UseAuthorization();   

/****************************************************************************************************
 * Enable Cross-Origin Requests (CORS), for all requests from any origin
****************************************************************************************************/

app.UseCors(x => x.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

/****************************************************************************************************
 * Establish the point to use the endpoint (as matched at UseRouting, previously) at the end of
 * the pipeline. The appropriate controller function is called here.
 ****************************************************************************************************/

app.UseEndpoints(endpoints =>  endpoints.MapControllers());

// Set it all running!

app.Run();  