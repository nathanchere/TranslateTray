using System;
using Xunit;

namespace TranslateTray.Core.Tests
{
    public class TranslationClientTests
    {
        [Fact]
        public void Translate_returns_correct_translation_for_real_words()
        {
            ITranslationClient client = new TranslationClient();
            var input = "helgen";
            var expected = "weekend";

            var result = client.Translate(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Translate_returns_translations_for_multiple_fragments()
        {
            ITranslationClient client = new TranslationClient();

            var input = "Nej. Ja,]\" Kanske. Dålig.";
            input = "ananas";
            var expected = "No. Yes,] Perhaps.";

            var result = client.Translate(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Translate_handles_realworld_text_copies_without_crashing()
        {
            ITranslationClient client = new TranslationClient();
            var input = @"Även fast det känns stökigt på kontoret för tillfället vill jag meddela att mycket är på g och efter semestertiderna ska det mesta vara färdigställt. 

•	Skåp blir levererade V.25/V.26
•	Interna mötesrum tas om hand succesivt och möbler att komplettera med är beställda
•	Lounger på alla våningsplan beräknas vara på plats 23/6
•	Ny receptionsdisk till både plan 6 och 7 är på god väg och beräknas komma på plats under semesterveckorna
•	Internhissen beräknas vara klar om 2-3 veckor

Viktigt är att så fort skåpen är på plats ska ALLA kartonger bort och därefter kommer kontoret grovstädas.
                                                      z
Förutom det önskar jag alla en härlig helg 

Bästa hälsningar


";
            var max = input.Length - 1;
            int currentLength = 0;
            string currentText = "";
            try
            {
                for (currentLength = 10; currentLength < input.Length; currentLength += 10, currentLength = Math.Min(max, currentLength))
                {
                    currentText = input.Substring(0, currentLength);
                    client.Translate(currentText);
                }
            }
            catch (Exception ex)
            {
                Assert.Equal(currentText, $"Failed on run at {currentLength} characters\nError:{ex.Message}\nValue:\n{currentText}");
            }

        }
    }
}
