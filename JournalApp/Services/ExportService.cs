using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using JournalApp.Models;
using Markdig;

namespace JournalApp.Services;

public class ExportService
{
    public async Task<byte[]> ExportToPdfAsync(List<JournalEntry> entries, DateTime startDate, DateTime endDate)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header()
                    .Text($"Journal Entries: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(col =>
                    {
                        foreach (var entry in entries)
                        {
                            col.Item().Element(container => RenderEntry(container, entry));
                            col.Item().PaddingTop(0.5f, Unit.Centimetre);
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        });

        return await Task.Run(() => document.GeneratePdf());
    }

    private void RenderEntry(IContainer container, JournalEntry entry)
    {
        container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(col =>
        {
            col.Item().Text(entry.Date.ToString("dddd, MMMM dd, yyyy"))
                .SemiBold().FontSize(14).FontColor(Colors.Blue.Darken2);

            if (!string.IsNullOrEmpty(entry.Title))
            {
                col.Item().PaddingTop(5).Text(entry.Title)
                    .SemiBold().FontSize(12);
            }

            col.Item().PaddingTop(5).Text($"Mood: {entry.PrimaryMood}")
                .FontSize(10).FontColor(Colors.Grey.Darken1);

            if (!string.IsNullOrEmpty(entry.SecondaryMood1) || !string.IsNullOrEmpty(entry.SecondaryMood2))
            {
                var secondaryMoods = new List<string>();
                if (!string.IsNullOrEmpty(entry.SecondaryMood1))
                    secondaryMoods.Add(entry.SecondaryMood1);
                if (!string.IsNullOrEmpty(entry.SecondaryMood2))
                    secondaryMoods.Add(entry.SecondaryMood2);

                col.Item().Text($"Secondary Moods: {string.Join(", ", secondaryMoods)}")
                    .FontSize(10).FontColor(Colors.Grey.Darken1);
            }

            if (!string.IsNullOrEmpty(entry.Category))
            {
                col.Item().Text($"Category: {entry.Category}")
                    .FontSize(10).FontColor(Colors.Grey.Darken1);
            }

            if (entry.Tags.Any())
            {
                col.Item().Text($"Tags: {string.Join(", ", entry.Tags.Select(t => t.Name))}")
                    .FontSize(10).FontColor(Colors.Grey.Darken1);
            }

            var htmlContent = Markdown.ToHtml(entry.Content);
            col.Item().PaddingTop(10).Text(StripHtml(entry.Content))
                .FontSize(11).LineHeight(1.5f);

            col.Item().PaddingTop(5).Text($"Word Count: {entry.WordCount}")
                .FontSize(9).FontColor(Colors.Grey.Medium);
        });
    }

    private string StripHtml(string content)
    {
        return System.Text.RegularExpressions.Regex.Replace(content, "<.*?>", string.Empty);
    }
}
