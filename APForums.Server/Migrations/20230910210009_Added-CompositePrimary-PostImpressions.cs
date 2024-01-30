using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APForums.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompositePrimaryPostImpressions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_post_impressions",
                table: "post_impressions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "post_impressions");

            migrationBuilder.AddPrimaryKey(
            name: "PK_post_impressions",
            table: "post_impressions",
            columns: new[] { "PostId", "UserId" });

            migrationBuilder.DropIndex(
            name: "IX_post_impressions_PostId",
            table: "post_impressions");


            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "post_impressions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "post_impressions",
                type: "nvarchar(30)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
            name: "IX_post_impressions_PostId",
            table: "post_impressions",
            column: "PostId");

            migrationBuilder.DropPrimaryKey(
                name: "PK_post_impressions",
                table: "post_impressions");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "post_impressions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_post_impressions",
                table: "post_impressions",
                column: "Id");
        }
    }
}
