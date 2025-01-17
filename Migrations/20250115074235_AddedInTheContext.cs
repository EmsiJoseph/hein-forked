using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hein.Migrations
{
    /// <inheritdoc />
    public partial class AddedInTheContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "identity",
                table: "AspNetUserLogins",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "identity",
                table: "AspNetUserLogins");
        }
    }
}
