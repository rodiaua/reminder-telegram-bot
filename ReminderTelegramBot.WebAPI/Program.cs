using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.Common;
using ReminderTelegramBot.WebAPI.Data.Context;
using ReminderTelegramBot.WebAPI.Data.Repository;
using ReminderTelegramBot.WebAPI.RequestHandlers.AddReminderRequestHandler;
using ReminderTelegramBot.WebAPI.RequestHandlers.AddTelegramChatRequestHandler;
using ReminderTelegramBot.WebAPI.RequestHandlers.GetRemindersRequestHandler;
using ReminderTelegramBot.WebAPI.RequestHandlers.RemoveRminderRequestHandler;
using ReminderTelegramBot.WebAPI.RequestHandlers.UpdateReminderRequestHandler;
using ReminderTelegramBot.WebAPI.Services;
using ReminderTelegramBot.WebAPI.Utils;
using ReminderTelegramBot.WebAPI.Utils.EventHandlers.ReminderTask;

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
builder.Services.AddSerilogLogging(builder.Configuration);

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
