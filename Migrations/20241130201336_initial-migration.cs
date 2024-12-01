using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderPickingSystem.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reaches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InitialLocationId = table.Column<int>(type: "int", nullable: false),
                    DestinationLocationId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelocateItemRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    InitialLocationId = table.Column<int>(type: "int", nullable: false),
                    DestinationLocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelocateItemRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Relocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaletteId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isle = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Letter = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: false),
                    PickingOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentUserId = table.Column<int>(type: "int", nullable: false),
                    RequiredRole = table.Column<int>(type: "int", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OngoingPaletteId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OngoingPickId = table.Column<int>(type: "int", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_RelocateItemRequest_RequestId",
                        column: x => x.RequestId,
                        principalTable: "RelocateItemRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Palettes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OngoingContainerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PickingOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Palettes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Palettes_Containers_OngoingContainerId",
                        column: x => x.OngoingContainerId,
                        principalTable: "Containers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Palettes_Orders_PickingOrderId",
                        column: x => x.PickingOrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PickRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PickingOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickRequest_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PickRequest_Orders_PickingOrderId",
                        column: x => x.PickingOrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Picks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PaletteId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContainerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PickingOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Picks_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Picks_Orders_PickingOrderId",
                        column: x => x.PickingOrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Orders_CurrentOrderId",
                        column: x => x.CurrentOrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRoleMappings",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleMappings", x => new { x.UserId, x.Role });
                    table.ForeignKey(
                        name: "FK_UserRoleMappings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Containers_PaletteId",
                table: "Containers",
                column: "PaletteId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_LocationId",
                table: "Items",
                column: "LocationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PickingOrderId",
                table: "Locations",
                column: "PickingOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OngoingPaletteId",
                table: "Orders",
                column: "OngoingPaletteId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OngoingPickId",
                table: "Orders",
                column: "OngoingPickId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RequestId",
                table: "Orders",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Palettes_OngoingContainerId",
                table: "Palettes",
                column: "OngoingContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Palettes_PickingOrderId",
                table: "Palettes",
                column: "PickingOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PickRequest_OrderId",
                table: "PickRequest",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PickRequest_PickingOrderId",
                table: "PickRequest",
                column: "PickingOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_ContainerId",
                table: "Picks",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_PickingOrderId",
                table: "Picks",
                column: "PickingOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentOrderId",
                table: "Users",
                column: "CurrentOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Palettes_PaletteId",
                table: "Containers",
                column: "PaletteId",
                principalTable: "Palettes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Locations_LocationId",
                table: "Items",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Orders_PickingOrderId",
                table: "Locations",
                column: "PickingOrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Palettes_OngoingPaletteId",
                table: "Orders",
                column: "OngoingPaletteId",
                principalTable: "Palettes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Picks_OngoingPickId",
                table: "Orders",
                column: "OngoingPickId",
                principalTable: "Picks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Palettes_PaletteId",
                table: "Containers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Palettes_OngoingPaletteId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Picks_Orders_PickingOrderId",
                table: "Picks");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "PickRequest");

            migrationBuilder.DropTable(
                name: "Reaches");

            migrationBuilder.DropTable(
                name: "Relocations");

            migrationBuilder.DropTable(
                name: "UserRoleMappings");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Palettes");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Picks");

            migrationBuilder.DropTable(
                name: "RelocateItemRequest");

            migrationBuilder.DropTable(
                name: "Containers");
        }
    }
}
