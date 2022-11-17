using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAPI.Migrations
{
    public partial class Update_202211172300 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shared_post_id",
                table: "Tags_Posts");

            migrationBuilder.RenameColumn(
                name: "post_id",
                table: "Tags_Follows",
                newName: "follower_id");

            migrationBuilder.AddColumn<string>(
                name: "tag_name",
                table: "Tags_Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tag_name",
                table: "Tags_Posts");

            migrationBuilder.RenameColumn(
                name: "follower_id",
                table: "Tags_Follows",
                newName: "post_id");

            migrationBuilder.AddColumn<int>(
                name: "shared_post_id",
                table: "Tags_Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
