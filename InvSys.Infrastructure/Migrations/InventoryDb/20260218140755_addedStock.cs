using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvSys.Infrastructure.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class addedStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Suppliers",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Sale",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Products",
                newName: "IsActive");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Sale",
                type: "DECIMAL(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL");

            migrationBuilder.AlterColumn<int>(
                name: "QuantitySold",
                table: "Sale",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL");

            migrationBuilder.AlterColumn<int>(
                name: "QuantityInStock",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "DECIMAL(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL");

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stock_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ProductId",
                table: "Stock",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Suppliers",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Sale",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Products",
                newName: "isActive");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Sale",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QuantitySold",
                table: "Sale",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "QuantityInStock",
                table: "Products",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)");
        }
    }
}
