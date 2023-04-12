﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReminderTelegramBot.WebAPI.Data.Context;

#nullable disable

namespace ReminderTelegramBot.WebAPI.Data.Migrations
{
    [DbContext(typeof(ReminderDbContext))]
    partial class ReminderDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ReminderTelegramBot.WebApp.Data.Entities.Reminder", b =>
                {
                    b.Property<long>("ReminderKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ReminderKey"), 1L, 1);

                    b.Property<string>("ReminderDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ReminderTimeLocal")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("ReminderTimeUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ReminderTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RepeatEveryDay")
                        .HasColumnType("bit");

                    b.Property<long>("TelegramChatKey")
                        .HasColumnType("bigint");

                    b.HasKey("ReminderKey");

                    b.HasIndex("TelegramChatKey");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("ReminderTelegramBot.WebApp.Data.Entities.TelegramChat", b =>
                {
                    b.Property<long>("TelegramChatKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("TelegramChatKey"), 1L, 1);

                    b.Property<long>("TelegramChatId")
                        .HasColumnType("bigint");

                    b.HasKey("TelegramChatKey");

                    b.ToTable("TelegramChats");
                });

            modelBuilder.Entity("ReminderTelegramBot.WebApp.Data.Entities.Reminder", b =>
                {
                    b.HasOne("ReminderTelegramBot.WebApp.Data.Entities.TelegramChat", "TelegramChat")
                        .WithMany("Reminders")
                        .HasForeignKey("TelegramChatKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TelegramChat");
                });

            modelBuilder.Entity("ReminderTelegramBot.WebApp.Data.Entities.TelegramChat", b =>
                {
                    b.Navigation("Reminders");
                });
#pragma warning restore 612, 618
        }
    }
}