## Image Downloader
This is a simple console application that allows you to download a specified number of images from the internet concurrently. You can control the total number of images to download, the maximum number of parallel downloads, and the save path for the downloaded images.

#### Usage
Run the application.
Enter the total number of images you want to download when prompted.
Enter the maximum number of parallel downloads when prompted.
Optionally, enter the save path for the downloaded images. If no path is provided, the default path "./outputs" will be used.
The application will start downloading the images and display the progress.
If you wish to cancel the download, press Ctrl+C. The application will attempt to clean up any downloaded images.
Once the download is completed or canceled, the corresponding message will be displayed.

#### Code Explanation
The main code is structured into two classes:

##### Program
The Program class contains the entry point of the application and handles user input. It prompts the user to enter the total number of images, maximum parallel downloads, and save path. Then, it creates an instance of the ImageDownloader class and starts the download process. It also subscribes to the ProgressChanged event to display the progress during the download.

##### ImageDownloader
The ImageDownloader class encapsulates the logic for downloading images. It takes the total number of images and the save path as parameters in its constructor. The DownloadImagesAsync method performs the actual download process. It uses a semaphore to control the number of parallel downloads and a cancellation token to handle cancellation requests.

The DownloadImagesAsync method creates download tasks for each image using the provided URL and save path. It also invokes the ProgressChanged event to update the progress.

The DownloadImageAsync method downloads an image from a given URL and saves it to the specified path. It uses WebClient to download the image bytes asynchronously and writes them to a file.

The CleanupDownloadedImages method deletes any downloaded images in case the download is canceled. It iterates through the completed images and attempts to delete them.

The CreateDirectoryIfNeeded method checks if the specified directory exists and creates it if it doesn't.