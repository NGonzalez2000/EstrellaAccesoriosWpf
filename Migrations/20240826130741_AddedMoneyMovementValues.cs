using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstrellaAccesoriosWpf.Migrations
{
    /// <inheritdoc />
    public partial class AddedMoneyMovementValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MoneyMovementType incomeType = MoneyMovementType.Create("INGRESO");
            MoneyMovementType wasteType = MoneyMovementType.Create("RETIRO");
            MoneyMovementType sellType = MoneyMovementType.Create("VENTA");
            migrationBuilder.InsertData("MoneyMovementTypes", columns: ["Id", "Description"], [incomeType.Id, incomeType.Description]);
            migrationBuilder.InsertData("MoneyMovementTypes", columns: ["Id", "Description"], [wasteType.Id, wasteType.Description]);
            migrationBuilder.InsertData("MoneyMovementTypes", columns: ["Id", "Description"], [sellType.Id, sellType.Description]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
