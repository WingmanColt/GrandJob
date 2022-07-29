using HireMe.Data.Repository.Interfaces;
using HireMe.Services.Interfaces;
using System;
using System.Linq;
using HireMe.Entities.Enums;
using HireMe.Core.Helpers;
using HireMe.Entities.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Collections.Generic;
using HireMe.Services.Utilities;
using System.Drawing;
using LazZiya.ImageResize;

namespace HireMe.Services
{
    public class BaseService : IBaseService
    {
        private readonly IRepository<Jobs> _jobsRepository;
        private readonly IRepository<Contestant> _contestantRepository;
        private readonly IRepository<Company> _companiesRepository;

        private readonly ILogService _logService;
        private readonly IToastNotification _toastNotification;
        private readonly IFilesService _filesService;

        private readonly string[] _permittedFiles = { ".pdf" };
        private readonly string _AppliedCVPath;
        private readonly string _myFilesCVPath;

        private readonly int _FileSizeLimit;
        private readonly int _FilesUploadLimit;
        

        private readonly string _ImageUsersPath;
        private readonly string _ImageCompaniesPath;
        private readonly string _GalleryPath;

        private readonly string[] _permittedImages = { ".png", ".jpg", ".gif", ".jpeg", ".JPG", ".JPEG", ".PNG", ".GIF" };
        public BaseService(
              IConfiguration config,
              IRepository<Jobs> jobsRepository,
              IRepository<Contestant> contestantRepository,
              IRepository<Company> companiesRepository,
              IToastNotification toastNotification,
              ILogService logService,
              IFilesService filesService)
        {
            _jobsRepository = jobsRepository;
            _contestantRepository = contestantRepository;
            _companiesRepository = companiesRepository;

            _toastNotification = toastNotification;
            _filesService = filesService;
            _logService = logService;

            _AppliedCVPath = config.GetValue<string>("CVPaths:AppliedCVPath");
            _myFilesCVPath = config.GetValue<string>("CVPaths:myFilesCVPath");

            _FileSizeLimit = config.GetValue<int>("CVPaths:FileSizeLimit");
            _FilesUploadLimit = config.GetValue<int>("CVPaths:FileSizeLimit");

            _ImageUsersPath = config.GetValue<string>("MySettings:ImageUsersPathHosting");
            _ImageCompaniesPath = config.GetValue<string>("MySettings:ImageCompaniesPathHosting");
            _GalleryPath = config.GetValue<string>("MySettings:GalleryPathHosting");

        }

        public async Task ToastNotifyLogAsync(User user, ToastMessageState state, string title, string message, string errorpage, int duration)
        {
            switch (state)
            {
                case ToastMessageState.Success:
                    ToastNotify(ToastMessageState.Success, title, message, duration);
                    break;
                case ToastMessageState.Info:
                    {
                        ToastNotify(ToastMessageState.Info, title, message, duration);
                        await _logService.Create(message, errorpage, LogLevel.Info, user?.UserName).ConfigureAwait(false);
                    }
                    break;
                case ToastMessageState.Alert:
                    ToastNotify(ToastMessageState.Alert, title, message, duration);
                    break;
                case ToastMessageState.Warning:
                    {
                        ToastNotify(ToastMessageState.Warning, title, message, duration);
                        await _logService.Create(message, errorpage, LogLevel.Warning, user?.UserName).ConfigureAwait(false);
                    }
                    break;
                case ToastMessageState.Error:
                    {
                        ToastNotify(ToastMessageState.Error, title, message, duration);
                        await _logService.Create(message, errorpage, LogLevel.Danger, user?.UserName).ConfigureAwait(false);
                    }
                    break;
            }
        }
        public void ToastNotify(ToastMessageState state, string title, string message, int duration)
        {
            switch (state)
            {
                case ToastMessageState.Success:
                    _toastNotification.AddSuccessToastMessage(message, new ToastrOptions() { Title = title, TimeOut = duration });
                    break;
                case ToastMessageState.Info:
                    _toastNotification.AddInfoToastMessage(message, new ToastrOptions() { Title = title, TimeOut = duration });
                    break;
                case ToastMessageState.Alert:
                    _toastNotification.AddAlertToastMessage(message, new ToastrOptions() { Title = title, TimeOut = duration });
                    break;
                case ToastMessageState.Warning:
                    _toastNotification.AddWarningToastMessage(message, new ToastrOptions() { Title = title, TimeOut = duration });
                    break;
                case ToastMessageState.Error:
                    _toastNotification.AddErrorToastMessage(message, new ToastrOptions() { Title = title, TimeOut = duration });
                    break;
            }
        }

        public async Task<OperationResult> Approve(int Id, PostType postType, ApproveType type)
        {
            switch (postType)
            {
                case PostType.Company:
                    {

                        var item = await _companiesRepository.Set().FirstOrDefaultAsync(j => j.Id == Id);
                        item.isApproved = type;

                        var success = await _companiesRepository.SaveChangesAsync();
                        return success;
                    }
                case PostType.Contestant:
                    {

                        var item = await _contestantRepository.Set().FirstOrDefaultAsync(j => j.Id == Id);
                        item.isApproved = type;

                        var success = await _contestantRepository.SaveChangesAsync();
                        return success;
                    }

                case PostType.Job:
                    {

                        var item = await _jobsRepository.Set().FirstOrDefaultAsync(j => j.Id == Id);
                        item.isApproved = type;

                        var success = await _jobsRepository.SaveChangesAsync();
                        return success;
                    }
            }
            return null;
        }

        //  [ValidateAntiForgeryToken]
        public async Task<string> MultipleUploadFileAsync(string folderName, int height, int width, List<IFormFile> files)
        {
            string folderClearedName = Path.Combine(_GalleryPath, StringHelper.Filter(folderName));
            if (!Directory.Exists(_GalleryPath))
            {
                Directory.CreateDirectory(_GalleryPath);
            }
            if (!Directory.Exists(folderClearedName))
            {
                Directory.CreateDirectory(folderClearedName);
            }

            List<string> filesName = new List<string>();

            int counter = 0;//files.Count();

            foreach (var file in files)
            {
                var fileName = file.TempFileName(false);
                var fileExt = fileName.Substring(fileName.LastIndexOf('.'));
                var path = Path.Combine(folderClearedName, fileName);

                filesName.Add(fileName);
                if (file.Length > 0)
                {
                    // these steps requires LazZiya.ImageResize package from nuget.org
                     //if (files.FirstOrDefault(ext => ext.Equals(fileExt, StringComparison.OrdinalIgnoreCase)))
                    // {
                         using (var stream = file.OpenReadStream())
                         {
                           using (var img = Image.FromStream(stream))
                           {
                            if (img.Size.Height > height && img.Size.Width > width)
                            {
                                img.ScaleAndCrop(width, height)
                                   .AddTextWatermark("GrandJob", new TextWatermarkOptions { TextColor = Color.FromArgb(255, Color.White), FontSize = 14 })
                                   .SaveAs(path);
                            } else { 
                           img.AddTextWatermark("GrandJob", new TextWatermarkOptions { TextColor = Color.FromArgb(255, Color.White), FontSize = 14 })
                              .SaveAs(path);
                           }
                            counter++;
                        }
                      }
                    // }
                    /* else
                     {
                    // upload and save files to upload folder
                    /*  using (var stream = new FileStream(_GalleryPath, FileMode.Create,
                                 FileAccess.ReadWrite,
                                 FileShare.None,
                                 4096,
                                 FileOptions.SequentialScan))*/

                    
                   /* using (var stream = new FileStream(path, FileMode.Create))

                    {
                        await file.CopyToAsync(stream);
                        counter++;
                    }*/
                    //}
                }
            }

            string newName;
            if (counter >= files.Count())
            {
                newName = String.Join(",", filesName.ToArray());
                return newName;
            }

            await _logService.Create("Грешка при качване на снимка в галерията.", "#", LogLevel.Danger, null).ConfigureAwait(false);
            return null;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string oldFile, string jobTitle, FileType fileType, User user)
        {
            if (file.Length < 0)
            {
                await ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", "Моля опитайте с друг файл.", $"{user?.UserName}, {user?.Role}, {user?.Email} - Файлът не е намерен", 5000).ConfigureAwait(false);
                return null;
            }

            if (file.Length > _FileSizeLimit)
            {
                await ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", "Допустимият размер е до 3MB.", $"{user?.UserName}, {user?.Role}, {user?.Email} - Файлът не е намерен", 5000).ConfigureAwait(false);
                return null;
            }

            string folderClearedName = null;
            string folderJobName;

            switch (fileType)
            {
                case FileType.AppliedCV:
                    {
                        if (!Directory.Exists(_AppliedCVPath))
                            Directory.CreateDirectory(_AppliedCVPath);

                        folderJobName = Path.Combine(_AppliedCVPath, StringHelper.FilterTrimSplit(jobTitle));
                        if (!Directory.Exists(folderJobName))
                            Directory.CreateDirectory(folderJobName);

                        folderClearedName = Path.Combine(folderJobName, StringHelper.Filter(user?.Email));
                    }
                    break;
                case FileType.MyFilesCV:
                    {
                     if (await _filesService.GetFilesByUserCount(user.Id) >= _FilesUploadLimit)
                            return null;

                        if (!Directory.Exists(_myFilesCVPath))
                            Directory.CreateDirectory(_myFilesCVPath);


                        folderClearedName = Path.Combine(_myFilesCVPath, StringHelper.Filter(user?.Email));      
                    }
                    break;

            }

            if (!Directory.Exists(folderClearedName))
            {
                Directory.CreateDirectory(folderClearedName);
            }

            try
            {

                string extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                string fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

                if (String.IsNullOrEmpty(extension) || !_permittedFiles.Contains(extension))
                {
                    await ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", "Този тип файл не е разрешен за качване! Само '.pdf' e разрешен тип.", $"{user?.UserName}, {user?.Role}, {user?.Email} - Файлът не е намерен", 5000).ConfigureAwait(false);
                    return null;
                }


                if (oldFile is not null)
                {
                    DeleteFile(Path.Combine(folderClearedName, oldFile));
                }

                var path = Path.Combine(folderClearedName, fileName);

                using (var stream = new FileStream(path, FileMode.Create,
                    FileAccess.ReadWrite,
                    FileShare.None,
                    4096,
                    FileOptions.Asynchronous))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }
            catch (Exception e)
            {
                await ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", "Неуспешно качване на файл.", $"{user?.UserName}, {user?.Role}, {user?.Email} - {e.Message}", 5000).ConfigureAwait(false);
            }

            return null;
        }

        //[RequestSizeLimit(5000000)]
        public async Task<string> UploadFileAsync(IFormFile file, FileType fileType, string jobTitle, string email)
        {

            if (file?.Length < 0)
            {
                ToastNotify(ToastMessageState.Error, "Грешка", "Неуспешно качване на файл. Опитайте с друг файл.", 5000); 
                return null;
            }

            if (file.Length > _FileSizeLimit)
            {
                ToastNotify(ToastMessageState.Error, "Грешка", "Допустимият размер е 3MB.", 5000);
                return null;
            }

            if (!Directory.Exists(_AppliedCVPath))
                Directory.CreateDirectory(_AppliedCVPath);

            string folderClearedName = null;
            string folderJobName;

            switch (fileType)
            {
                case FileType.GuestsCV:
                    folderJobName = Path.Combine(_AppliedCVPath, StringHelper.FilterTrimSplit(jobTitle));
                    if (!Directory.Exists(folderJobName))
                        Directory.CreateDirectory(folderJobName);

                    folderClearedName = Path.Combine(folderJobName, StringHelper.Filter(email));
                    break;
            }

            if (!Directory.Exists(folderClearedName))
            {
                Directory.CreateDirectory(folderClearedName);
            }

            try
            {

                string extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                string fileName = StringHelper.Filter(email) + extension; 

                if (String.IsNullOrEmpty(extension) || !_permittedFiles.Contains(extension))
                {
                    ToastNotify(ToastMessageState.Error, "Грешка", "Този тип файл не е разрешен за качване! Само '.pdf' e разрешен тип.", 5000);
                    return null;
                }


                var path = Path.Combine(folderClearedName, fileName);

                using (var stream = new FileStream(path, FileMode.Create,
                    FileAccess.ReadWrite,
                    FileShare.None,
                    4096,
                    FileOptions.Asynchronous))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }
            catch (Exception e)
            {
                ToastNotify(ToastMessageState.Error, "Грешка", "Неуспешно качване на файл.", 5000);
            }

            return null;
        }
        public async Task<bool> CloneFileAsync(string File, string jobTitle, User user)
        {
            string folderJobName = Path.Combine(_AppliedCVPath, StringHelper.FilterTrimSplit(jobTitle));
            string folderUserName = Path.Combine(_myFilesCVPath, StringHelper.Filter(user?.Email));
            string full = Path.Combine(folderJobName, StringHelper.Filter(user?.Email));
            if (Directory.Exists(folderUserName))
            {
                if (!Directory.Exists(full))
                {
                    Directory.CreateDirectory(full);
                }

                CopyFilesRecursively(folderUserName, Path.Combine(full, File));
                return true;
            }

            return false;
        }

            public async Task<string> UploadImageAsync(IFormFile file, string oldFile, bool isCompany, User user)
        {
            if (file.Length < 0)
            {
                await ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", "Моля опитайте с друга снимка.", $"{user.UserName}, {user.Role}, {user.Email} - Файлът не е намерен", 5000).ConfigureAwait(false);
                return null;
            }

            string fileName;
            string url = isCompany ? _ImageCompaniesPath : _ImageUsersPath;

            int width = 150;
            int height = 150;

            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

                if (String.IsNullOrEmpty(extension) || !_permittedImages.Contains(extension))
                {
                   await ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", "Този тип файл не е разрешен за качване! Само '.jpg .png .jpeg .gif' са разрешените типове.", $"{user.UserName}, {user.Role}, {user.Email} - Файлът не е намерен", 5000);
                    return null;
                }

                if (!Directory.Exists(url))
                {
                    Directory.CreateDirectory(url);
                }

                if (oldFile != null)
                {
                    DeleteFile(Path.Combine(url, oldFile));
                }

                var path = Path.Combine(url, fileName);

                using (var stream = /*new FileStream(path, FileMode.Create, FileAccess.ReadWrite)*/ file.OpenReadStream())
                {
                    using (var img = Image.FromStream(stream))
                    {
                        if (img.Size.Height > height && img.Size.Width > width)
                        {
                            img.ScaleAndCrop(width, height).SaveAs(path);
                        }
                        else
                        {
                            img.SaveAs(path);
                        }
                    }

                }

                return fileName;
            }
            catch (Exception e)
            {
                await ToastNotifyLogAsync(user, ToastMessageState.Error, "Грешка", "Неуспешно качване на снимка.", $"{user.UserName}, {user.Role}, {user.Email} - {e.Message}", 5000);
            }

            return null;
        }

            public async Task<string> fileScanner(string fileName, string filePath)
        {
            //var scan = await _scanner.ScanAsync(filePath);

            // var result = _scanner.completedProcess(scan);


            /*  switch (_scanner.Scan(filePath))
              {
                  case ScanResult.ThreatFound:
                      {
                          Delete(filePath);
                          ToastNotify(ToastMessageState.Error, "Грешка", "Вашият файл не може да бъде качен. Опитайте по-късно.", 5000);
                          //this._logger.LogError($"\n\n\n\n\nUser with name: , is trying to upload virus file.");
                          return null; // success = false;
                      }
                  case ScanResult.NoThreatFound:
                      {
                          //this._logger.LogInformation($"\n\n\n\n\n's, file: '{fileName}' is clean.");
                          return fileName;// success = true;
                      }
                  case ScanResult.FileNotFound:
                      {
                          //this._logger.LogError($"\n\n\n\n\nNo file found to scan.");
                          return null; // success = false;
                      }

                  case ScanResult.Timeout:
                      {
                          //this._logger.LogError($"\n\n\n\n\nScan Timeout.");
                          return null; // success = false;
                      }

                  case ScanResult.Error:
                      {
                          //this._logger.LogError($"\n\n\n\n\nScan Error.");
                          return null; // success = false;
                      }
                  default:
                      {
                          //this._logger.LogError($"\n\n\n\n\nScan Error.");
                          return null; // success = false;
                      }
              }*/
            return filePath;
        }

        public OperationResult DeleteCompanyResources(Company company)
        {
            if(company is null)
                return OperationResult.FailureResult($"Company is not found.");

            try
            {
                string folderClearedName = Path.Combine(_GalleryPath, StringHelper.Filter(company.Email));

                if (Directory.Exists(folderClearedName))
                {
                    try
                    {
                        DirectoryInfo di = new DirectoryInfo(folderClearedName);
                        FileInfo[] fileInfo = di.GetFiles();
                        foreach (var item in fileInfo)
                        {
                            item.Delete();
                        }
                    }
                    finally
                    {

                        Directory.Delete(folderClearedName);
                    }
                }


                string folderLogo = Path.Combine(_ImageCompaniesPath, company.Logo);
                if (File.Exists(folderLogo))
                {
                    File.Delete(folderLogo);
                }
                return OperationResult.SuccessResult("");
            }
            catch (Exception)
            {
                return OperationResult.FailureResult($"Resources of company {company.Title} could not be removed.");
            }
        }

        public OperationResult DeleteUserResources(User user, bool allImages)
        {
            try
            {
                string folderClearedName = Path.Combine(_myFilesCVPath, StringHelper.Filter(user?.Email));

                if (Directory.Exists(folderClearedName))
                {
                    try
                    {
                        DirectoryInfo di = new DirectoryInfo(folderClearedName);
                        FileInfo[] fileInfo = di.GetFiles();
                        foreach (var item in fileInfo)
                        {
                            item.Delete();
                        }
                    }
                    finally
                    {
                        Directory.Delete(folderClearedName);
                    }
                }

                if (allImages && !(user.PictureName == "200x200.jpg"))
                {
                    string folderLogo = Path.Combine(_ImageUsersPath, user.PictureName);
                    if (File.Exists(folderLogo))
                    {
                        File.Delete(folderLogo);
                    }
                }
                return OperationResult.SuccessResult("");
            }
            catch (Exception)
            {
                return OperationResult.FailureResult($"Resources of user {user.Email} could not be removed.");
            }

        }


        public bool Delete(string fullpath)
        {
            if (Directory.Exists(fullpath))
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(fullpath);
                    FileInfo[] fileInfo = di.GetFiles();
                    foreach (var item in fileInfo)
                    {
                        item.Delete();
                    }
                }
                finally
                {
                    Directory.Delete(fullpath);
                }
                return true;
            }
            return false;
        }
        private bool DeleteFile(string fullpath)
        {
            if (!File.Exists(fullpath))
            {
                return false;
            }

            try
            {
                File.Delete(fullpath);
                return true;
            }
            catch (Exception e)
            {
            }


            return false;
        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            var dir = new DirectoryInfo(sourcePath);
            foreach (FileInfo file in dir.GetFiles())
            {
                //string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetPath);
            }
            //Now Create all of the directories
            /* foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }*/
        }

    }
}
