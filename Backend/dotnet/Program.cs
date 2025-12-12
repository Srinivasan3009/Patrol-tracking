using Microsoft.Extensions.Options;
using MongoDB.Driver;
using dotnet.config;
using dotnet.services;
using dotnet.models;
using dotnet.Hubs;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.Configure<MongoDBSettings>(
builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailHelper>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddScoped<IPasswordHasher<employeeModel>, PasswordHasher<employeeModel>>();
builder.Services.AddScoped<IPasswordHasher<SignupModel>, PasswordHasher<SignupModel>>();
builder.Services.AddScoped<IPasswordHasher<SignupModel>, PasswordHasher<SignupModel>>();

builder.Services.AddScoped(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Add controllers
builder.Services.AddControllers();

// Swagger (optional, for testing via browser)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "sample ",
        Version = "v1"
    });
}
);

builder.Services.AddScoped<IPasswordHasher<SignupModel>, PasswordHasher<SignupModel>>();
builder.Services.AddScoped<IPasswordHasher<UpdateUserModel>, PasswordHasher<UpdateUserModel>>();

builder.Services.AddScoped<LoginServices>();
builder.Services.AddTransient<LocationService>();
builder.Services.AddTransient<employeeServices>();
builder.Services.AddTransient<LoginServices>();
builder.Services.AddTransient<UserServices>();
builder.Services.AddTransient<RoleServices>();
builder.Services.AddTransient<WorkflowServices>();
builder.Services.AddTransient<TaskServices>();
builder.Services.AddSignalR();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "login");
        c.RoutePrefix = string.Empty;
    });
}
app.UseCors("AllowAll");
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<TaskNotificationHub>("/taskHub");
app.Run();