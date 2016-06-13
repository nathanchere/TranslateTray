using System;
using System.IO;
using System.Net;

namespace TranslateTray.Core
{
    public class TranslationClient : ITranslationClient
    {
        public string FromLanguage => "sv";
        public string ToLanguage => "en";

        private string EncodeQuery(string input) => Uri.EscapeDataString(input);        

        public string Translate(string input)
        {
            var format = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={FromLanguage}&tl={ToLanguage}&dt=t&q={EncodeQuery(input)}";

            var rawResult = new WebClient().DownloadString(format);

            if (!rawResult.StartsWith("[[[") || !rawResult.EndsWith($"]],,\"{FromLanguage}\"]"))
                throw new InvalidDataException("Expected result in a different format");

            var result = rawResult.Substring(4, rawResult.IndexOf($"\",\"{input}\",,,") - 4);

            return result;
        }
    }
}