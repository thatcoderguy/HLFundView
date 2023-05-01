using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HLFundView.Data.Migrations
{
    public partial class addedmarketdesignation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Market",
                table: "Dividends",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Market",
                table: "Dividends");
        }
    }
}
