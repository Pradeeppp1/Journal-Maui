using QuestPDF.Fluent;
using PdfColors = QuestPDF.Helpers.Colors;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Journal.Models;

namespace Journal.Services;

public interface IPdfService
{
    byte[] GenerateJournalPdf(List<JournalDisplayModel> entries, DateTime? startDate, DateTime? endDate);
}

public class PdfService : IPdfService
{
    public PdfService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GenerateJournalPdf(List<JournalDisplayModel> entries, DateTime? startDate, DateTime? endDate)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(PdfColors.White);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                page.Header()
                    .BorderBottom(1)
                    .BorderColor(PdfColors.Grey.Lighten2)
                    .PaddingBottom(10)
                    .Column(column =>
                    {
                        column.Item().Text("My Journal Collection")
                            .FontSize(24)
                            .Bold()
                            .FontColor(PdfColors.Blue.Darken2);

                        if (startDate.HasValue && endDate.HasValue)
                        {
                            column.Item().Text($"From: {startDate.Value:MMM dd, yyyy} • To: {endDate.Value:MMM dd, yyyy}")
                                .FontSize(12)
                                .FontColor(PdfColors.Grey.Darken1);
                        }

                        column.Item().Text($"Total Entries: {entries.Count}")
                            .FontSize(10)
                            .FontColor(PdfColors.Grey.Medium);
                    });

                page.Content()
                    .PaddingVertical(20)
                    .Column(column =>
                    {
                        foreach (var entry in entries)
                        {
                            column.Item().PaddingBottom(20).Column(entryColumn =>
                            {
                                entryColumn.Item().Row(row =>
                                {
                                    row.RelativeItem().Text($"{entry.CreatedAt:dddd, MMM dd, yyyy}")
                                        .FontSize(10)
                                        .Bold()
                                        .FontColor(PdfColors.Grey.Darken1);

                                    row.AutoItem().Background(PdfColors.Grey.Lighten3)
                                        .Padding(5)
                                        .Text($"{GetMoodEmoji(entry.Mood)} {entry.Mood}")
                                        .FontSize(9);
                                });

                                entryColumn.Item().PaddingTop(8).Text(entry.Title)
                                    .FontSize(16)
                                    .Bold()
                                    .FontColor(PdfColors.Blue.Darken3);

                                entryColumn.Item().PaddingTop(8).Text(StripHtml(entry.Content))
                                    .FontSize(11)
                                    .LineHeight(1.5f)
                                    .FontColor(PdfColors.Grey.Darken3);

                                if (!string.IsNullOrEmpty(entry.Tags))
                                {
                                    entryColumn.Item().PaddingTop(8).Row(row =>
                                    {
                                        foreach (var tag in entry.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                                        {
                                            row.AutoItem().PaddingRight(8).Text($"#{tag}")
                                                .FontSize(9)
                                                .FontColor(PdfColors.Blue.Medium);
                                        }
                                    });
                                }

                                entryColumn.Item().PaddingTop(15)
                                    .BorderBottom(1)
                                    .BorderColor(PdfColors.Grey.Lighten2);
                            });
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ").FontSize(9).FontColor(PdfColors.Grey.Medium);
                        x.CurrentPageNumber().FontSize(9).FontColor(PdfColors.Grey.Medium);
                        x.Span(" of ").FontSize(9).FontColor(PdfColors.Grey.Medium);
                        x.TotalPages().FontSize(9).FontColor(PdfColors.Grey.Medium);
                    });
            });
        });

        return document.GeneratePdf();
    }

    private static string GetMoodEmoji(string mood)
    {
        return mood switch
        {
            "Happy" => "😊",
            "Calm" => "😌",
            "Neutral" => "😐",
            "Reflective" => "😔",
            "Sad" => "😢",
            _ => "📝"
        };
    }

    private static string StripHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return string.Empty;
        var plainText = System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
        try
        {
            plainText = System.Net.WebUtility.HtmlDecode(plainText);
        }
        catch { /* ignore */ }
        return plainText;
    }
}
