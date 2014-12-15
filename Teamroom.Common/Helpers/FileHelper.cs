using System.IO;

namespace HobbyClue.Common.Helpers
{
    public static class FileHelper
    {
        public static void CreateDirectoryIfNotExists(string directoryName)
        {
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
        }

        private static readonly string[] sizeArry = { "Byes", "KB", "MB", "GB" };
        public static string GetFileSize(ulong sizebytes, int index = 0)
        {
            while (true)
            {
                if (sizebytes < 1000)
                    return sizebytes + sizeArry[index];

                sizebytes = sizebytes/1024;
                index = ++index;
            }
        }
    }
}