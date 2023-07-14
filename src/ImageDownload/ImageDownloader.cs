using System.Net;

namespace ImageDownload;

public class ImageDownloader
{
    private int count; // Total number of images to download
    private int completedCount; // Number of completed downloads
    private string path = "output"; // Directory where the images will be saved
    private CancellationTokenSource cancellationTokenSource;
    private SemaphoreSlim semaphore; // Semaphore for controlling parallel downloads

    public delegate void ProgressHandler(int current, int total); // Delegate for progress event
    public event ProgressHandler ProgressChanged;

    public ImageDownloader(int totalImages, string directoryName)
    {
        count = totalImages;
        completedCount = 0;
        if (!string.IsNullOrEmpty(directoryName))
            path = directoryName;

        CreateDirectoryIfNeeded(path);
    }

    public async Task DownloadImagesAsync(int maxParallelDownloads)
    {
        semaphore = new SemaphoreSlim(maxParallelDownloads);
        cancellationTokenSource = new CancellationTokenSource();
        var downloadTasks = new List<Task>();

        Console.CancelKeyPress += async (sender, e) =>
        {
            e.Cancel = true;
            cancellationTokenSource.Cancel();
            await Task.WhenAll(downloadTasks);
            Console.WriteLine();
            Console.WriteLine("Download canceled!");
            CleanupDownloadedImages(); // Clean up downloaded images when cancel key is pressed
        };

        Console.WriteLine("Downloading images..."); // Display "Downloading images..." message

        for (int i = 1; i <= count; i++)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
                break;

            await semaphore.WaitAsync(); // Wait on semaphore to control parallel downloads

            string imageUrl = $"https://picsum.photos/200/300?random={i}";
            string fileName = $"{i}.png";
            string savePath = Path.Combine(path, fileName);

            downloadTasks.Add(DownloadImageAsync(imageUrl, savePath));

            completedCount++;
            ProgressChanged?.Invoke(i, count); // Trigger progress event
        }

        await Task.WhenAll(downloadTasks);
    }

    private async Task DownloadImageAsync(string imageUrl, string savePath)
    {
        using var client = new WebClient();
        byte[] imageBytes = await client.DownloadDataTaskAsync(imageUrl);
        await File.WriteAllBytesAsync(savePath, imageBytes);
        semaphore.Release(); // Release semaphore when download is complete
    }

    public void CleanupDownloadedImages()
    {
        try
        {
            for (int i = 1; i <= completedCount; i++)
            {
                string temp = Path.Combine(path, $"{i}.png");
                if (File.Exists(temp))
                    File.Delete(temp);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to delete files: {ex.Message}");
        }
    }

    private void CreateDirectoryIfNeeded(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
}
