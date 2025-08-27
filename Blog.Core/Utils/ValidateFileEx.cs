using System.Globalization;

namespace Blog.Core.Utils;

public static class ValidateFileEx
{
    private static readonly string[] AllowedExtension = [".jpg", ".jpeg", ".png", ".gif", ".pdf", ".doc", ".docx",".xls", ".xlsx", ".txt",".webp"];

    #region Validate

    public static bool IsValidFileName(this string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return false;
        }

        fileName = Path.GetFileName(fileName);

        var fileEx = Path.GetExtension(fileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(fileEx) || !AllowedExtension.Contains(fileEx))
        {
            return false;
        }

        if (fileName.Contains(".."))
        {
            return false;
        }

        return true;
    }

    #endregion

    #region ToPersianDateTimeForFileName

    public static string ToPersianDateTimeForFileName(this DateTime t)
    {
        var pc = new PersianCalendar();

        return $"{pc.GetYear(t).ToString()}-{pc.GetMonth(t).ToString()}-" +
               $"{pc.GetDayOfMonth(t).ToString()}-{pc.GetHour(t).ToString()}-" +
               $"{pc.GetMinute(t).ToString()}-{pc.GetSecond(t).ToString()}-" +
               $"{pc.GetMilliseconds(t).ToString()}";
    }

    #endregion

    #region ToUniqueFileName

    public static string ToUniqueFileName(this string fileName)
    {
        fileName = Path.GetFileName(fileName.ToLower());

        var unique = $"{Guid.NewGuid().ToString().Replace("-", "")}-{DateTime.Now.ToPersianDateTimeForFileName()}-{fileName}";

        return unique;
    }

    #endregion

}