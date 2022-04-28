using HireMe.Core.Helpers;
using HireMe.Entities.Enums;
using HireMe.Entities.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HireMe.Services.Interfaces
{
    public interface IBaseService
    {
        Task<OperationResult> Approve(int Id, PostType postType, ApproveType type);

        Task<string> UploadFileAsGuestAsync(IFormFile file, string jobTitle, string email);
        Task<string> MultipleUploadFileAsync(string folderName, int height, int width, List<IFormFile> files);

        Task<string> UploadFileAsync(IFormFile file, string oldFile, User user);

        Task<string> UploadImageAsync(IFormFile file, string oldFile, bool isCompany, User user);

        Task<string> fileScanner(string fileName, string filePath);

        bool Delete(string path);

        void ToastNotify(ToastMessageState state, string title, string message, int duration);

        Task ToastNotifyLogAsync(User user, ToastMessageState state, string title, string message, string errorpage, int duration);

        OperationResult DeleteCompanyResources(Company company);
        OperationResult DeleteUserResources(User user, bool allImages);
    }
}