﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            var rawText = _client.DownloadString(GetTranslationUrl(input));
            return new TranslationParser().ParseText(rawText);            
        }
        
        private string GetTranslationUrl(string input)
        {
            return $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={FromLanguage}&tl={ToLanguage}&dt=t&q={EncodeQuery(input)}";
        }
    }

    public class TranslationParser
    {
        public string FromLanguage => "sv";
        public string ToLanguage => "en";

        public string ParseText(string input)
        {
            if (!input.StartsWith("[[[") || !input.EndsWith($",\"{FromLanguage}\"]"))
                throw new InvalidDataException("Expected result in a different format");

            var results = new List<string>();

            // Drop opening/closing brace and tailing fields
            var innerInput = Debrace(Debrace(input).Substring(0, input.Length - ",null,'sv'".Length - 2));
            foreach (var current in GetTranslations(innerInput))
            {
            }

            return string.Join(" ", results.ToArray());
        }

        private IEnumerable<string> GetTranslations(string input)
        {            
            var index = 0;
            var results = new List<string>();

            index += 2; // skip the ["

            var nextResult = new StringBuilder();

            while (index < input.Length)
            {                                    
                switch (input[index])
                {
                    case '\\':
                        if (input[index + 1] == '"')
                        {
                            index++;
                            nextResult.Append('"');
                        }
                        break;

                    case '"':
                        results.Add(nextResult.ToString());
                        nextResult = new StringBuilder();
                        break;
                        
                    default:
                        nextResult.Append(input[index]);
                        break;
                }
                index++;                             
            }
            yield break;
        }

        private string Debrace(string input)
        {
            if (!input.StartsWith("[") || !input.EndsWith("]"))
                throw new InvalidDataException("Expected result in a different format");

            return input.Substring(1, input.Length - 2);
        }
    }
}