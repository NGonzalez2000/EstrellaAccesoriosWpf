using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace EstrellaAccesoriosWpf.Models.Common;

public class ImageManager
{
    private readonly string _imageFolderPath;
    public ImageManager()
    {
        _imageFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EstrellaWpf", "Images");
    }
    public string GetImage(string newImageName)
    {
        string fullImageName = Path.Combine(_imageFolderPath, newImageName);
        OpenFileDialog openFileDialog = new()
        {
            Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string selectedFilePath = openFileDialog.FileName;
            using Bitmap resizedImage = ResizeImage(selectedFilePath, 250, 250, newImageName);
            
            resizedImage.Save(fullImageName, ImageFormat.Jpeg);
            return fullImageName;
        }
        return "";
    }
    public static void DeleteImage(string imageFullName)
    {
        if(File.Exists(imageFullName) && !imageFullName.EndsWith("default_product.jpeg"))
        {
            File.Delete(imageFullName);
        }
    }
    private static Bitmap ResizeImage(string imagePath, int width, int height, string outputImageName)
    {
        using Bitmap originalImage = new(imagePath);
        Bitmap resizedImage = new(width, height);
        using (Graphics graphics = Graphics.FromImage(resizedImage))
        {
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(originalImage, 0, 0, width, height);
        }
        return resizedImage;
    }
}
