using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Microsoft.EntityFrameworkCore;
using JobSchedular.Data;
using JobSchedular.Job;
using JobSchedular.Model;
using JobSchedular.Schedular;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IJobFactory, JobSchedular.Job_Factory.JobBuilder>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
builder.Services.AddSingleton<Notification>();
builder.Services.AddSingleton(new JobMetadata(Guid.NewGuid(),typeof(Notification),"Notify job", "0 */3 * ? * *" +
    "" +
    ""));
builder.Services.AddHostedService<SchedularClass>();
builder.Services.AddDbContext<JobSchedularContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JobSchedularContext") ?? throw new InvalidOperationException("Connection string 'JobSchedularContext' not found.")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
