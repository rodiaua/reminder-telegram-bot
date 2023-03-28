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
                    TelegramChatKey = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TelegramChatId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramChats", x => x.TelegramChatKey);
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    ReminderKey = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TelegramChatKey = table.Column<long>(type: "bigint", nullable: false),
                    RemindTimeUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RemindTimeLocal = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ReminderTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReminderDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.ReminderKey);
                    table.ForeignKey(
                        name: "FK_Reminders_TelegramChats_TelegramChatKey",
                        column: x => x.TelegramChatKey,
                        principalTable: "TelegramChats",
                        principalColumn: "TelegramChatKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_TelegramChatKey",
                table: "Reminders",
                column: "TelegramChatKey");
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
