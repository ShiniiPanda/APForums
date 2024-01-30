using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APForums.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddLogoFileClubsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "clubs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "clubs");
        }
    }
}
