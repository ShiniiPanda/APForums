using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APForums.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedForumClubRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "forums",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_forums_ClubId",
                table: "forums",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_forums_clubs_ClubId",
                table: "forums",
                column: "ClubId",
                principalTable: "clubs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_forums_clubs_ClubId",
                table: "forums");

            migrationBuilder.DropIndex(
                name: "IX_forums_ClubId",
                table: "forums");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "forums");
        }
    }
}
