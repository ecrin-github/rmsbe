using System.Reflection;
using rmsbe.Helpers;
using rmsbe.Helpers.Interfaces;

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
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICredentials, Credentials>();

// run the build command to create the web app
// and then configure the HTTP request pipeline.

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();                                // enables authorisation on all controllers
app.MapControllers();                                  // scan the controllers for route info

app.Run();  // set it off!