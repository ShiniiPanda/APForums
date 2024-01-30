using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APForums.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompositePrimaryCommentImpressions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_comment_impressions",
                table: "comment_impressions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "comment_impressions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_comment_impressions",
                table: "comment_impressions",
                columns: new[] { "CommentId", "UserId" });

            migrationBuilder.DropIndex(
                name: "IX_comment_impressions_CommentId",
                table: "comment_impressions");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "comment_impressions",
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
                table: "comment_impressions",
                type: "nvarchar(30)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_comment_impressions_CommentId",
                table: "comment_impressions",
                column: "CommentId");

            migrationBuilder.DropPrimaryKey(
                name: "PK_comment_impressions",
                table: "comment_impressions");


            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "comment_impressions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_comment_impressions",
                table: "comment_impressions",
                column: "Id");

        }
    }
}
