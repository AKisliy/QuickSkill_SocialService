using System.Reflection;
using System.Text.Json.Serialization;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using SocialService.Application.Services;
using SocialService.Core.Interfaces.Repositories;
using SocialService.Core.Interfaces.Services;
using SocialService.Core.Interfaces.Utils;
using SocialService.DataAccess;
using SocialService.DataAccess.Repository;
using SocialService.Infrastructure;
using SocialService.WebApi.Extensions;
using SocialService.WebApi.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<SocialServiceContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
builder.Services.AddScoped<ILectureRepository, LectureRepository>();
builder.Services.AddScoped<ILeagueRepository, LeagueRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILeagueService, LeagueService>();
builder.Services.AddScoped<IDiscussionService, DiscussionService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<IBotsGenerator, BotsGenerator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddMassTransitWithRabbitMQ();
builder.Services.AddHangfireToApp(builder.Configuration);

// var rabbitMqOptions = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();
// builder.Services.AddRabbitMqConnection(rabbitMqOptions);
// builder.Services.AddRabbitMqRegistration(rabbitMqOptions);
// builder.Services.AddEventBus();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.ConfigureEventBus();
app.UseHangfireDashboard();
app.ConfigureHangfireTasks();
app.UseExceptionHandler();
app.UseRouting();
app.UseHttpsRedirection();

app.UseEndpoints(ep => ep.MapControllers());

app.Run();
