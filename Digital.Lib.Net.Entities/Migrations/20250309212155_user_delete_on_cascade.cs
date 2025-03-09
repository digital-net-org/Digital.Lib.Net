using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Digital.Lib.Net.Entities.Migrations
{
    /// <inheritdoc />
    public partial class user_delete_on_cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_User_UploaderId",
                schema: "digital_core",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_User_UserId",
                schema: "digital_core",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Avatar_AvatarId",
                schema: "digital_core",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_User_UploaderId",
                schema: "digital_core",
                table: "Document",
                column: "UploaderId",
                principalSchema: "digital_core",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_User_UserId",
                schema: "digital_core",
                table: "Event",
                column: "UserId",
                principalSchema: "digital_core",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Avatar_AvatarId",
                schema: "digital_core",
                table: "User",
                column: "AvatarId",
                principalSchema: "digital_core",
                principalTable: "Avatar",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_User_UploaderId",
                schema: "digital_core",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_User_UserId",
                schema: "digital_core",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Avatar_AvatarId",
                schema: "digital_core",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_User_UploaderId",
                schema: "digital_core",
                table: "Document",
                column: "UploaderId",
                principalSchema: "digital_core",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_User_UserId",
                schema: "digital_core",
                table: "Event",
                column: "UserId",
                principalSchema: "digital_core",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Avatar_AvatarId",
                schema: "digital_core",
                table: "User",
                column: "AvatarId",
                principalSchema: "digital_core",
                principalTable: "Avatar",
                principalColumn: "Id");
        }
    }
}
