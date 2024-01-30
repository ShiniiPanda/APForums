using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APForums.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsersTagsJoinedNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_profile_tags_profile_tags_ProfileTagsId",
                table: "users_profile_tags");

            migrationBuilder.DropForeignKey(
                name: "FK_users_profile_tags_users_UsersId",
                table: "users_profile_tags");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "users_profile_tags",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ProfileTagsId",
                table: "users_profile_tags",
                newName: "ProfileTagId");

            migrationBuilder.RenameIndex(
                name: "IX_users_profile_tags_UsersId",
                table: "users_profile_tags",
                newName: "IX_users_profile_tags_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "users_profile_tags",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "ProfileTagId",
                table: "users_profile_tags",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddForeignKey(
                name: "FK_users_profile_tags_profile_tags_ProfileTagId",
                table: "users_profile_tags",
                column: "ProfileTagId",
                principalTable: "profile_tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_profile_tags_users_UserId",
                table: "users_profile_tags",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_profile_tags_profile_tags_ProfileTagId",
                table: "users_profile_tags");

            migrationBuilder.DropForeignKey(
                name: "FK_users_profile_tags_users_UserId",
                table: "users_profile_tags");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "users_profile_tags",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "ProfileTagId",
                table: "users_profile_tags",
                newName: "ProfileTagsId");

            migrationBuilder.RenameIndex(
                name: "IX_users_profile_tags_UserId",
                table: "users_profile_tags",
                newName: "IX_users_profile_tags_UsersId");

            migrationBuilder.AlterColumn<int>(
                name: "UsersId",
                table: "users_profile_tags",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "ProfileTagsId",
                table: "users_profile_tags",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddForeignKey(
                name: "FK_users_profile_tags_profile_tags_ProfileTagsId",
                table: "users_profile_tags",
                column: "ProfileTagsId",
                principalTable: "profile_tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_profile_tags_users_UsersId",
                table: "users_profile_tags",
                column: "UsersId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
