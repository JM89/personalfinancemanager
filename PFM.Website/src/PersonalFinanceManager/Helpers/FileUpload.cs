using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using PersonalFinanceManager.Services.Exceptions;

namespace PersonalFinanceManager.Helpers
{
    public static class FileUpload
    {
        /// <summary>
        /// Validate and upload a file on the webserver (default location is root of the current project).
        /// </summary>
        /// <param name="file">Posted file.</param>
        /// <param name="fieldName">Property name for form validation.</param>
        /// <param name="relativeLocation">Relative Path with Filename.</param>
        /// <param name="maxSize">File max size.</param>
        /// <param name="allowedExtensions">List of allowed extensions.</param>
        /// <returns></returns>
        public static string UploadFileToServer(HttpPostedFileBase file, string fieldName, string relativeLocation, long maxSize, List<string> allowedExtensions)
        {
            if (file?.FileName == null)
                throw new BusinessException(fieldName, BusinessExceptionMessage.FileUploadHasNotBeenSelected);

            var path = System.Web.Hosting.HostingEnvironment.MapPath((relativeLocation.StartsWith("/") ? ("~") : string.Empty) + relativeLocation);

            if (path == null)
                throw new ArgumentException("Location is not found on the server.");

            if (file.ContentLength <= 0)
                throw new BusinessException(fieldName, BusinessExceptionMessage.FileUploadEmpty);

            if (file.ContentLength > maxSize)
                throw new BusinessException(fieldName, string.Format(BusinessExceptionMessage.FileUploadMaxSize, maxSize));

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant().Replace(".", "");

            if (!allowedExtensions.Contains(fileExtension))
            {
                var allowedExtensionsStr = string.Join(";", allowedExtensions.ToArray());
                throw new BusinessException(fieldName, string.Format(BusinessExceptionMessage.FileUploadWrongExtensions, fileExtension, allowedExtensionsStr));
            }

            var filePath = Path.Combine(path, Path.GetFileName(file.FileName));
            if (File.Exists(filePath))
                throw new BusinessException(fieldName, BusinessExceptionMessage.FileUploadSameName);

            file.SaveAs(filePath);

            return $"{relativeLocation}/{file.FileName}";
        }
    }
}