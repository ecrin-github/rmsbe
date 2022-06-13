using System.Reflection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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
// Add services to the builder (before the build command is executed)
// including configuration details
// configuring Swagger/OpenAPI details at https://aka.ms/aspnetcore/swashbuckle

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddConfiguration(configFiles);  // add the configuration files defined above
builder.Services.AddControllers();                    // add core ASP.NetCore API controller capability
builder.Services.AddEndpointsApiExplorer();           // Configures API Explorer

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

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.AddSingleton<ICredentials, Credentials>();
builder.Services.AddSingleton<ILookupRepository, LookupRepository>();
builder.Services.AddSingleton<ILookupService, LookupService>();

//builder.Services.AddScoped<IStudyRepository, StudyRepository>();
//builder.Services.AddScoped<IObjectRepository, ObjectRepository>();
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
        c.DocumentTitle = "The RMS REST API";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "The RMS REST API (v.1)");
        c.InjectStylesheet("/documentation/swagger-custom/swagger-custom-styles.css");
        c.InjectJavascript("/documentation/swagger-custom/swagger-custom-script.js");
        c.RoutePrefix = "api/rest/documentation";
    });
    
    app.UseDeveloperExceptionPage();                    // seems sensible
}

app.UseHttpsRedirection();
app.UseRouting();

// app.UseAuthentication();     
// app.UseAuthorization();                              // enables authorisation on all controllers
            
app.UseCors("Open");
app.UseEndpoints(endpoints =>  endpoints.MapControllers());


app.Run();  // set it off!