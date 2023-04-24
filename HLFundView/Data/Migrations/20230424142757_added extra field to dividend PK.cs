using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HLFundView.Data.Migrations
{
    public partial class addedextrafieldtodividendPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "DividendAmount",
                table: "Dividends",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float")
                .Annotation("Relational:ColumnOrder", 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "DividendAmount",
                table: "Dividends",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float")
                .OldAnnotation("Relational:ColumnOrder", 2);
        }
    }
}
