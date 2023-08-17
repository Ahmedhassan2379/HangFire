using Hangfire;
using Hangfire.SqlServer;
using HangFire.ExportFile.Interface;
using HangFire.ExportFile.Service;
using HangFire.ListToDataTable;
using HangFire.Mail.Interfaces;
using HangFire.Mail.Services;
using HangFire.Models;
using HangFire.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<ISendMails, SendMail>();
builder.Services.AddHangfire(configuration => configuration
       .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
       .UseSimpleAssemblyNameTypeSerializer()
       .UseRecommendedSerializerSettings()
       .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
       {
           CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
           SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
           QueuePollInterval = TimeSpan.Zero,
           UseRecommendedIsolationLevel = true,
           DisableGlobalLocks = true
       }));
builder.Services.AddHangfireServer();
builder.Services.AddTransient<IExportFile,ExportFile>();
builder.Services.AddTransient<IListToDataTable, ListToDataTable>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHangfireDashboard("/dashboard");

app.MapControllers();

app.Run();
