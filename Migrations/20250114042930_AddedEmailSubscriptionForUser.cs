using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hein.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmailSubscriptionForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailSubscription",
                schema: "identity",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailSubscription",
                schema: "identity",
                table: "AspNetUsers");
        }
    }
}
