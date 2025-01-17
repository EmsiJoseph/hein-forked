using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hein.Migrations
{
    /// <inheritdoc />
    public partial class AddedOAuthProviderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTemporary",
                schema: "identity",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTemporary",
                schema: "identity",
                table: "AspNetUsers");
        }
    }
}
