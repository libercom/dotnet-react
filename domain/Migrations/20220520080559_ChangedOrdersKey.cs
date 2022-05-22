using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace domain.Migrations
{
    public partial class ChangedOrdersKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_DestinationCountryId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SendingCountryId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DestinationCountryId",
                table: "Orders",
                column: "DestinationCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SendingCountryId",
                table: "Orders",
                column: "SendingCountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_DestinationCountryId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SendingCountryId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DestinationCountryId",
                table: "Orders",
                column: "DestinationCountryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SendingCountryId",
                table: "Orders",
                column: "SendingCountryId",
                unique: true);
        }
    }
}
