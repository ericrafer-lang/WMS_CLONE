using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practice_for_wms.Migrations
{
    /// <inheritdoc />
    public partial class Removedcolumnstatusqualityexpiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchAddress",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BranchNumber",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BranchStatus",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchAddress",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BranchNumber",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BranchStatus",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
