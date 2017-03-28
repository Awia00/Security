using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Instaroot.Storage.Migrations
{
    public partial class cascadingdeletes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Images_ImageId",
                schema: "public",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                schema: "public",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Images_ImageId",
                schema: "public",
                table: "Comments",
                column: "ImageId",
                principalSchema: "public",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Images_ImageId",
                schema: "public",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                schema: "public",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Images_ImageId",
                schema: "public",
                table: "Comments",
                column: "ImageId",
                principalSchema: "public",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
