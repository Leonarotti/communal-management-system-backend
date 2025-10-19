using CommunalManagementSystem.BusinessWorkflow.Interfaces.BW;
using CommunalManagementSystem.BusinessWorkflow.Interfaces.DA;
using CommunalManagementSystem.BusinessWorkflow.UseCases;
using CommunalManagementSystem.DataAccess.Actions;
using CommunalManagementSystem.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System;


using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using QuestPDF.Infrastructure; // 👈 agrega este using

var builder = WebApplication.CreateBuilder(args);

// 👇 agrega esta línea ANTES de usar QuestPDF
QuestPDF.Settings.License = LicenseType.Community;


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//// Nuevo
// ✅ Configurar JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

///

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

app.UseAuthentication(); // ✅ debe ir antes de Authorization

app.UseAuthorization();

app.MapControllers();

app.Run();
