using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APForums.Server.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedClubsTableNullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "clubs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldDefaultValue: "SIG");

            migrationBuilder.UpdateData(
                table: "clubs",
                keyColumn: "Abbreviation",
                keyValue: null,
                column: "Abbreviation",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Abbreviation",
                table: "clubs",
                type: "nvarchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "clubs",
                type: "nvarchar(40)",
                nullable: false,
                defaultValue: "SIG",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Abbreviation",
                table: "clubs",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");
        }
    }
}
