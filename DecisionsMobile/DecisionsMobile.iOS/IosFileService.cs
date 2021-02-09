using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DecisionsMobile.Services;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DecisionsMobile.iOS.IosFileService))]
namespace DecisionsMobile.iOS
{
    public class IosFileService : IFileService
    {
        public string SaveFile(string name, byte[] data)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "decisions", "cachedimage");
            Directory.CreateDirectory(documentsPath);

            string filePath = Path.Combine(documentsPath, name);

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                fs.Write(data, 0, data.Length);
            }
            return filePath;
        }
    }
}