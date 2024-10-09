using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstrellaAccesoriosWpf.Migrations
{
    /// <inheritdoc />
    public partial class AddedLogicToMoneyMovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PaymentMethodId",
                table: "MoneyMovements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoneyMovements_PaymentMethodId",
                table: "MoneyMovements",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoneyMovements_PaymentMethods_PaymentMethodId",
                table: "MoneyMovements",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoneyMovements_PaymentMethods_PaymentMethodId",
                table: "MoneyMovements");

            migrationBuilder.DropIndex(
                name: "IX_MoneyMovements_PaymentMethodId",
                table: "MoneyMovements");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "MoneyMovements");
        }
    }
}
