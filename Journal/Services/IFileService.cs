namespace Journal.Services;

public interface IFileService
{
    Task<string> SaveHtmlFileAsync(string htmlContent, string fileName);
    Task<string?> SaveWithPickerAsync(string htmlContent, string fileName);
    Task<string> SavePdfFileAsync(byte[] pdfContent, string fileName);
}
