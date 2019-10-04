using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace PhotoCompress
{
    public static class FileService
    {
        public static string rootDirectory { get; set; } = @"C:\New";

        public static FileInfo InitFileName(FileInfo fileInfo)
        {
            fileInfo.FileName = $"[{fileInfo.Country}][{fileInfo.City}][{fileInfo.Tag}][{fileInfo.Coordinates}][{Guid.NewGuid()}]";
            return fileInfo;
        }

        public static void ResizeImage(FileInfo fileInfo)
        {
            var isCorrect = false;
            if (SizeCorrection(fileInfo)) isCorrect = true;
            ISupportedImageFormat format = new JpegFormat { Quality = 70 };
            using (MemoryStream inStream = new MemoryStream(fileInfo.PhotoBytes))
            {
                using (FileStream outStream = File.Create($"{DirectoryFind(fileInfo)}{fileInfo.FileName}.{format.DefaultExtension}"))
                {
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        if (isCorrect)
                        {
                            imageFactory.Load(inStream).Format(format).Save(outStream);
                        }
                        else
                        {
                            Size size = imageFactory.Load(inStream).Image.Size / 2;
                            imageFactory.Load(inStream).Resize(size).Format(format).Save(outStream);
                        }
                    }
                }
            }
        }

        public static void ResizeImage(List<FileInfo> files)
        {
            foreach (var fileInfo in files)
            {
                ResizeImage(fileInfo);
            }
        }

        public static void SearchImage(string country, string city, string tag, string coordinates)
        {
            var path = DirectoryFind(new FileInfo() { Country = country, City = city, Tag = tag, Coordinates = coordinates }, false);
            if (path != null)
            {
                var images = Directory.GetFiles(path);

                Console.WriteLine("Найденные фотографии:");
                foreach (var item in images)
                {
                    Console.WriteLine(item);
                }
            }
            else
                Console.WriteLine("Фотографии не найдены!");

        }

        private static bool SizeCorrection(FileInfo fileInfo)
        {
            var desiredValue = 3;
            var sizeInMB = fileInfo.PhotoBytes.Length / 1000 / 1024;
            return sizeInMB < desiredValue;
        }

        private static string DirectoryFind(FileInfo fileInfo, bool check = true)
        {
            // Переменная check выполняет функционал
            // обычного наращивания пути без создания директорий.

            var path = rootDirectory;
            if (!Directory.Exists($@"{path}\{fileInfo.Country}") && check)
            {
                Directory.CreateDirectory($@"{rootDirectory}\{fileInfo.Country}");
            }
            path += $@"\{fileInfo.Country}";
            if (!Directory.Exists($@"{path}\{fileInfo.City}") && check)
            {
                Directory.CreateDirectory($@"{path}\{fileInfo.City}");
            }
            path += $@"\{fileInfo.City}";
            if (!Directory.Exists($@"{path}\{fileInfo.Tag}") && check)
            {
                Directory.CreateDirectory($@"{path}\{fileInfo.Tag}");
            }
            path += $@"\{fileInfo.Tag}";
            if (!Directory.Exists($@"{path}\{fileInfo.Coordinates}") && check)
            {
                Directory.CreateDirectory($@"{path}\{fileInfo.Coordinates}");
            }
            path += $@"\{fileInfo.Coordinates}\";
            if (!Directory.Exists(path) && !check) return null;
            return path;
        }

        public static void InitOfName(List<FileInfo> images, List<string> filesPath)
        {
            List<string> filesName = new List<string>();
            foreach (var file in filesPath)
            {
                filesName.Add(Path.GetFileNameWithoutExtension(file));

                var partsName = filesName.Last().Split("_");
                images.Add(new FileInfo(file)
                {
                    Country = partsName[0],
                    City = partsName[1],
                    Tag = partsName[2],
                    Coordinates = partsName[3]
                });
                FileService.InitFileName(images.Last());
            }
        }
    }
}
