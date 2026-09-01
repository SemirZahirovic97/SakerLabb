using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
using SakerLabb.Web.Components;
using SakerLabb.Web.Data;
using SakerLabb.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<ImportService>();

builder.Services.AddSingleton<Db>();
builder.Services.AddScoped<TicketRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddSingleton<FileService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

app.Services.GetRequiredService<Db>().Initialize();

app.UseDeveloperExceptionPage();
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Powered-By"] = "SakerLabb 1.4.2 (ASP.NET Core 10.0)";
    context.Response.Headers["X-Backend-Node"] = Environment.MachineName;
    await next();
});

app.UseCors();

app.UseStaticFiles();


app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>();

app.Run();
