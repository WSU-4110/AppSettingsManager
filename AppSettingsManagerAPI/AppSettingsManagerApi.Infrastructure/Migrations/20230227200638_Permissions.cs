using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppSettingsManagerApi.Infrastructure.Migrations
{
    public partial class Permissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_BaseUsers_BaseUserUserId",
                table: "Settings");

            migrationBuilder.DropTable(
                name: "BaseUsers");

            migrationBuilder.RenameColumn(
                name: "BaseUserUserId",
                table: "Settings",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Settings",
                newName: "SettingGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Settings_Id",
                table: "Settings",
                newName: "IX_Settings_SettingGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Settings_BaseUserUserId",
                table: "Settings",
                newName: "IX_Settings_UserId");

            migrationBuilder.CreateTable(
                name: "SettingGroups",
                columns: table => new
                {
                    SettingId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingGroups", x => x.SettingId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SettingGroupId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CurrentPermissionLevel = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WaitingForApproval = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RequestedPermissionLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => new { x.UserId, x.SettingGroupId });
                    table.ForeignKey(
                        name: "FK_Permissions_SettingGroups_SettingGroupId",
                        column: x => x.SettingGroupId,
                        principalTable: "SettingGroups",
                        principalColumn: "SettingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_SettingGroupId",
                table: "Permissions",
                column: "SettingGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_SettingGroups_SettingGroupId",
                table: "Settings",
                column: "SettingGroupId",
                principalTable: "SettingGroups",
                principalColumn: "SettingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_Users_UserId",
                table: "Settings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_SettingGroups_SettingGroupId",
                table: "Settings");

            migrationBuilder.DropForeignKey(
                name: "FK_Settings_Users_UserId",
                table: "Settings");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "SettingGroups");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Settings",
                newName: "BaseUserUserId");

            migrationBuilder.RenameColumn(
                name: "SettingGroupId",
                table: "Settings",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Settings_UserId",
                table: "Settings",
                newName: "IX_Settings_BaseUserUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Settings_SettingGroupId",
                table: "Settings",
                newName: "IX_Settings_Id");

            migrationBuilder.CreateTable(
                name: "BaseUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUsers", x => x.UserId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_BaseUsers_BaseUserUserId",
                table: "Settings",
                column: "BaseUserUserId",
                principalTable: "BaseUsers",
                principalColumn: "UserId");
        }
    }
}
