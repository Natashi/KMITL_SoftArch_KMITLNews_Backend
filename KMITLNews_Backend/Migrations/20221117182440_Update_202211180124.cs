using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAPI.Migrations
{
    /// <inheritdoc />
    public partial class Update202211180124 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "DummyPrimary",
                table: "Tags_Posts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

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
        }
    }
}
