using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReminderTelegramBot.WebAPI.Data.Migrations
{
    public partial class AddRepeatEveryDayPropToReminder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemindTimeUtc",
                table: "Reminders",
                newName: "ReminderTimeUtc");

            migrationBuilder.RenameColumn(
                name: "RemindTimeLocal",
                table: "Reminders",
                newName: "ReminderTimeLocal");

            migrationBuilder.AddColumn<bool>(
                name: "RepeatEveryDay",
                table: "Reminders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepeatEveryDay",
                table: "Reminders");

            migrationBuilder.RenameColumn(
                name: "ReminderTimeUtc",
                table: "Reminders",
                newName: "RemindTimeUtc");

            migrationBuilder.RenameColumn(
                name: "ReminderTimeLocal",
                table: "Reminders",
                newName: "RemindTimeLocal");
        }
    }
}
