using System.Text.Json.Serialization;
using Identity.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(options => 
{
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<IdentityContext>(options =>
{
    options.UseSqlite("Data Source=rbac-identity.db", opt =>
    {
        opt.MigrationsAssembly(typeof(IdentityContext).Assembly.GetName().Name);
    });

    options.ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning));
});

builder.Services.AddOptions();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapControllers();

app.UseHttpsRedirection();

//do code first so we dont need run query to generating the database
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        Task.Run(async () =>
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();

            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(IdentityContext).Name);

            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await dbContext.Database.MigrateAsync();
            });
        }).Wait();

    }
    catch (Exception ex)
    {
        logger.LogError("An error occured while running auto-migration");
        logger.LogError(ex.ToString());
    }
}

app.Run();