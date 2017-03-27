using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Super_Text_Adventure_Maker.DTOs;

namespace Super_Text_Adventure_Maker.Helpers
{
    public static class SceneParseHelper
    {
        // Given scene text, returns a list of unstructured action blocks
        public static IEnumerable<string> GetActions(string text)
        {
            var trimmedText = text.Trim();
            var actionsStart = trimmedText.IndexOf("|", StringComparison.Ordinal);

            var actionsSection = trimmedText.Substring(actionsStart);

            // Split by a pipe that is followed by one or more non-whitespace, non-pipe characters and then a final pipe.
            // Lookahead means that only the first pipe is removed.
            return Regex.Split(actionsSection, @"\|(?=[^\s\|]*\|)");
        }

        // Given an enumerable list of file paths, returns an enumerable list of Scene objects
        public static IEnumerable<Scene> GetScenes(IEnumerable<string> paths)
        {
            return paths.SelectMany(path =>
            {
                var text = File.ReadAllText(path);
                var scenes = FileParseHelper.SplitByScene(text);
                return
                    scenes.Select(
                        scene => new Scene { FilePath = path, Name = GetSceneName(scene), Text = scene });
            });
        }

        // Given scene text, returns the trimmed scene description
        public static string GetSceneDescription(string text)
        {
            var trimmedText = text.Trim();
            var startIndex = trimmedText.IndexOf(">", StringComparison.Ordinal);
            var endIndex = trimmedText.IndexOf("|", StringComparison.Ordinal);

            if (endIndex == -1)
            {
                endIndex = trimmedText.Length;
            }

            return trimmedText.Substring(startIndex, endIndex - startIndex).Trim();
        }

        // Given scene text, returns the trimmed scene name
        private static string GetSceneName(string text)
        {
            var trimmedText = text.Trim();
            var endIndex = trimmedText.IndexOf(">", StringComparison.Ordinal);
            var sceneName = endIndex > 0 ? trimmedText.Substring(0, endIndex).Trim() : string.Empty;
            return sceneName;
        }
    }
}
