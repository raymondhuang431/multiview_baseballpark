using Microsoft.EntityFrameworkCore;
using Mutiview_BaseballPark.Data;

var builder = WebApplication.CreateBuilder(args);

string connectionString;

// Render 會自動提供 DATABASE_URL 環境變數
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl))
{
    // 修正 scheme
    databaseUrl = databaseUrl.Replace("postgresql://", "postgres://");
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');
    var npgsqlBuilder = new Npgsql.NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = 5432,
        Username = userInfo[0],
        Password = userInfo[1],
        Database = uri.AbsolutePath.TrimStart('/'),
        SslMode = Npgsql.SslMode.Require,
        TrustServerCertificate = true
    };
    connectionString = npgsqlBuilder.ToString();
    Console.WriteLine($"[DEBUG] connectionString: {connectionString}");
}
else
{
    // 本地開發時，吃 appsettings.json 或 appsettings.Development.json 的 DefaultConnection
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine($"[DEBUG] Fallback connectionString: {connectionString}");
}

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add PostgreSQL Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
