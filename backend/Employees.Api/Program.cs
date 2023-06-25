using Employees.Core.DbContexts;
using Employees.Core.Domain.Repositories;
using Employees.Core.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", false, true)
               .AddJsonFile($"appsettings.{env}.json", true)
               .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();


try
{
    Log.Logger.Information("Starting up...");

    // Add services to the container.
    var services = builder.Services;

    services.AddControllers();
    services.AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddSerilog();

    services.AddDbContext<EmployeesDbContext>(options =>
        options.UseSqlServer(config.GetSection("ConnectionStrings:EmployeesDb").Value));

    services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
    services.AddTransient<IUserService, UserService>();
    services.AddTransient<IEmployeeService, EmployeeService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.

    using (var serviceScope = app?.Services?.GetService<IServiceScopeFactory>()?.CreateScope())
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<EmployeesDbContext>();
        context.Database.EnsureCreated();
    }


    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    await app.RunAsync();

}

catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Application start-up failed");
    throw;
}

finally
{
    Log.CloseAndFlush();
}
