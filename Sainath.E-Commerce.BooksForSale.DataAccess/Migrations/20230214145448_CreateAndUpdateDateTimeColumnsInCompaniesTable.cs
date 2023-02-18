using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sainath.ECommerce.BooksForSale.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreateAndUpdateDateTimeColumnsInCompaniesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Companies",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Companies");
        }
    }
}
