using EventBus.Base.Standard.Configuration;
using EventBus.RabbitMQ.Standard.Configuration;
using EventBus.RabbitMQ.Standard.Options;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using SocialService.Application.Services;
using SocialService.Core.Interfaces;
using SocialService.Core.Interfaces.Repositories;
using SocialService.Core.Interfaces.Services;
using SocialService.DataAccess;
using SocialService.DataAccess.Repository;
using SocialService.WebApi.Extensions;
using SocialService.WebApi.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<SocialServiceContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
builder.Services.AddScoped<ILectureRepository, LectureRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
//builder.Services.AddHangfireToApp(builder.Configuration);

// var rabbitMqOptions = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();
// builder.Services.AddRabbitMqConnection(rabbitMqOptions);
// builder.Services.AddRabbitMqRegistration(rabbitMqOptions);
// builder.Services.AddEventBus();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.ConfigureEventBus();
//app.UseHangfireDashboard();
app.UseExceptionHandler();
app.UseRouting();
app.UseHttpsRedirection();

app.UseEndpoints(ep => ep.MapControllers());

app.Run();
