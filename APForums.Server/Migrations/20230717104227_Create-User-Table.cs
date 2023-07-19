using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APForums.Server.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TpNumber = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "nvarchar(255)", nullable: false, computedColumnSql: "CONCAT(TpNumber, '@mail.apu.edu.my')", stored: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DegreeType = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Course = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Enrollment = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_users_TpNumber",
                table: "users",
                column: "TpNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
