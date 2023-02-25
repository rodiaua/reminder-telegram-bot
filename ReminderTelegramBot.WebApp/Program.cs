using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.WebApp.Data.Context;
using ReminderTelegramBot.WebApp.Data.Repository;
using ReminderTelegramBot.WebApp.RequestHandlers.AddReminderRequestHandler;
using ReminderTelegramBot.WebApp.RequestHandlers.AddTelegramChatRequestHandler;
using ReminderTelegramBot.WebApp.RequestHandlers.RemoveRminderRequestHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region database registrations
builder.Services.AddDbContext<ReminderDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("ReminderDb"));
});
builder.Services.AddTransient<IReminderRepository, ReminderRepository>();
builder.Services.AddTransient<ITelegramChatRepository, TelegramChatRepository>();
#endregion

#region handlers registrations
builder.Services.AddTransient<AddReminderRequestHandler>();
builder.Services.AddTransient<AddTelegramChatRequestHandler>();
builder.Services.AddTransient<RemoveReminderRequestHandler>();
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
