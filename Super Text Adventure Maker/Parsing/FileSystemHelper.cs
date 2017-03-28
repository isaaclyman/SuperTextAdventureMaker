using System.Collections.Generic;
using System.IO;
using System.Linq;
using Super_Text_Adventure_Maker.DTOs;

namespace Super_Text_Adventure_Maker.Parsing
{
    public static class FileSystemHelper
    {
        public static List<StamFile> GetStamFiles()
        {
            var baseFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            var files = SearchStamFiles(baseFolder);

            return files.Select(FileParseHelper.GetStamFile).ToList();
        }

        private static IEnumerable<string> SearchStamFiles(string folder)
        {
            var files =
                Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
                    .Where(file => file.EndsWith(".txt") || file.EndsWith(".text") || file.EndsWith(".stam"));

            return files.ToList();
        }
    }
}
