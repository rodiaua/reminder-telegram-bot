using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.OpenApi.Models;
using ReminderTelegramBot.WebApp.Data.Context;
using ReminderTelegramBot.WebApp.Data.Repository;
using ReminderTelegramBot.WebApp.RequestHandlers.AddReminderRequestHandler;
using ReminderTelegramBot.WebApp.RequestHandlers.AddTelegramChatRequestHandler;
using ReminderTelegramBot.WebApp.RequestHandlers.GetRemindersRequestHandler;
using ReminderTelegramBot.WebApp.RequestHandlers.RemoveRminderRequestHandler;
using ReminderTelegramBot.WebApp.RequestHandlers.UpdateReminderRequestHandler;
using ReminderTelegramBot.WebApp.Services;
using ReminderTelegramBot.WebApp.Utils;
using ReminderTelegramBot.WebApp.Utils.EventHandlers.ReminderTask;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region database registrations
builder.Services.AddDbContextFactory<ReminderDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReminderDb"));
});
builder.Services.AddDbContext<ReminderDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReminderDb"));
});
builder.Services.AddTransient<IReminderRepository, ReminderRepository>();
builder.Services.AddTransient<IReminderRepositoryFactory, ReminderRepositoryFactory>();
builder.Services.AddTransient<ITelegramChatRepository, TelegramChatRepository>();
#endregion

#region handlers registrations
builder.Services.AddTransient<AddReminderRequestHandler>();
builder.Services.AddTransient<AddTelegramChatRequestHandler>();
builder.Services.AddTransient<RemoveReminderRequestHandler>();
builder.Services.AddTransient<UpdateReminderRequestHandler>();
builder.Services.AddTransient<GetRemindersByChatIdRequestHandler>();
#endregion

#region services registrations
builder.Services.AddTransient<ReminderScheduler>();
builder.Services.AddTransient<ReminderService>();
#endregion

#region utils
builder.Services.AddTransient<ReminderTaskEventsHandler>();
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(configure =>
{
    configure.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
    configure.AddSimpleConsole(configure =>
    {
        configure.TimestampFormat = "[dd/MM/yyyy - HH:mm:ss]";
    });
});

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
