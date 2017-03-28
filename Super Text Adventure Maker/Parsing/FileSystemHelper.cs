using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Super_Text_Adventure_Maker.DTOs;
using Super_Text_Adventure_Maker.Validation;

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

        public static void ValidateFiles(List<StamFile> files)
        {
            var exceptions = ValidationHelper.ValidateFiles(files);
            if (exceptions.Count <= 0)
            {
                return;
            }

            foreach (var ex in exceptions)
            {
                Console.WriteLine(ex.Message);
            }

            throw new Exception(
                "Your adventure could not be built because of the above errors. Please fix them and try again.");
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
