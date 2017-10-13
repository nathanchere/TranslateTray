using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TranslateTray.Core
{
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

            return string.Join(" ", GetTranslations(innerInput).ToArray());
        }

        private IEnumerable<string> GetTranslations(string input)
        {            
            var index = 0;

            var segments = Debrace(input).Split(new [] {"],["}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var segment in segments)
            {                
                yield return Debrace(segment.Split(',')[0], '"', '"');
            }            
        }

        private string Debrace(string input, char start = '[', char end = ']')
        {
            if (!input.StartsWith(start.ToString()) || !input.EndsWith(end.ToString()))
                throw new InvalidDataException("Expected result in a different format");
            return input.Substring(1, input.Length - 2);
        }
    }
}