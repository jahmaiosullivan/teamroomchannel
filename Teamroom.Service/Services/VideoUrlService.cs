using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HobbyClue.Business.Services
{
    public class VideoUrlService : IVideoUrlService
    {
        public string GetTitleFromUrl(string videoUrl)
        {
            var req = WebRequest.Create(videoUrl);
            using (var resp = req.GetResponse())
            {
                var responseStream = resp.GetResponseStream();
                var pageSource = new StreamReader(responseStream ?? new MemoryStream(), Encoding.UTF8).ReadToEnd();
                return GetVideoTitle(pageSource);
            }
        }
        private static string GetVideoTitle(string pageSource)
        {
            string videoTitle = null;

            try
            {
                const string videoTitlePattern = @"\<meta name=""title"" content=""(?<title>.*)""\>";
                var videoTitleRegex = new Regex(videoTitlePattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                var videoTitleMatch = videoTitleRegex.Match(pageSource);

                if (videoTitleMatch.Success)
                {
                    videoTitle = videoTitleMatch.Groups["title"].Value;
                    videoTitle = HttpUtility.HtmlDecode(videoTitle);

                    // Remove the invalid characters in file names
                    // In Windows they are: \ / : * ? " < > |
                    videoTitle = Regex.Replace(videoTitle, @"[:\*\?""\<\>\|]", String.Empty);
                    videoTitle = videoTitle.Replace("\\", "-").Replace("/", "-").Trim();
                }
            }
            catch (Exception)
            {
                videoTitle = null;
            }

            return videoTitle;
        }

    }
}
