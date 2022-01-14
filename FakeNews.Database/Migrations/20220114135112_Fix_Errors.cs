using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeNews.Database.Migrations
{
    public partial class Fix_Errors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_AspNetUsers_AuthorId",
                table: "News");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "733853f4-1af7-466a-b28b-4360db0f456a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3ed1a906-15b3-4987-aa7d-2a17324b51cc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "4ed3f644-0001-4a11-879d-ae3ad17edcaf");

            migrationBuilder.AddForeignKey(
                name: "FK_News_AspNetUsers_AuthorId",
                table: "News",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_AspNetUsers_AuthorId",
                table: "News");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d04b9932-cad7-43e6-a67a-729c99f2a719");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "30ed41ca-53af-406e-97c5-296b600a8858");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "93211cd5-f152-4c57-803c-fa37b4cd2ce0");

            migrationBuilder.AddForeignKey(
                name: "FK_News_AspNetUsers_AuthorId",
                table: "News",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
