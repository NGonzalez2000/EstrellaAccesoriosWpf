using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstrellaAccesoriosWpf.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSells : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalEarned",
                table: "Sells",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalEarned",
                table: "Sells");
        }
    }
}
