using Microsoft.EntityFrameworkCore.Migrations;

namespace iTrice.SAAS.TenantManager.Migrations
{
    public partial class tentupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "Tenant",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "URL",
                table: "Tenant");
        }
    }
}
