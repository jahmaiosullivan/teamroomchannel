using System;

namespace HobbyClue.Common.Helpers
{
    public static class ArgumentValidator
    {
        public static void ValidateNotNull<T>(T item) where T : class
        {
            if (item == null)
                throw new ArgumentNullException("item");

        }

        public static void ValidateNotEmptyGuid(Guid item)
        {
            if (item == Guid.Empty)
                throw new ArgumentException("item cannot be empty guid");

        }
    }
}
