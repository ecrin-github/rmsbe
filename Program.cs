using System.Reflection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using rmsbe.Helpers;
using rmsbe.Helpers.Interfaces;
using rmsbe.DataLayer;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Services;
using rmsbe.Services.Interfaces;

// Set up file based configuration environment.

var configFiles = new ConfigurationBuilder()
    .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
    .Build();

// Establish web app builder 

var builder = WebApplication.CreateBuilder(args);

// Add services to the builder (before the build command is executed)
// including configuration details, as defined above.
// AddControllers will enable core ASP.NetCore API controller capability.
// AddEndpointsApiExplorer will enable exploration of endpoints
// Add CORS will enable CORS to be added in the pipeline

builder.Configuration.AddConfiguration(configFiles);    
builder.Services.AddControllers();           
builder.Services.AddEndpointsApiExplorer();  
builder.Services.AddCors();

// set up Swagger documentation generation within the Services
// configuring Swagger details at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "The RMS REST API ", Version = "v1" });
    c.EnableAnnotations();
    /*
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
    c.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    };
    c.AddSecurityRequirement(securityRequirement);
    */
});

// Add this to the default Kestrel configuration

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

// Add these as singletons (in effect static)

builder.Services.AddSingleton<ICredentials, Credentials>();
builder.Services.AddSingleton<ILookupRepository, LookupRepository>();
builder.Services.AddSingleton<ILookupService, LookupService>();

// Add these as scoped ( = per API call)

builder.Services.AddScoped<IStudyService, StudyService>();
builder.Services.AddScoped<IStudyRepository, StudyRepository>();
builder.Services.AddScoped<IObjectService, ObjectService>();
builder.Services.AddScoped<IObjectRepository, ObjectRepository>();
builder.Services.AddScoped<IDtpRepository, DtpRepository>();
builder.Services.AddScoped<IDupRepository, DupRepository>();


// run the build command to create the web app

var app = builder.Build();

// and then configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "The Repository Management System REST API";
        c.RoutePrefix = "api/rest/documentation";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "The RMS REST API (v.1)");
        
        // css and js injection customise the header
        c.InjectStylesheet("/StaticContent/swagger-custom/swagger-custom-styles.css");
        c.InjectJavascript("/StaticContent/swagger-custom/swagger-custom-script.js");
    });
    app.UseDeveloperExceptionPage();                    // full stack shown on error
}

// redirect a response to the client if a request is forwarded
// through an insecure or HTTP configured network, to HTTPS equivalent.

app.UseHttpsRedirection();

// Set up this folder for static content
// (used for Swagger header files, including ECRIN logo)

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "StaticContent")),
    RequestPath = "/StaticContent"
});

// UseRouting() marks the position in the middleware pipeline where a routing decision is 
// made through matching request to endpoints. In other words, when the endpoint is selected
// but not, at this stage, actioned. Metadata is added to the request.

app.UseRouting();

// call to Authentication and Authorization middleware must go after
// UseRouting, so that route information is available for
// authentication decisions, and before UseEndpoints
// so that users are authenticated before accessing the endpoints.

// app.UseAuthentication();  

// app.UseAuthorization();                              

// Enable Cross-Origin Requests (CORS)
// For all requests from any origin

app.UseCors(x => x.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

// Execute the endpoint (as matched at UseRouting, above). 
// the appropriate function is called on the correct controller.

app.UseEndpoints(endpoints =>  endpoints.MapControllers());

// Set it running!

app.Run();  