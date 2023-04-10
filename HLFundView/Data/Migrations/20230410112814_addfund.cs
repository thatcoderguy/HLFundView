using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HLFundView.Data.Migrations
{
    public partial class addfund : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fund",
                columns: table => new
                {
                    sedol = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    full_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fund_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    unit_type = table.Column<int>(type: "int", nullable: false),
                    running_yield = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    historic_yield = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    distribution_yield = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    underlying_yield = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    gross_yield = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    gross_running_yield = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    kiid_url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fund", x => x.sedol);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fund");
        }
    }
}
