using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Digital.Lib.Net.Entities.Migrations
{
    /// <inheritdoc />
    public partial class created_ApplicationOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationOption",
                schema: "digital_core",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Value = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationOption", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationOption_Id",
                schema: "digital_core",
                table: "ApplicationOption",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationOption",
                schema: "digital_core");
        }
    }
}
