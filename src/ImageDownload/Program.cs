using ImageDownload;

Console.Write("Enter the total number of images to download: ");
int totalImages = int.Parse(Console.ReadLine());

Console.Write("Enter the maximum number of parallel downloads: ");
int maxParallelDownloads = int.Parse(Console.ReadLine());

Console.Write("Enter the save path (default: ./outputs): ");
string path = Console.ReadLine();

var downloader = new ImageDownloader(totalImages, path);
downloader.ProgressChanged += (current, total) =>
{
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write($"Progress: {current}/{total}");
};

await downloader.DownloadImagesAsync(maxParallelDownloads);
