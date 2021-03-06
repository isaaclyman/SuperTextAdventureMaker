﻿using System;
using Super_Text_Adventure_Maker.DTOs;

namespace Super_Text_Adventure_Maker.Parsing
{
    public static class ActionParseHelper
    {
        // Given an action blocks, returns a SceneAction object
        // SceneAction.NextScene and SceneAction.Result may be null
        public static SceneAction GetSceneAction(Scene scene, string action)
        {
            var abbreviationEnd = action.IndexOf("|", StringComparison.Ordinal);
            var abbreviation = action.Substring(0, abbreviationEnd).Trim();

            var descriptionStart = abbreviationEnd + 1;
            var descriptionEnd = action.IndexOf(">", StringComparison.Ordinal);
            if (descriptionEnd == -1)
            {
                descriptionEnd = action.Length;
            }
            var description = action.Substring(descriptionStart, descriptionEnd - descriptionStart).Trim();

            // The action result and the next scene can be given in any order
            var nextSceneLineStart = action.IndexOf(">>", StringComparison.Ordinal);
            string nextScene = null;
            if (nextSceneLineStart != -1)
            {
                var nextSceneTextStart = nextSceneLineStart + ">>".Length;
                var nextSceneTextEnd = action.IndexOf(">", nextSceneTextStart, StringComparison.Ordinal);

                nextScene = nextSceneTextEnd == -1
                    ? action.Substring(nextSceneTextStart).Trim()
                    : action.Substring(nextSceneTextStart, nextSceneTextEnd - nextSceneTextStart).Trim();
            }

            var resultLineStart = action.IndexOf(">", StringComparison.Ordinal);
            string result = null;
            if (resultLineStart == nextSceneLineStart)
            {
                resultLineStart = action.IndexOf(">", nextSceneLineStart + ">>".Length, StringComparison.Ordinal);
            }

            if (resultLineStart != -1)
            {
                var resultTextStart = resultLineStart + ">".Length;
                var resultTextEnd = action.IndexOf(">>", resultTextStart, StringComparison.Ordinal);

                result = resultTextEnd == -1
                    ? action.Substring(resultTextStart).Trim()
                    : action.Substring(resultTextStart, resultTextEnd - resultTextStart).Trim();
            }

            return new SceneAction
            {
                Abbreviation = abbreviation,
                Description = description,
                NextScene = nextScene,
                Result = result,
                Scene = scene
            };
        }
    }
}
