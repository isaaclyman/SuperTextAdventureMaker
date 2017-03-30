using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Super_Text_Adventure_Maker.DTOs;

namespace Super_Text_Adventure_Maker.Parsing
{
    public static class FileParseHelper
    {
        public static Scene GetScene(string filePath, string sceneText)
        {
            return new Scene
            {
                Description = SceneParseHelper.GetSceneDescription(sceneText),
                FilePath = filePath,
                Name = SceneParseHelper.GetSceneName(sceneText),
                Text = sceneText
            };
        }

        // Given an enumerable list of StamFile, returns an enumerable list of Scene
        public static IEnumerable<Scene> GetScenes(IEnumerable<StamFile> files)
        {
            return files.SelectMany(file =>
            {
                var scenes = SplitByScene(file.Content);
                return scenes.Select(scene => GetScene(file.FilePath, scene));
            });
        }

        // Given a file path, returns a StamFile
        public static StamFile GetStamFile(string path)
        {
            return new StamFile
            {
                Content = File.ReadAllText(path),
                FilePath = path
            };
        }

        // Given the text content of a file, returns a list of unstructured scene blocks
        public static IEnumerable<string> SplitByScene(string text)
        {
            return
                text.Split(new[] { ">>>" }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(scene => !string.IsNullOrWhiteSpace(scene))
                    .ToList();
        }
    }
}
