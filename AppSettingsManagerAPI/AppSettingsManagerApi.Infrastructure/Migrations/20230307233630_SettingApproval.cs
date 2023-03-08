using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppSettingsManagerApi.Infrastructure.Migrations
{
    public partial class SettingApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ApprovedAt",
                table: "Settings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "Settings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Settings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Settings");
        }
    }
}
