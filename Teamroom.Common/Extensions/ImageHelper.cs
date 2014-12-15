namespace HobbyClue.Common.Extensions
{
    public static class ImageHelper
    {
        public static bool TrimmedStringIsNullOrEmpty(this string text)
        {
            return (text != null && text.Trim() != string.Empty);
        }

    }
}
