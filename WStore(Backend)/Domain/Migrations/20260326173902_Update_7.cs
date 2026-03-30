using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Update_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Feedbacks_FeedbackId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Products_ProductEntityId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ProductEntityId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "FeedbackId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductEntityId",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "FeedbackId",
                table: "Feedbacks",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_FeedbackId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Products_ProductId",
                table: "Feedbacks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Products_ProductId",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Feedbacks",
                newName: "FeedbackId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_ProductId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_FeedbackId");

            migrationBuilder.AddColumn<Guid>(
                name: "FeedbackId",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductEntityId",
                table: "Feedbacks",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ProductEntityId",
                table: "Feedbacks",
                column: "ProductEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Feedbacks_FeedbackId",
                table: "Feedbacks",
                column: "FeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Products_ProductEntityId",
                table: "Feedbacks",
                column: "ProductEntityId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
