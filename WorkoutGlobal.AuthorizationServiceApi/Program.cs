using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkoutGlobal.AuthorizationServiceApi.Extensions;
using WorkoutGlobal.Shared.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers()
    .AddFluentValidation(configuration =>
    {
        configuration.RegisterValidatorsFromAssemblyContaining<Program>();
        configuration.DisableDataAnnotationsValidation = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositories();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureIdentity();
builder.Services.ConfigureAttributes();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JwtSettings:ValidIssuer").Value,
        ValidAudience = builder.Configuration.GetSection("JwtSettings:ValidAudience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("JwtSettings:Key").Value))
    };
});
builder.Services.ConfigureMassTransit(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var bus = Bus.Factory.CreateUsingRabbitMq(config =>
{
    config.Host(builder.Configuration["MassTransitSettings:Host"]);

    config.ReceiveEndpoint(builder.Configuration["MassTransitSettings:UpdateQueue"], c =>
    {
        c.Handler<UpdateUserMessage>(async ctx => await Console.Out.WriteLineAsync(ctx.Message.UpdationId.ToString()));
    });
    config.ReceiveEndpoint(builder.Configuration["MassTransitSettings:DeleteQueue"], c =>
    {
        c.Handler<DeleteUserMessage>(async ctx => await Console.Out.WriteLineAsync(ctx.Message.DeletionId.ToString()));
    });
});

bus.Start();

app.Run();
