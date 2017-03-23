using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Storage.Migrations
{
    public partial class userIdText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageUsers_AspNetUsers_UserId1",
                schema: "public",
                table: "ImageUsers");

            migrationBuilder.DropIndex(
                name: "IX_ImageUsers_UserId1",
                schema: "public",
                table: "ImageUsers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                schema: "public",
                table: "ImageUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "public",
                table: "ImageUsers",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ImageUsers_AspNetUsers_UserId",
                schema: "public",
                table: "ImageUsers",
                column: "UserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageUsers_AspNetUsers_UserId",
                schema: "public",
                table: "ImageUsers");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                schema: "public",
                table: "ImageUsers",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                schema: "public",
                table: "ImageUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageUsers_UserId1",
                schema: "public",
                table: "ImageUsers",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageUsers_AspNetUsers_UserId1",
                schema: "public",
                table: "ImageUsers",
                column: "UserId1",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
