using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReminderTelegramBot.WebApp.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TelegramChats",
                columns: table => new
                {
                    TelegramChatKey = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TelegramChatId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramChats", x => x.TelegramChatKey);
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    ReminderKey = table.Column<long>(type: "INTEGER", nullable: false),
                    TelegramChatKey = table.Column<long>(type: "INTEGER", nullable: false),
                    RemindTimeUtc = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    RemindTimeLocal = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ReminderTitle = table.Column<string>(type: "TEXT", nullable: false),
                    ReminderDescription = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.ReminderKey);
                    table.ForeignKey(
                        name: "FK_Reminders_TelegramChats_ReminderKey",
                        column: x => x.ReminderKey,
                        principalTable: "TelegramChats",
                        principalColumn: "TelegramChatKey",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reminders");

            migrationBuilder.DropTable(
                name: "TelegramChats");
        }
    }
}
