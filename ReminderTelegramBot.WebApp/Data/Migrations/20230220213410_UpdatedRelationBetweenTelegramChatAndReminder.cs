using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReminderTelegramBot.WebApp.Data.Migrations
{
    public partial class UpdatedRelationBetweenTelegramChatAndReminder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_TelegramChats_ReminderKey",
                table: "Reminders");

            migrationBuilder.AlterColumn<long>(
                name: "ReminderKey",
                table: "Reminders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_TelegramChatKey",
                table: "Reminders",
                column: "TelegramChatKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_TelegramChats_TelegramChatKey",
                table: "Reminders",
                column: "TelegramChatKey",
                principalTable: "TelegramChats",
                principalColumn: "TelegramChatKey",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_TelegramChats_TelegramChatKey",
                table: "Reminders");

            migrationBuilder.DropIndex(
                name: "IX_Reminders_TelegramChatKey",
                table: "Reminders");

            migrationBuilder.AlterColumn<long>(
                name: "ReminderKey",
                table: "Reminders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_TelegramChats_ReminderKey",
                table: "Reminders",
                column: "ReminderKey",
                principalTable: "TelegramChats",
                principalColumn: "TelegramChatKey",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
