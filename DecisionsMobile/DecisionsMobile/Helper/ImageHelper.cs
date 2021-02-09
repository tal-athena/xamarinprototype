using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using DecisionsMobile.Models.FormService;
using DecisionsMobile.Services;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace DecisionsMobile.Helper
{
    public class ImageHelper
    {
        public static string GetImageInfoUrl(ImageInfo info, int width = 48, int height = 48)
        {

            if (info.ImageType == (long)ImageInfoType.Url)
            {
                return info.ImageUrl;
            }
            else if (info.ImageType == (long)ImageInfoType.StoredImage)
            {
                return Uri.EscapeUriString($"{AuthService.Instance.CurrentSession.ServerBaseUrl}/Handlers/SvgImage.ashx?svgFile={info.ImageId}&width={width}&height={height}");

            }
            else if (info.ImageType == (long)ImageInfoType.Document)
            {
                return Uri.EscapeUriString($"{AuthService.Instance.CurrentSession.ServerBaseUrl}/Handlers/SvgImage.ashx?documentId={info.DocumentId}&width={width}height=${height}");
            }
            else if (info.ImageType == (long)ImageInfoType.RawData && info.ImageFileReferenceId != null)
            {
                var fileNameArr = ((string)info.ImageFileReferenceId).Split('-');
                if (fileNameArr.Length >= 2)
                {
                    string referenceId = (string)info.ImageFileReferenceId;
                    referenceId = referenceId.Replace($"{fileNameArr[0]}_", "");

                    return Uri.EscapeUriString($"{AuthService.Instance.CurrentSession.ServerBaseUrl}/{RestConstants.GetServiceUri("FileReferenceService", "DownloadFile")}?id={fileNameArr[0]}&filename={referenceId}");
                }
            }

            return null;
        }

        public static ImageSource GetImageSourceFromBase64EncodedUrl(string encodedUrl)
        {
            if (encodedUrl == null)
                return null;

            var match = Regex.Match(encodedUrl, @"data:(?<type>.+?);base64,(?<data>.+)");
            
            var contentType = match.Groups["type"].Value;

            if (string.IsNullOrEmpty(contentType) || !contentType.Contains("image"))
                return null;

            var base64Data = match.Groups["data"].Value;            
            var binData = Convert.FromBase64String(base64Data);

            return ImageSource.FromStream(() => new MemoryStream(binData));
        }
    }
}
