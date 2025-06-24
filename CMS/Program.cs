using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
using CommunalManagementSystem.BusinessWorkflow.Interfaces.DA;
using CommunalManagementSystem.BusinessWorkflow.UseCases;
using CommunalManagementSystem.DataAccess.Actions;
using CommunalManagementSystem.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency injection
builder.Services.AddTransient<IManageAuthUserDA, ManageAuthUserDA>();
builder.Services.AddTransient<IManagePersonDA, ManagePersonDA>();
builder.Services.AddTransient<IManageIncomeDA, ManageIncomeDA>();
builder.Services.AddTransient<IManageExpenseDA, ManageExpenseDA>();
builder.Services.AddTransient<IManageQuotaDA, ManageQuotaDA>();

builder.Services.AddTransient<IManageAuthUserBW, ManageAuthUserBW>();
builder.Services.AddTransient<IManagePersonBW, ManagePersonBW>();
builder.Services.AddTransient<IManageIncomeBW, ManageIncomeBW>();
builder.Services.AddTransient<IManageExpenseBW, ManageExpenseBW>();
builder.Services.AddTransient<IManageQuotaBW, ManageQuotaBW>();
builder.Services.AddScoped<IDashboardBW, DashboardBW>();


builder.Services.AddDbContext<CommunalManagementSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{ 
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
