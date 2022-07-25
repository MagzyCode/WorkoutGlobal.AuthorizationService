using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
#pragma warning disable 1591

namespace WorkoutGlobal.AuthorizationServiceApi.Migrations
{
    public partial class UpdatePatronymicPropMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccount_AspNetUsers_UserCredentialsId",
                table: "UserAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccount",
                table: "UserAccount");

            migrationBuilder.RenameTable(
                name: "UserAccount",
                newName: "UserAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccount_UserCredentialsId",
                table: "UserAccounts",
                newName: "IX_UserAccounts_UserCredentialsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_AspNetUsers_UserCredentialsId",
                table: "UserAccounts",
                column: "UserCredentialsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_AspNetUsers_UserCredentialsId",
                table: "UserAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts");

            migrationBuilder.RenameTable(
                name: "UserAccounts",
                newName: "UserAccount");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounts_UserCredentialsId",
                table: "UserAccount",
                newName: "IX_UserAccount_UserCredentialsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccount",
                table: "UserAccount",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f4d7080-beee-4a97-be65-2ffccde5eb72",
                column: "ConcurrencyStamp",
                value: "dfd93a76-281b-422c-9022-c61cb7016736");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6abe6f33-ae4b-4430-8f14-493dc9a5a9d1",
                column: "ConcurrencyStamp",
                value: "f3e48a9d-d445-48de-8caa-b427cb837359");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4a4ce79-c6b3-4e12-9c98-ff07b5030752",
                column: "ConcurrencyStamp",
                value: "713f4301-1e27-47d0-b793-7f647dd423ab");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b5b84fd7-5366-44eb-9d1b-408c6a4a8926",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d35b5f1d-646a-4202-8bff-45c8cbe50fb4", "fa93ce58-fa58-4874-8ed9-b7fd6eee80cb" });

            migrationBuilder.UpdateData(
                table: "UserAccount",
                keyColumn: "Id",
                keyValue: new Guid("07d1a783-adf7-4dcc-aa35-53abd353152d"),
                column: "DateOfRegistration",
                value: new DateTime(2022, 7, 20, 16, 19, 30, 678, DateTimeKind.Local).AddTicks(2761));

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccount_AspNetUsers_UserCredentialsId",
                table: "UserAccount",
                column: "UserCredentialsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
