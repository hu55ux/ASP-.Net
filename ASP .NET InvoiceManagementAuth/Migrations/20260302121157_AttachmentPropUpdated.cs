using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_.NET_InvoiceManagementAuth.Migrations
{
    /// <inheritdoc />
    public partial class AttachmentPropUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Köhnə cədvəli silirik (SQL xətasının qarşısını almaq üçün)
            migrationBuilder.DropTable(name: "Attachments");

            // 2. Cədvəli Guid ID ilə sıfırdan yaradırıq
            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    // SQL-in avtomatik Guid yaratması üçün NEWID() istifadə edirik
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoredFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    // Invoys cədvəli ilə əlaqə (Foreign Key)
                    table.ForeignKey(
                        name: "FK_Attachments_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Attachments",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
