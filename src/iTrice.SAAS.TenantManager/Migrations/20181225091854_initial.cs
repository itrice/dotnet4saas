using Microsoft.EntityFrameworkCore.Migrations;

namespace iTrice.SAAS.TenantManager.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataBase",
                table: "Tenant",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataBase",
                table: "Tenant");
        }
    }
}
