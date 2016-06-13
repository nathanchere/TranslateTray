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
    }
}
