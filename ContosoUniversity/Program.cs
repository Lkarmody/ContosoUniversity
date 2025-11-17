using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ContosoUniversity.Data;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddResponseCaching();
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'SchoolContext' not found.")));


//HealthCheck for the database context
builder.Services.AddHealthChecks()
    .AddDbContextCheck<SchoolContext>("SchoolContext_DBContextCheck")
    .AddSqlServer(
        builder.Configuration.GetConnectionString("SchoolContext"),
        name: "SchoolContext_SQLServerCheck"
    );

//Telemetry
builder.Services.AddApplicationInsightsTelemetry();


builder.Services.AddDbContext<ContosoUniversityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ContosoUniversityContextConnection")));



builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ContosoUniversityContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllers();
builder.Services.AddLogging(logging =>
{
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
 
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<SchoolContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);

    var transactionManager = new TransactionManager(context);
    await transactionManager.HandleMultiEntityTransactionAsync();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseResponseCaching();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Logger.LogInformation("Starting application...");

app.MapHealthChecks("/healthz");

app.Run();

public partial class Program { } // Make the implicit Program class public so integration tests can access it