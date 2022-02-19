using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShop.Migrations
{
    public partial class editedfieldnamebooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalePice",
                table: "Books",
                newName: "SalePrice");

            migrationBuilder.RenameColumn(
                name: "CostPice",
                table: "Books",
                newName: "CostPrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "Books",
                newName: "SalePice");

            migrationBuilder.RenameColumn(
                name: "CostPrice",
                table: "Books",
                newName: "CostPice");
        }
    }
}
