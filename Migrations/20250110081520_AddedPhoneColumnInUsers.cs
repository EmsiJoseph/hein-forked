using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hein.Migrations
{
    /// <inheritdoc />
    public partial class AddedPhoneColumnInUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                schema: "identity",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                schema: "identity",
                table: "AspNetUsers");
        }
    }
}
