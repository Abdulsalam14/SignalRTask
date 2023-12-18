using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiSignalR.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[] { 1, "Mercedes", 5000.0 });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[] { 2, "Chevrolet", 4000.0 });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[] { 3, "Maseratti", 7000.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
