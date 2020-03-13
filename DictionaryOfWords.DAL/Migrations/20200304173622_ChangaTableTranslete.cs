using Microsoft.EntityFrameworkCore.Migrations;

namespace DictionaryOfWords.DAL.Migrations
{
    public partial class ChangaTableTranslete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WordTranslations_Languages_LanguageId",
                table: "WordTranslations");

            migrationBuilder.RenameColumn(
                name: "LanguageId",
                table: "WordTranslations",
                newName: "LanguageToId");

            migrationBuilder.RenameIndex(
                name: "IX_WordTranslations_LanguageId",
                table: "WordTranslations",
                newName: "IX_WordTranslations_LanguageToId");

            migrationBuilder.AddColumn<int>(
                name: "LanguageFromId",
                table: "WordTranslations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "LockoutEnabled",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "EmailConfirmed",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_WordTranslations_LanguageFromId",
                table: "WordTranslations",
                column: "LanguageFromId");

            migrationBuilder.AddForeignKey(
                name: "FK_WordTranslations_Languages_LanguageFromId",
                table: "WordTranslations",
                column: "LanguageFromId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WordTranslations_Languages_LanguageToId",
                table: "WordTranslations",
                column: "LanguageToId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WordTranslations_Languages_LanguageFromId",
                table: "WordTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_WordTranslations_Languages_LanguageToId",
                table: "WordTranslations");

            migrationBuilder.DropIndex(
                name: "IX_WordTranslations_LanguageFromId",
                table: "WordTranslations");

            migrationBuilder.DropColumn(
                name: "LanguageFromId",
                table: "WordTranslations");

            migrationBuilder.RenameColumn(
                name: "LanguageToId",
                table: "WordTranslations",
                newName: "LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_WordTranslations_LanguageToId",
                table: "WordTranslations",
                newName: "IX_WordTranslations_LanguageId");

            migrationBuilder.AlterColumn<int>(
                name: "LockoutEnabled",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<int>(
                name: "EmailConfirmed",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AddForeignKey(
                name: "FK_WordTranslations_Languages_LanguageId",
                table: "WordTranslations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
