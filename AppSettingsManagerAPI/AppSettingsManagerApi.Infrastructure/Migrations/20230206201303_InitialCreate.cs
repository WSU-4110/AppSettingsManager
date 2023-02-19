using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppSettingsManagerApi.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase().Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "BaseUsers",
                    columns: table =>
                        new
                        {
                            UserId = table
                                .Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            Password = table
                                .Column<string>(type: "longtext", nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4")
                            
                        },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_BaseUsers", x => x.UserId);
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "BaseUsers");
        }
    }
}
