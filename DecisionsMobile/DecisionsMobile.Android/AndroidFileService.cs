using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DecisionsMobile.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(DecisionsMobile.Droid.AndroidFileService))]
namespace DecisionsMobile.Droid
{
    public class AndroidFileService : IFileService
    {
        public string SaveFile(string filename, byte[] data)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "decisions", "cachedimage");
            Directory.CreateDirectory(documentsPath);

            string filePath = Path.Combine(documentsPath, filename);

            
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {   
                fs.Write(data, 0, data.Length);
            }
            return filePath;
        }
    }
}