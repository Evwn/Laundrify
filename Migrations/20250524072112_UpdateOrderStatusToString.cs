using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laundrify.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderStatusToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, create a temporary table with the new schema
            migrationBuilder.CreateTable(
                name: "ef_temp_Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    DropOffDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PickUpDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Copy data with status conversion
            migrationBuilder.Sql(@"
                INSERT INTO ef_temp_Orders (Id, ClientId, ServiceId, Quantity, Status, DropOffDate, PickUpDate)
                SELECT 
                    Id, 
                    ClientId, 
                    ServiceId, 
                    Quantity,
                    CASE Status
                        WHEN 0 THEN 'Pending'
                        WHEN 1 THEN 'Completed'
                        WHEN 2 THEN 'Ready'
                        WHEN 3 THEN 'Postponed'
                        WHEN 4 THEN 'Canceled'
                        ELSE 'Pending'
                    END,
                    DropOffDate,
                    PickUpDate
                FROM Orders;
            ");

            // Drop the old table and rename the new one
            migrationBuilder.DropTable(name: "Orders");
            migrationBuilder.RenameTable(
                name: "ef_temp_Orders",
                newName: "Orders");

            // Recreate indexes
            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ServiceId",
                table: "Orders",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Create temporary table with old schema
            migrationBuilder.CreateTable(
                name: "ef_temp_Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DropOffDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PickUpDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Copy data with status conversion back to integers
            migrationBuilder.Sql(@"
                INSERT INTO ef_temp_Orders (Id, ClientId, ServiceId, Quantity, Status, DropOffDate, PickUpDate)
                SELECT 
                    Id, 
                    ClientId, 
                    ServiceId, 
                    Quantity,
                    CASE Status
                        WHEN 'Pending' THEN 0
                        WHEN 'Completed' THEN 1
                        WHEN 'Ready' THEN 2
                        WHEN 'Postponed' THEN 3
                        WHEN 'Canceled' THEN 4
                        ELSE 0
                    END,
                    DropOffDate,
                    PickUpDate
                FROM Orders;
            ");

            // Drop the new table and rename the old one back
            migrationBuilder.DropTable(name: "Orders");
            migrationBuilder.RenameTable(
                name: "ef_temp_Orders",
                newName: "Orders");

            // Recreate indexes
            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ServiceId",
                table: "Orders",
                column: "ServiceId");
        }
    }
}
