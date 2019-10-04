using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Фото будут раскиданы по папкам, по 100шт в каждой. В каждой папке по 10 папок(примерно)
// В отдельном файле будет отсортированная по координатам инфа каждой фотки, а именно: (координаты фото,имя, ID, расположение в бд)
// При увелечении карты на определенный % прога выведет все фотки которые входит в диапазон координат. 4 угла

namespace PhotoCompress
{
    class Program
    {
        static void Main(string[] args)
        {
            List<FileInfo> images = new List<FileInfo>(5);


            var rootDirectory = @"D:\New\";
            List<string> filesPath = Directory.GetFiles(rootDirectory).ToList();

            FileService.InitOfName(images, filesPath);

            FileService.ResizeImage(images);
            foreach (var image in images)
            {
                FileService.ResizeImage(image);
            }

            FileService.SearchImage("Россия", "Москва", "Река", "288485");
            
        }


    }
}
