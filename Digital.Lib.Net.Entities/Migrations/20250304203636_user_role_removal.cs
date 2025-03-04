using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Digital.Lib.Net.Entities.Migrations
{
    /// <inheritdoc />
    public partial class user_role_removal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                schema: "digital_core",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                schema: "digital_core",
                table: "User",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
