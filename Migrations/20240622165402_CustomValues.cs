using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstrellaAccesoriosWpf.Migrations
{
    /// <inheritdoc />
    public partial class CustomValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Provider provider = Provider.Create("SIN PROVEEDOR");
            Category category = Category.Create("SIN CATEGORIA");

            migrationBuilder.InsertData("Providers", columns: ["Id","Name"],[provider.Id,provider.Name]);
            migrationBuilder.InsertData("Categories", columns: ["Id","Description"], [category.Id,category.Description]);
            migrationBuilder.InsertData("SubCategories", columns: ["Id","Description","CategoryId"], [category.SubCategories[0].Id,category.SubCategories[0].Description, category.Id]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
