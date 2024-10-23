using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options => options.AddDefaultPolicy(
            policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()));

var configuration = builder.Configuration;
//Dependency Insection
builder.Services.AddInfrasturctureServices(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors();
app.Run();
