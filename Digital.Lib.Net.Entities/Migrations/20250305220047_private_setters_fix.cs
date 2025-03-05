using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Digital.Lib.Net.Entities.Migrations
{
    /// <inheritdoc />
    public partial class private_setters_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "digital_core",
                table: "Event",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "digital_core",
                table: "Event");
        }
    }
}
