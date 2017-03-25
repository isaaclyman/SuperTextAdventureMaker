using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super_Text_Adventure_Maker.Helpers
{
    internal static class FileSystemHelper
    {
        public static string GetConcatenatedFiles(string baseFolder = null)
        {
            baseFolder = baseFolder ?? Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            var files = SearchStamFiles(baseFolder);
            ValidateFiles(files);

            
        }

        private static void ValidateFiles(List<string> files)
        {
            var exceptions = ValidationHelper.ValidateFiles(files);
            if (exceptions.Count <= 0)
            {
                return;
            }

            Console.WriteLine("Your adventure has errors! Please fix them and try again:");
            foreach (var ex in exceptions)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static string ConcatFiles(List<string> paths)
        {
            return string.Join("\n\n", paths.Select(File.ReadAllText));
        }

        private static List<string> SearchStamFiles(string folder)
        {
            var files =
                Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
                    .Where(file => file.EndsWith(".txt") || file.EndsWith(".text") || file.EndsWith(".stam"));

            return files.ToList();
        }
    }
}
