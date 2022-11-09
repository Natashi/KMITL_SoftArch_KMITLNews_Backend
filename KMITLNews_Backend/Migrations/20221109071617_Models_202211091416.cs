using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAPI.Migrations
{
    public partial class Models_202211091416 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advertisers",
                columns: table => new
                {
                    advertiser_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    advertiser_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ad_image_url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisers", x => x.advertiser_id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    post_text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    attached_image_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    verified = table.Column<bool>(type: "bit", nullable: false),
                    report_count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.post_id);
                });

            migrationBuilder.CreateTable(
                name: "Posts_Users",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Tags_Follows",
                columns: table => new
                {
                    tag_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    post_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Tags_Posts",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "int", nullable: false),
                    shared_post_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Users_Follows",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    follower_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Users_SharedPosts",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    shared_post_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertisers");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Posts_Users");

            migrationBuilder.DropTable(
                name: "Tags_Follows");

            migrationBuilder.DropTable(
                name: "Tags_Posts");

            migrationBuilder.DropTable(
                name: "Users_Follows");

            migrationBuilder.DropTable(
                name: "Users_SharedPosts");
        }
    }
}
