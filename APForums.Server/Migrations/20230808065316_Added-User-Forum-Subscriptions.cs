using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APForums.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserForumSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "forum_subscriptions",
                columns: table => new
                {
                    SubscribedForumsId = table.Column<int>(type: "int", nullable: false),
                    SubscribedUsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forum_subscriptions", x => new { x.SubscribedForumsId, x.SubscribedUsersId });
                    table.ForeignKey(
                        name: "FK_forum_subscriptions_forums_SubscribedForumsId",
                        column: x => x.SubscribedForumsId,
                        principalTable: "forums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_forum_subscriptions_users_SubscribedUsersId",
                        column: x => x.SubscribedUsersId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_forum_subscriptions_SubscribedUsersId",
                table: "forum_subscriptions",
                column: "SubscribedUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "forum_subscriptions");
        }
    }
}
