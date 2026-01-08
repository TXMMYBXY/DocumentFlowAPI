using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentFlowAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTokenFieldSizeInRefreshTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RefreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "varchar(127)",
                maxLength: 127,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(31)",
                oldMaxLength: 31,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RefreshTokens",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "varchar(31)",
                maxLength: 31,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(127)",
                oldMaxLength: 127,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
