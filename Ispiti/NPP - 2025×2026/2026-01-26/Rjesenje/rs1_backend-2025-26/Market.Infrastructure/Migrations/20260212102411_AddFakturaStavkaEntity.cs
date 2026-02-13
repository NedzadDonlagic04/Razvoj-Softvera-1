using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Market.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFakturaStavkaEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrojStavki",
                table: "Fakture");

            migrationBuilder.CreateTable(
                name: "FakturaStavke",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    FakturaId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FakturaStavke", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FakturaStavke_Fakture_FakturaId",
                        column: x => x.FakturaId,
                        principalTable: "Fakture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FakturaStavke_ProductCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FakturaStavke_CategoryId",
                table: "FakturaStavke",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FakturaStavke_FakturaId",
                table: "FakturaStavke",
                column: "FakturaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FakturaStavke");

            migrationBuilder.AddColumn<int>(
                name: "BrojStavki",
                table: "Fakture",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
