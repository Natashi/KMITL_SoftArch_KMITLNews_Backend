using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAPI.Migrations
{
    /// <inheritdoc />
    public partial class CompositeKey202211180216 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users_SharedPosts",
                table: "Users_SharedPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users_Follows",
                table: "Users_Follows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags_Posts",
                table: "Tags_Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags_Follows",
                table: "Tags_Follows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts_Users",
                table: "Posts_Users");

            migrationBuilder.DropColumn(
                name: "DummyPrimary",
                table: "Users_SharedPosts");

            migrationBuilder.DropColumn(
                name: "DummyPrimary",
                table: "Users_Follows");

            migrationBuilder.DropColumn(
                name: "DummyPrimary",
                table: "Tags_Posts");

            migrationBuilder.DropColumn(
                name: "DummyPrimary",
                table: "Tags_Follows");

            migrationBuilder.DropColumn(
                name: "DummyPrimary",
                table: "Posts_Users");

            migrationBuilder.AlterColumn<string>(
                name: "tag_name",
                table: "Tags_Posts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "tag_name",
                table: "Tags_Follows",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users_SharedPosts",
                table: "Users_SharedPosts",
                columns: new[] { "user_id", "shared_post_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users_Follows",
                table: "Users_Follows",
                columns: new[] { "user_id", "follower_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags_Posts",
                table: "Tags_Posts",
                columns: new[] { "tag_name", "post_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags_Follows",
                table: "Tags_Follows",
                columns: new[] { "tag_name", "follower_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts_Users",
                table: "Posts_Users",
                columns: new[] { "post_id", "user_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users_SharedPosts",
                table: "Users_SharedPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users_Follows",
                table: "Users_Follows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags_Posts",
                table: "Tags_Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags_Follows",
                table: "Tags_Follows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts_Users",
                table: "Posts_Users");

            migrationBuilder.AddColumn<int>(
                name: "DummyPrimary",
                table: "Users_SharedPosts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "DummyPrimary",
                table: "Users_Follows",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "tag_name",
                table: "Tags_Posts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "DummyPrimary",
                table: "Tags_Posts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "tag_name",
                table: "Tags_Follows",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "DummyPrimary",
                table: "Tags_Follows",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "DummyPrimary",
                table: "Posts_Users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users_SharedPosts",
                table: "Users_SharedPosts",
                column: "DummyPrimary");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users_Follows",
                table: "Users_Follows",
                column: "DummyPrimary");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags_Posts",
                table: "Tags_Posts",
                column: "DummyPrimary");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags_Follows",
                table: "Tags_Follows",
                column: "DummyPrimary");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts_Users",
                table: "Posts_Users",
                column: "DummyPrimary");
        }
    }
}
