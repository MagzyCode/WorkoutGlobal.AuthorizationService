using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
#pragma warning disable 1591

namespace WorkoutGlobal.AuthorizationServiceApi.Migrations
{
    public partial class ReUpdatePatronymicPropMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f4d7080-beee-4a97-be65-2ffccde5eb72",
                column: "ConcurrencyStamp",
                value: "fc368376-5b5d-4fd4-8c87-5f46744d14bd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6abe6f33-ae4b-4430-8f14-493dc9a5a9d1",
                column: "ConcurrencyStamp",
                value: "257bb93d-e821-43fa-adcb-379c9fa2b51a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4a4ce79-c6b3-4e12-9c98-ff07b5030752",
                column: "ConcurrencyStamp",
                value: "22310fe2-1d45-484a-8ca2-a88397289cbf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b5b84fd7-5366-44eb-9d1b-408c6a4a8926",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6dbfb65c-358e-4f37-b1fe-a022a727a8b7", "ff824c15-11fc-4daa-9f21-e6c5b096d912" });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: new Guid("07d1a783-adf7-4dcc-aa35-53abd353152d"),
                column: "DateOfRegistration",
                value: new DateTime(2022, 7, 20, 16, 50, 6, 592, DateTimeKind.Local).AddTicks(9194));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f4d7080-beee-4a97-be65-2ffccde5eb72",
                column: "ConcurrencyStamp",
                value: "5a3c8eae-6c3a-4289-afd5-2a19ffd63ab6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6abe6f33-ae4b-4430-8f14-493dc9a5a9d1",
                column: "ConcurrencyStamp",
                value: "ae45354a-eab0-4e87-8c8c-00003b244089");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4a4ce79-c6b3-4e12-9c98-ff07b5030752",
                column: "ConcurrencyStamp",
                value: "18be11ed-2e73-4b66-b389-a1ae70fb4741");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b5b84fd7-5366-44eb-9d1b-408c6a4a8926",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "f5ea03f1-e793-40e1-8e3a-747f91a8e94d", "399de6a4-9768-4867-aa38-4ed281d5557a" });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: new Guid("07d1a783-adf7-4dcc-aa35-53abd353152d"),
                column: "DateOfRegistration",
                value: new DateTime(2022, 7, 20, 16, 46, 3, 287, DateTimeKind.Local).AddTicks(9155));
        }
    }
}
