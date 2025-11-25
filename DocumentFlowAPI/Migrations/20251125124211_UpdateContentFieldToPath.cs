using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentFlowAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContentFieldToPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "StatementTemplates",
                newName: "Path");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "ContractTemplates",
                newName: "Path");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "varchar(63)",
                maxLength: 63,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(63)",
                oldMaxLength: 63)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "StatementTemplates",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "ContractTemplates",
                newName: "Content");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: null,
                column: "Email",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "varchar(63)",
                maxLength: 63,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(63)",
                oldMaxLength: 63,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
