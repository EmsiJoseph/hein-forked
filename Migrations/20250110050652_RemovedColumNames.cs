using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hein.Migrations
{
    /// <inheritdoc />
    public partial class RemovedColumNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "role",
                schema: "identity",
                table: "AspNetUsers",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "longitude",
                schema: "identity",
                table: "AspNetUsers",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "latitude",
                schema: "identity",
                table: "AspNetUsers",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "gender",
                schema: "identity",
                table: "AspNetUsers",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "dob",
                schema: "identity",
                table: "AspNetUsers",
                newName: "Dob");

            migrationBuilder.RenameColumn(
                name: "age",
                schema: "identity",
                table: "AspNetUsers",
                newName: "Age");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "identity",
                table: "AspNetUsers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "style_preference",
                schema: "identity",
                table: "AspNetUsers",
                newName: "StylePreference");

            migrationBuilder.RenameColumn(
                name: "last_name",
                schema: "identity",
                table: "AspNetUsers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "full_name",
                schema: "identity",
                table: "AspNetUsers",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "first_name",
                schema: "identity",
                table: "AspNetUsers",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "identity",
                table: "AspNetUsers",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                schema: "identity",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                schema: "identity",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StylePreference",
                schema: "identity",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                schema: "identity",
                table: "AspNetUsers",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                schema: "identity",
                table: "AspNetUsers",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                schema: "identity",
                table: "AspNetUsers",
                newName: "latitude");

            migrationBuilder.RenameColumn(
                name: "Gender",
                schema: "identity",
                table: "AspNetUsers",
                newName: "gender");

            migrationBuilder.RenameColumn(
                name: "Dob",
                schema: "identity",
                table: "AspNetUsers",
                newName: "dob");

            migrationBuilder.RenameColumn(
                name: "Age",
                schema: "identity",
                table: "AspNetUsers",
                newName: "age");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "identity",
                table: "AspNetUsers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StylePreference",
                schema: "identity",
                table: "AspNetUsers",
                newName: "style_preference");

            migrationBuilder.RenameColumn(
                name: "LastName",
                schema: "identity",
                table: "AspNetUsers",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "FullName",
                schema: "identity",
                table: "AspNetUsers",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                schema: "identity",
                table: "AspNetUsers",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "identity",
                table: "AspNetUsers",
                newName: "created_at");

            migrationBuilder.AlterColumn<decimal>(
                name: "longitude",
                schema: "identity",
                table: "AspNetUsers",
                type: "decimal(10,7)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "latitude",
                schema: "identity",
                table: "AspNetUsers",
                type: "decimal(10,7)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "style_preference",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
