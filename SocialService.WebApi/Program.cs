using SocialService.Application.Services;
using SocialService.Core.Interfaces;
using SocialService.Core.Interfaces.Repositories;
using SocialService.Core.Interfaces.Services;
using SocialService.DataAccess;
using SocialService.DataAccess.Repository;
using SocialService.WebApi.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<SocialServiceContext>();
builder.Services.AddControllers();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseRouting();
app.UseHttpsRedirection();

app.UseEndpoints(ep => ep.MapControllers());

app.Run();
