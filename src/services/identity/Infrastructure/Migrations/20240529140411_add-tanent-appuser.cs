using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastDelivery.Service.Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addtanentappuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "Identity",
                table: "Users",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Identity",
                table: "Users");
        }
    }
}
