using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentFlowAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateNewForeignKeyInTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StatementTemplates_CreatedBy",
                table: "StatementTemplates",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ContractTemplates_CreatedBy",
                table: "ContractTemplates",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractTemplates_Users_CreatedBy",
                table: "ContractTemplates",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StatementTemplates_Users_CreatedBy",
                table: "StatementTemplates",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractTemplates_Users_CreatedBy",
                table: "ContractTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_StatementTemplates_Users_CreatedBy",
                table: "StatementTemplates");

            migrationBuilder.DropIndex(
                name: "IX_StatementTemplates_CreatedBy",
                table: "StatementTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ContractTemplates_CreatedBy",
                table: "ContractTemplates");
        }
    }
}
