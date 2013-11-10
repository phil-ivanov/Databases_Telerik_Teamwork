using System;
using System.Linq;

namespace TrasnferDataToSqlServer
{
    public static class DirectoryInfoExtention
    {
        public static void EmptyDirectory(this System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles())
            { 
                file.Delete();
            }

            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories())
            {
                subDirectory.Delete(true);
            }
        }
    }
}
