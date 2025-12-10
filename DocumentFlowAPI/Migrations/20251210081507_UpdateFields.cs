using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentFlowAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statements");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "StatementTemplates",
                newName: "Path");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "ContractTemplates",
                newName: "Path");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Contracts",
                newName: "Path");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Contracts",
                type: "varchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "StatementTemplates",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "ContractTemplates",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Contracts",
                newName: "Content");

            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statements", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
