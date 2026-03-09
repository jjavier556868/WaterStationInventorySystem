using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using InvSys.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace InvSys.App.Helpers
{
    public static class ReceiptPdfGenerator
    {
        public static string Generate(ReceiptData data)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "InvSys", "Receipts");
            Directory.CreateDirectory(folder);

            string fileName = $"Receipt_{data.PurchaseId}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string filePath = Path.Combine(folder, fileName);

            decimal vatAmount = data.TotalAmount - (data.TotalAmount / 1.12m);
            decimal vatableAmt = data.TotalAmount - vatAmount;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(226, 700, Unit.Point); // ~80mm thermal width
                    page.Margin(12, Unit.Point);
                    page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

                    page.Content().Column(col =>
                    {
                        // ── Header ────────────────────────────────────────
                        col.Item().AlignCenter().Text("InvSys Store").Bold().FontSize(14);
                        col.Item().AlignCenter().Text("Official Receipt").FontSize(8).FontColor(Colors.Grey.Darken2);
                        col.Item().AlignCenter().Text("Tel: (123) 456-7890").FontSize(7).FontColor(Colors.Grey.Darken1);
                        col.Item().PaddingVertical(4).LineHorizontal(0.5f);

                        // ── Meta ──────────────────────────────────────────
                        col.Item().Row(row => {
                            row.RelativeItem().Text("Receipt #:").Bold();
                            row.RelativeItem().AlignRight().Text($"{data.PurchaseId}");
                        });
                        col.Item().Row(row => {
                            row.RelativeItem().Text("Date:").Bold();
                            row.RelativeItem().AlignRight().Text(data.PurchasedOn.ToString("MM/dd/yyyy hh:mm tt"));
                        });
                        col.Item().Row(row => {
                            row.RelativeItem().Text("Cashier:").Bold();
                            row.RelativeItem().AlignRight().Text(data.CashierName);
                        });
                        col.Item().Row(row => {
                            row.RelativeItem().Text("Payment:").Bold();
                            row.RelativeItem().AlignRight().Text(data.PaymentMethod.ToString());
                        });
                        col.Item().PaddingVertical(4).LineHorizontal(0.5f);

                        // ── Items table ───────────────────────────────────
                        col.Item().Table(tbl =>
                        {
                            tbl.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(4);
                                c.RelativeColumn(2);
                                c.RelativeColumn(3);
                                c.RelativeColumn(3);
                            });

                            tbl.Cell().Text("ITEM").Bold();
                            tbl.Cell().AlignCenter().Text("QTY").Bold();
                            tbl.Cell().AlignRight().Text("PRICE").Bold();
                            tbl.Cell().AlignRight().Text("SUBTOTAL").Bold();

                            foreach (var item in data.Items)
                            {
                                tbl.Cell().Text(item.ProductName);
                                tbl.Cell().AlignCenter().Text(item.Quantity.ToString());
                                tbl.Cell().AlignRight().Text($"P{item.UnitPrice:N2}");
                                tbl.Cell().AlignRight().Text($"P{item.Subtotal:N2}");
                            }
                        });

                        col.Item().PaddingVertical(4).LineHorizontal(0.5f);

                        // ── Totals ────────────────────────────────────────
                        col.Item().Row(row => {
                            row.RelativeItem().Text("Vatable Sales:");
                            row.RelativeItem().AlignRight().Text($"P{vatableAmt:N2}");
                        });
                        col.Item().Row(row => {
                            row.RelativeItem().Text("VAT (12%):");
                            row.RelativeItem().AlignRight().Text($"P{vatAmount:N2}");
                        });
                        col.Item().Row(row => {
                            row.RelativeItem().Text("TOTAL:").Bold().FontSize(10);
                            row.RelativeItem().AlignRight().Text($"P{data.TotalAmount:N2}").Bold().FontSize(10);
                        });

                        if (data.PaymentMethod == PaymentMethod.Cash)
                        {
                            col.Item().Row(row => {
                                row.RelativeItem().Text("Amount Paid:");
                                row.RelativeItem().AlignRight().Text($"P{data.AmountPaid:N2}");
                            });
                            col.Item().Row(row => {
                                row.RelativeItem().Text("Change:");
                                row.RelativeItem().AlignRight().Text($"P{(data.AmountPaid - data.TotalAmount):N2}");
                            });
                        }

                        col.Item().PaddingVertical(4).LineHorizontal(0.5f);

                        // ── Footer ────────────────────────────────────────
                        col.Item().PaddingTop(6).AlignCenter().Text("Thank you for your purchase!").Bold();
                        col.Item().AlignCenter().Text("This serves as your official receipt.").FontSize(7).FontColor(Colors.Grey.Darken1);
                        col.Item().AlignCenter().Text("Powered by InvSys").FontSize(7).FontColor(Colors.Grey.Darken1);
                    });
                });
            })
            .GeneratePdf(filePath);

            return filePath;
        }
    }

    public class ReceiptData
    {
        public int PurchaseId { get; set; }
        public DateTime PurchasedOn { get; set; }
        public string CashierName { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public List<ReceiptLineItem> Items { get; set; } = new();
    }

    public class ReceiptLineItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}