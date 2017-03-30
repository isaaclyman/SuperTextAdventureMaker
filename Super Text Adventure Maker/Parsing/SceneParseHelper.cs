using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Super_Text_Adventure_Maker.DTOs;

namespace Super_Text_Adventure_Maker.Parsing
{
    public static class SceneParseHelper
    {
        // Given scene text, returns an enumerable list of SceneAction
        public static IEnumerable<SceneAction> GetSceneActions(Scene scene)
        {
            var actionsStart = scene.Text.IndexOf("|", StringComparison.Ordinal);
            if (actionsStart == -1)
            {
                return new List<SceneAction>();
            }

            var actionsSection = scene.Text.Substring(actionsStart);

            // Split by a pipe that is followed by one or more non-whitespace, non-pipe characters and then a final pipe.
            // Lookahead means that only the first pipe is removed.
            var actionBlocks = Regex.Split(actionsSection, @"\|(?=[^\s\|]*\|)").Where(block => !string.IsNullOrWhiteSpace(block));

            return actionBlocks.Select(actionBlock => ActionParseHelper.GetSceneAction(scene, actionBlock)).ToList();
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
        public static string GetSceneName(string text)
        {
            var trimmedText = text.Trim();
            var endIndex = trimmedText.IndexOf(">", StringComparison.Ordinal);
            var sceneName = endIndex > 0 ? trimmedText.Substring(0, endIndex).Trim() : string.Empty;
            return sceneName;
        }
    }
}
