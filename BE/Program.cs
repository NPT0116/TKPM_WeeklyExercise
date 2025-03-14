using BE.Config;
using BE.Data;
using BE.Interface;
using BE.Middlewares;
using BE.Repository;
using BE.Services;
using BE.Utils;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection; // Needed for Assembly

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole(); // Log to console
    loggingBuilder.AddDebug();   // Log to debug output
});
builder.Services.AddProblemDetails();
builder.Configuration.AddJsonFile("seedData.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("email.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Student management API",
        Version = "v1",
        Description = "API for Student Management System"
    });
    c.OperationFilter<FileUploadOperationFilter>();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()  // Chấp nhận tất cả nguồn (nên giới hạn trong production)
                  .AllowAnyMethod()   // Chấp nhận tất cả HTTP methods (GET, POST, PUT, DELETE)
                  .AllowAnyHeader();  // Chấp nhận tất cả headers
        });
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.Configure<BusinessRulesSettings>(
    builder.Configuration.GetSection("BusinessRules"));
builder.Services.AddControllers();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddSingleton<IBusinessRulesService, BusinessRulesService>();
builder.Services.AddHostedService<ApplyMigrationService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IApplicationProgramRepository, ApplicationProgramRepository>();
builder.Services.AddScoped<IStudentStatusRepository, StudentStatusRepository>();
builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
builder.Services.AddScoped<IStudentExportService, StudentExportService>();
builder.Services.AddScoped<IStudentImportService, StudentImportService>();
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<PhoneSetting>(builder.Configuration.GetSection("PhoneSetting"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSenderSettings"));
builder.Services.Configure<StudentStatusTransitionConfig>(builder.Configuration.GetSection("StudentStatusTransitionSettings"));
builder.Services.Configure<StudentDeletionSetting>(builder.Configuration.GetSection("StudentDeletionSettings"));
builder.Services.AddScoped<IStudentStatusTransitionService, StudentStatusTransitionService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandlers>();
builder.Services.AddScoped<IValidateStudentEmail, ValidateStudentEmail>();
builder.Services.AddScoped<IValidateStudentPhone, ValidateStudentPhone>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build();

app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

var logger = app.Services.GetRequiredService<ILogger<Program>>(); // Get logger instance
var assembly = Assembly.GetExecutingAssembly();
var version = assembly.GetName().Version?.ToString() ?? "Unknown";
var buildDate = System.IO.File.GetLastWriteTime(assembly.Location).ToString("yyyy-MM-dd HH:mm:ss");
logger.LogInformation("Application Version: {Version}, Build Date: {BuildDate}", version, buildDate);

app.UseHttpsRedirection();
app.UseExceptionHandler(); // Works
app.MapControllers();
app.Run();
