namespace Journal.Services;

public class FileService : IFileService
{
    public async Task<string> SaveHtmlFileAsync(string htmlContent, string fileName)
    {
        try
        {
            // Try multiple locations in order of preference
            var possiblePaths = new[]
            {
                "/Users/pradeepbaral/Documents/AD",
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileSystem.AppDataDirectory,
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Path.GetTempPath()
            };

            string? successfulPath = null;
            Exception? lastException = null;

            foreach (var basePath in possiblePaths)
            {
                try
                {
                    if (string.IsNullOrEmpty(basePath))
                        continue;

                    var filePath = Path.Combine(basePath, fileName);
                    
                    // Create a complete HTML document with styling
                    var fullHtml = $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Journal Export</title>
    <style>
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto', 'Oxygen', 'Ubuntu', 'Cantarell', sans-serif;
            line-height: 1.6;
            max-width: 800px;
            margin: 40px auto;
            padding: 20px;
            background: #f8f9fa;
        }}
        @media print {{
            body {{ background: white; margin: 0; padding: 20px; }}
        }}
    </style>
</head>
<body>
    {htmlContent}
</body>
</html>";

                    // Write the HTML content to file
                    await File.WriteAllTextAsync(filePath, fullHtml);
                    
                    // Verify the file was created
                    if (File.Exists(filePath))
                    {
                        successfulPath = filePath;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    continue;
                }
            }

            if (successfulPath != null)
            {
                return successfulPath;
            }

            throw new Exception($"Failed to save file to any location. Last error: {lastException?.Message ?? "Unknown error"}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save file: {ex.Message}", ex);
        }
    }

    public async Task<string?> SaveWithPickerAsync(string htmlContent, string fileName)
    {
        try
        {
            // For now, just use the automatic save
            // MAUI FileSaver requires additional platform-specific setup
            return await SaveHtmlFileAsync(htmlContent, fileName);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save file with picker: {ex.Message}", ex);
        }
    }

    public async Task<string> SavePdfFileAsync(byte[] pdfContent, string fileName)
    {
        try
        {
            // Try multiple locations in order of preference
            var possiblePaths = new[]
            {
                "/Users/pradeepbaral/Documents/AD",
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileSystem.AppDataDirectory,
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Path.GetTempPath()
            };

            string? successfulPath = null;
            Exception? lastException = null;

            foreach (var basePath in possiblePaths)
            {
                try
                {
                    if (string.IsNullOrEmpty(basePath))
                        continue;

                    var filePath = Path.Combine(basePath, fileName);
                    
                    // Write the PDF content to file
                    await File.WriteAllBytesAsync(filePath, pdfContent);
                    
                    // Verify the file was created
                    if (File.Exists(filePath))
                    {
                        successfulPath = filePath;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    continue;
                }
            }

            if (successfulPath != null)
            {
                return successfulPath;
            }

            throw new Exception($"Failed to save PDF to any location. Last error: {lastException?.Message ?? "Unknown error"}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save PDF file: {ex.Message}", ex);
        }
    }
}
