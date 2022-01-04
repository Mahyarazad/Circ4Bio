using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using _0_Framework.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ServiceHost
{

    public class FileUploader : IFileUploader
    {
        private void WriteFile(IFormFile file, string path)
        {
            using (var writer = System.IO.File.Create(path))
            {
                file.CopyTo(writer);
            }
        }
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploader(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string Uploader(IFormFile file, string folder, string fileName)
        {
            string[] files;
            string outputFileName = null;
            if (file == null) return null;
            var path = $"{_webHostEnvironment.WebRootPath}\\Images\\{folder}\\";
            var fileExtension = Path.GetExtension(file.FileName);

            if (Directory.Exists(path))
            {
                int checkInt;
                files = Directory.GetFiles(path);
                var targetFile = $"{path}{fileName}{fileExtension}";

                if (files.Contains(targetFile))
                {
                    targetFile = files[files.Length - 1];
                    outputFileName = Path.GetFileName(targetFile);
                    var checkName = Regex.Match(Path.GetFileName(targetFile), @"\d+");
                    if (checkName.Value == "")
                    {
                        outputFileName = $"{fileName}01";
                        WriteFile(file, $"{path}{outputFileName}{fileExtension}");

                    }
                    try
                    {
                        checkInt = int.Parse(checkName.Value);
                        if (checkInt != -1)
                        {
                            if (checkInt < 9)
                            {
                                outputFileName = $"{outputFileName.Substring(0, outputFileName.Length - 6)}0{checkInt + 1}";
                            }
                            else
                            {
                                outputFileName = $"{outputFileName.Substring(0, outputFileName.Length - 6)}{checkInt + 1}";
                            }
                            WriteFile(file, $"{path}{outputFileName}{fileExtension}");
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"Unable to parse");
                    }
                }

            }
            Directory.CreateDirectory(path);
            var filePath = $"{path}{fileName}{fileExtension}";
            WriteFile(file, $"{path}{fileName}{fileExtension}");
            outputFileName = fileName;
            return $"{outputFileName}{fileExtension}";
        }
    }

}