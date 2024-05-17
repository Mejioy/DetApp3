using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DetApp3.Migrations
{
    public partial class gjgj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Employers");
        }
    }
}
