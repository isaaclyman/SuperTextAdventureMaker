using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Super_Text_Adventure_Maker.DTOs;

namespace Super_Text_Adventure_Maker.Parsing
{
    public static class FileSystemHelper
    {
        private const string PackageExtension = ".stam.game";
        private const string ProjectFolderName = "STAM";

        public static string GetCurrentPath()
        {
            var baseFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            // If we're running in Visual Studio...
            if (System.Diagnostics.Debugger.IsAttached)
            {
                baseFolder = Path.GetFullPath(Path.Combine(baseFolder, @"..\..\Examples"));
            }

            return baseFolder;
        }

        // Recursively finds files with STAM-compatible extensions as a Dictionary<projectName, List<StamFile>>
        // If there is a STAM folder, each folder therein will be a project
        // Otherwise, all files are considered a part of a project with name string.Empty
        public static Dictionary<string, List<StamFile>> GetStamProjects()
        {
            var baseFolder = GetCurrentPath();

            if (DirectoryHasProjectFolder(baseFolder))
            {
                baseFolder = Path.GetFullPath(Path.Combine(baseFolder, $"{ProjectFolderName}"));
                var projectPaths = Directory.GetDirectories(baseFolder);
                var stamProjects = projectPaths.ToDictionary(path => new DirectoryInfo(path).Name,
                    path => SearchStamFiles(path).Select(FileParseHelper.GetStamFile).ToList());

                return stamProjects;
            }

            var files = SearchStamFiles(baseFolder).ToList();

            if (files.Count <= 0)
            {
                throw new FileNotFoundException();
            }

            return new Dictionary<string, List<StamFile>>
            {
                { string.Empty, files.Select(FileParseHelper.GetStamFile).ToList() }
            };
        }

        public static List<Scene> ReadPackage(string path)
        {
            var encodedFile = File.ReadAllText(path);
            var decodedBytes = Convert.FromBase64String(encodedFile);
            var decoded = Encoding.UTF8.GetString(decodedBytes);

            return
                FileParseHelper.SplitByScene(decoded)
                    .Select(sceneText => FileParseHelper.GetScene(path, sceneText))
                    .ToList();
        }

        public static IEnumerable<string> SearchPackages(string basePath)
        {
            var files = Directory.GetFiles(basePath, $"*{PackageExtension}", SearchOption.AllDirectories);
            return files;
        }

        public static void WritePackage(List<Scene> scenes, string path)
        {
            var sceneSeparator = $"{Environment.NewLine}>>>{Environment.NewLine}";
            var decoded = string.Join(sceneSeparator, scenes.Select(scene => scene.Text));
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(decoded));

            File.WriteAllText($"{path}{PackageExtension}", encoded);
        }

        private static bool DirectoryHasProjectFolder(string pathName)
        {
            return Directory.GetDirectories(pathName, ProjectFolderName).Length > 0;
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
