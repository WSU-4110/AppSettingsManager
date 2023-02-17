using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppSettingsManagerApi.Infrastructure.Migrations
{
    public partial class Settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .AlterColumn<string>(
                    name: "Password",
                    table: "BaseUsers",
                    type: "varchar(36)",
                    maxLength: 36,
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "longtext"
                )
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "Settings",
                    columns: table =>
                        new
                        {
                            Id = table
                                .Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            Version = table.Column<int>(type: "int", nullable: false),
                            Input = table
                                .Column<string>(type: "longtext", nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            IsCurrent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                            CreatedBy = table
                                .Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            LastUpdatedAt = table
                                .Column<DateTime>(
                                    type: "timestamp(6)",
                                    rowVersion: true,
                                    nullable: false
                                )
                                .Annotation(
                                    "MySql:ValueGenerationStrategy",
                                    MySqlValueGenerationStrategy.ComputedColumn
                                ),
                            CreatedAt = table.Column<DateTimeOffset>(
                                type: "datetime(6)",
                                nullable: false
                            ),
                            BaseUserUserId = table
                                .Column<string>(type: "varchar(36)", nullable: true)
                                .Annotation("MySql:CharSet", "utf8mb4")
                        },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Settings", x => new { x.Id, x.Version });
                        table.ForeignKey(
                            name: "FK_Settings_BaseUsers_BaseUserUserId",
                            column: x => x.BaseUserUserId,
                            principalTable: "BaseUsers",
                            principalColumn: "UserId"
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_BaseUserUserId",
                table: "Settings",
                column: "BaseUserUserId"
            );

            migrationBuilder.CreateIndex(name: "IX_Settings_Id", table: "Settings", column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_IsCurrent",
                table: "Settings",
                column: "IsCurrent"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Settings");

            migrationBuilder
                .AlterColumn<string>(
                    name: "Password",
                    table: "BaseUsers",
                    type: "longtext",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(36)",
                    oldMaxLength: 36
                )
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
