using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HLFundView.Data.Migrations
{
    public partial class addedextrafieldtodividendPK2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Dividends",
                table: "Dividends");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dividends",
                table: "Dividends",
                columns: new[] { "Symbol", "DividendExDate", "DividendAmount" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Dividends",
                table: "Dividends");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dividends",
                table: "Dividends",
                columns: new[] { "Symbol", "DividendExDate" });
        }
    }
}
