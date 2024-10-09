using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstrellaAccesoriosWpf.Migrations
{
    /// <inheritdoc />
    public partial class AddedCustomValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            CashClose cashClose = CashClose.Create(DateOnly.FromDateTime(DateTime.Now).AddDays(-13), 0m);
            migrationBuilder.InsertData("CashCloses", columns: ["Id", "Date", "Balance"], [cashClose.Id, cashClose.Date, cashClose.Balance]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
