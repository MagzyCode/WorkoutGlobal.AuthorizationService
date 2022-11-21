using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutGlobal.AuthorizationServiceApi.Migrations
{
    public partial class CascadeDeleteMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_AspNetUsers_UserCredentialsId",
                table: "UserAccounts");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f4d7080-beee-4a97-be65-2ffccde5eb72",
                column: "ConcurrencyStamp",
                value: "05540a5e-222f-4d45-aec8-54f16879223c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6abe6f33-ae4b-4430-8f14-493dc9a5a9d1",
                column: "ConcurrencyStamp",
                value: "bdd86f53-9711-4dc3-84fc-d87e459a586e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4a4ce79-c6b3-4e12-9c98-ff07b5030752",
                column: "ConcurrencyStamp",
                value: "b4087a72-c9f2-4217-9f33-8ebb86be8195");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b5b84fd7-5366-44eb-9d1b-408c6a4a8926",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "5b290c5e-ddb6-4700-978b-1570e90cf2c1", "a554454c-ea61-4219-9cdb-928798469a4f" });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: new Guid("07d1a783-adf7-4dcc-aa35-53abd353152d"),
                column: "DateOfRegistration",
                value: new DateTime(2022, 11, 18, 15, 28, 37, 974, DateTimeKind.Local).AddTicks(1804));

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_AspNetUsers_UserCredentialsId",
                table: "UserAccounts",
                column: "UserCredentialsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_AspNetUsers_UserCredentialsId",
                table: "UserAccounts");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f4d7080-beee-4a97-be65-2ffccde5eb72",
                column: "ConcurrencyStamp",
                value: "d2cd9e6b-aa79-4fcf-a4c4-46713831b6ee");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6abe6f33-ae4b-4430-8f14-493dc9a5a9d1",
                column: "ConcurrencyStamp",
                value: "ffa65d4f-241e-4cb7-b5aa-a3a7abf8dfc4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4a4ce79-c6b3-4e12-9c98-ff07b5030752",
                column: "ConcurrencyStamp",
                value: "8a2d201d-957b-4ef0-8810-01dba4d5360c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b5b84fd7-5366-44eb-9d1b-408c6a4a8926",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ce0a411d-76cf-4c02-bd3d-a7b45c58e7d4", "74f81be4-dc8d-4683-bba1-bebbffdc08d3" });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: new Guid("07d1a783-adf7-4dcc-aa35-53abd353152d"),
                column: "DateOfRegistration",
                value: new DateTime(2022, 11, 7, 12, 7, 36, 437, DateTimeKind.Local).AddTicks(8395));

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_AspNetUsers_UserCredentialsId",
                table: "UserAccounts",
                column: "UserCredentialsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
