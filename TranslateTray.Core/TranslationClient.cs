using System;
using System.Collections;
using System.Net;
using System.Text;

namespace TranslateTray.Core
{
    public class TranslationClient : ITranslationClient
    {
        public string FromLanguage => "sv";
        public string ToLanguage => "en";

        private readonly WebClient _client;

        private string EncodeQuery(string input) => Uri.EscapeDataString(input);        

        public TranslationClient()
        {            
            _client = new WebClient { Encoding = Encoding.UTF8 };
            _client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
            _client.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
        }

        public string Translate(string input)
        {
            var cleanInput = input
                .Replace('\\', '/')
                .Replace('"', '\'')
                .Replace('[', '{')
                .Replace(']', '}')
                .Replace(',', '~');
            var rawText = _client.DownloadString(GetTranslationUrl(cleanInput));
            return new TranslationParser().ParseText(rawText);            
        }
        
        private string GetTranslationUrl(string input)
        {
            return $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={FromLanguage}&tl={ToLanguage}&dt=t&q={EncodeQuery(input)}";
        }
    }
}