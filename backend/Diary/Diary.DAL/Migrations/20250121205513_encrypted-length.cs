using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diary.DAL.Migrations
{
    /// <inheritdoc />
    public partial class encryptedlength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Records",
                type: "nvarchar(1100)",
                maxLength: 1100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Records",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1100)",
                oldMaxLength: 1100);
        }
    }
}
