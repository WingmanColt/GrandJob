using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HireMe.Services.Utilities
{
    public static class IFileNameExtension
    {
        public static string TempFileName(this IFormFile file, bool randomName = true)
        {
            var ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));

            var fileName = randomName ? Path.GetTempFileName() : file.FileName;

            var start = fileName.LastIndexOf('\\');

            if (start < 0)
                start = 0;

            var end = fileName.LastIndexOf('.');

            return $"{fileName.Substring(start, end - start)}{ext}".TrimStart('\\');
        }
    }
}
