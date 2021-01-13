using Microsoft.EntityFrameworkCore.Migrations;

namespace Lotus.Server.Data.Migrations
{
    public partial class Extend_IdentityUser_MobileNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MobileNumber",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "AspNetUsers");
        }
    }
}
