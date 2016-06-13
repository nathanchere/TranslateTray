using System;
using System.Net;
using System.Text;

namespace TranslateTray.Core
{
    public static class WebClientExtensions
    {
        public static string DownloadStringEx(this WebClient webClient, Uri uri)
        {
            var rawData = webClient.DownloadData(uri);
            var encoding = WebUtils.GetEncodingFrom(webClient.ResponseHeaders, Encoding.UTF8);
            return encoding.GetString(rawData);
        }
    }
}