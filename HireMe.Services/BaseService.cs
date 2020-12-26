using HireMe.Data.Repository.Interfaces;
using HireMe.Services.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using HireMe.Entities.Enums;
using HireMe.Core.Helpers;
using HireMe.Entities.Models;
using HireMe.Mapping.Utility;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace HireMe.Services
{
    public class BaseService : IBaseService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Jobs> _jobsRepository;
        private readonly IRepository<Contestant> _contestantRepository;
        private readonly IRepository<Company> _companiesRepository;

        private readonly ILogService _logService;
        private readonly IToastNotification _toastNotification;

        private readonly IResumeService _resumeService;
        private readonly int _resumeSizeLimit;

        private readonly long _fileSizeLimit;
        private readonly string _FilePath;
        private readonly string[] _permittedFiles = { ".pdf", ".ps", ".doc", ".docx", ".odt", ".sxw", ".txt", ".rtf" };

        private readonly long _imageSizeLimit;
        private readonly string _ImagePath;
        private readonly string[] _permittedImages = { ".png", ".jpg", ".gif", ".jpeg", ".JPG", ".JPEG", ".PNG", ".GIF" };


        public BaseService(
              IConfiguration config,
              IRepository<User> userRepository,
              IRepository<Jobs> jobsRepository,
              IRepository<Contestant> contestantRepository,
              IRepository<Company> companiesRepository,
              IToastNotification toastNotification,
              ILogService logService,
              IResumeService resumeService)
        {
             _userRepository = userRepository;
             _jobsRepository = jobsRepository;
             _contestantRepository = contestantRepository;
             _companiesRepository = companiesRepository;

            _toastNotification = toastNotification;
            _resumeService = resumeService;
            _logService = logService;

            _resumeSizeLimit = config.GetValue<int>("MySettings:ResumeUploadLimit");
            _fileSizeLimit = config.GetValue<long>("MySettings:FileSizeLimit");
            _FilePath = config.GetValue<string>("MySettings:FilePathHosting"); 
            _imageSizeLimit = config.GetValue<long>("ImageSizeLimit");
            _ImagePath = config.GetValue<string>("MySettings:ImagePathHosting");
        }

        public void ToastNotifyLog(User user, ToastMessageState state, string title, string message, string errorpage, int duration)
        {
            switch (state)
            {
                case ToastMessageState.Success:
                       ToastNotify(ToastMessageState.Success, title, message, duration);
                    break;
                case ToastMessageState.Info:
                    {
                        ToastNotify(ToastMessageState.Info, title, message, duration);
                        _logService.Create(message, errorpage, LogLevel.Info, user.UserName);
                    }
                    break;
                case ToastMessageState.Alert:
                        ToastNotify(ToastMessageState.Alert, title, message, duration);
                    break;
                case ToastMessageState.Warning:
                    {
                        ToastNotify(ToastMessageState.Warning, title, message, duration);
                        _logService.Create(message, errorpage, LogLevel.Warning, user.UserName);
                    }
                    break;
                case ToastMessageState.Error:
                    {
                        ToastNotify(ToastMessageState.Error, title, message, duration);
                        _logService.Create(message, errorpage, LogLevel.Danger, user.UserName);
                    }
                    break;
            }
        }
        public void ToastNotify(ToastMessageState state, string title, string message, int duration)
        {
            switch (state)
            {
                case ToastMessageState.Success:_toastNotification.AddSuccessToastMessage(message, new ToastrOptions() { Title = title, TimeOut = duration });
                    break;
                case ToastMessageState.Info:_toastNotification.AddInfoToastMessage(message, new ToastrOptions() { Title = title, TimeOut = duration });            
                    break;
                case ToastMessageState.Alert: _toastNotification.AddAlertToastMessage(message, new ToastrOptions() { Title = title, TimeOut = duration });          
                    break;
                case ToastMessageState.Warning:_toastNotification.AddWarningToastMessage(message, new ToastrOptions() { Title = title, TimeOut = duration });
                    break;
                case ToastMessageState.Error:_toastNotification.AddErrorToastMessage(message, new ToastrOptions() { Title = title, TimeOut = duration });
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

        [RequestSizeLimit(5000000)]
        public async Task<string> UploadFileAsync(IFormFile file, string oldFile, User user)
        {
            if (await _resumeService.GetFilesByUserCount(user.Id) >= _resumeSizeLimit)          
                return null;

            if (file.Length < 0)
            {
                ToastNotifyLog(user, ToastMessageState.Error, "Грешка", "Моля опитайте с друга снимка.", $"{user.UserName}, {user.Role}, {user.Email} - Файлът не е намерен", 5000);
                return null;
            }

            string fileName;

            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

                if (String.IsNullOrEmpty(extension) || !_permittedFiles.Contains(extension))
                {
                    ToastNotifyLog(user, ToastMessageState.Error, "Грешка", "Този тип файл не е разрешен за качване! Само '.pdf .ps .doc .docx .odt .sxw .txt .rtf' са разрешените типове.", $"{user.UserName}, {user.Role}, {user.Email} - Файлът не е намерен", 5000);
                    return null;
                }

                if (!Directory.Exists(_FilePath))
                {
                    Directory.CreateDirectory(_FilePath);
                }

                if (oldFile != null)
                {
                    Delete(_FilePath + oldFile);
                }

                var path = Path.Combine(_FilePath, fileName);

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
                //log error
            }

            return null;
        }

        [RequestSizeLimit(2000000)]
        public async Task<string> UploadImageAsync(IFormFile file, string oldFile, User user)
        {
            if (file.Length < 0)
            {
                ToastNotifyLog(user, ToastMessageState.Error, "Грешка", "Моля опитайте с друга снимка.", $"{user.UserName}, {user.Role}, {user.Email} - Файлът не е намерен", 5000);
                return null;
            }

            string fileName;

            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

                if (String.IsNullOrEmpty(extension) || !_permittedImages.Contains(extension))
                {
                    ToastNotifyLog(user, ToastMessageState.Error, "Грешка", "Този тип файл не е разрешен за качване! Само '.jpg .png .jpeg .gif' са разрешените типове.", $"{user.UserName}, {user.Role}, {user.Email} - Файлът не е намерен", 5000);
                    return null;
                }

                if (!Directory.Exists(_ImagePath))
                {
                    Directory.CreateDirectory(_ImagePath);
                }

                if (oldFile != null)
                {
                    Delete(_ImagePath + oldFile);
                }

                var path = Path.Combine(_ImagePath, fileName);

                using (var stream = new FileStream(path, FileMode.Create, 
                    FileAccess.ReadWrite,
                    FileShare.None,
                    4096,
                    FileOptions.SequentialScan))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }
            catch (Exception e)
            {
                //log error
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

        public bool Delete(string fullpath)
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

    }
}
