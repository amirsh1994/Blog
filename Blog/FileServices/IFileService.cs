using Blog.Core.Common;
using Blog.Core.Utils;

namespace Blog.Web.FileServices;

public interface IFileService
{
    Task<OperationResult<string>> UploadImage(IFormFile? file, string folderName);

    Task<OperationResult<string>> RemoveImage(string relativePath);
}

public class FileService(IWebHostEnvironment env) : IFileService
{
    #region Upload

    public async Task<OperationResult<string>> UploadImage(IFormFile? file, string folderName)
    {
        if (file is null)
        {
            return new OperationResult<string>
            {
                IsSuccess = true,
                Message = "",
                Code = OperationCode.Success,
                Data = ""
            };
        }

        var fileName = Path.GetFileName(file.FileName);

        if (!fileName.IsValidFileName())
        {
            return new OperationResult<string>
            {
                IsSuccess = false,
                Message = "فرمت آپلود شده فایل صحیح نمی باشد",
                Code = OperationCode.Failed,
                Data = ""
            };
        }

        var uniqueFileName = ToUniqueFileName(fileName);
        var physicalAddress = ToPhysicalAddress(uniqueFileName, folderName);

        await using (var fs = new FileStream(physicalAddress, FileMode.Create))
        {
            await file.CopyToAsync(fs);
        }

        return new OperationResult<string>
        {
            IsSuccess = true,
            Message = "آپلود شد",
            Code = OperationCode.Success,
            Data = ToRelativeAddress(folderName, uniqueFileName)
        };

    }
    #endregion

    #region Remove

    public async Task<OperationResult<string>> RemoveImage(string relativePath)
    {
        try
        {
            var filePath = Path.Combine(env.WebRootPath, relativePath.TrimStart('/'));

            if (File.Exists(filePath))
                File.Delete(filePath);

            return new OperationResult<string>
            {
                IsSuccess = true,
                Message = "فایل حذف شد",
                Code = OperationCode.Success
            };
        }
        catch (Exception ex)
        {
            return new OperationResult<string>
            {
                IsSuccess = false,
                Message = ex.Message,
                Code = OperationCode.Failed
            };
        }
    }

    #endregion

    #region private functions

    private string ToPhysicalAddress(string fileName, string folderName)
    {
        var physicalFolder = Path.Combine(env.WebRootPath, folderName);

        if (Directory.Exists(physicalFolder) == false)
        {
            Directory.CreateDirectory(physicalFolder);
        }

        var physicalAddress = Path.Combine(physicalFolder, fileName);

        return physicalAddress;
    }

    private string ToRelativeAddress(string folderName, string uniqueFileName)
    {
        var relative = "/" + folderName + "/" + uniqueFileName;
        return relative;
    }

    private string ToUniqueFileName(string fileName)
    {
        var uniqueFileName = fileName.ToUniqueFileName();
        return uniqueFileName;
    }
    #endregion
}