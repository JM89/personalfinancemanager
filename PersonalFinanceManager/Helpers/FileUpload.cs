using PersonalFinanceManager.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Helpers
{
    public class FileUpload
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
            var businessException = new BusinessException();

            if (file == null)
            {
                throw new BusinessException(fieldName, BusinessExceptionMessage.FileUploadHasNotBeenSelected);
            }

            var relativeFilePath = string.Empty;

            var path = System.Web.Hosting.HostingEnvironment.MapPath((relativeLocation.StartsWith("/") ? ("~") : string.Empty) + relativeLocation);

            if (path == null)
            {
                throw new ArgumentException("Location is not found on the server.");
            }

            if (file.ContentLength > 0)
            {
                if (file.ContentLength > maxSize)
                {
                    businessException.AddErrorMessage(fieldName, string.Format(BusinessExceptionMessage.FileUploadMaxSize, maxSize));
                }
                else
                {
                    var fileExtension = Path.GetExtension(file.FileName).ToLower().Replace(".", "");

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        var allowedExtensionsStr = string.Join(";", allowedExtensions.ToArray());

                        businessException.AddErrorMessage(fieldName, string.Format(BusinessExceptionMessage.FileUploadWrongExtensions, fileExtension, allowedExtensionsStr));
                    }
                    else
                    {
                        var filePath = Path.Combine(path, Path.GetFileName(file.FileName));
                        if (File.Exists(filePath))
                        {
                            businessException.AddErrorMessage(fieldName, BusinessExceptionMessage.FileUploadSameName);
                        }
                        else
                        {
                            file.SaveAs(filePath);

                            relativeFilePath = string.Format("{0}/{1}", relativeLocation, file.FileName);
                        }
                    }
                }
            }
            else
            {
                businessException.AddErrorMessage(fieldName, BusinessExceptionMessage.FileUploadEmpty);
            }

            if (businessException.HasError())
            {
                throw businessException;
            }

            return relativeFilePath;
        }
    }
}