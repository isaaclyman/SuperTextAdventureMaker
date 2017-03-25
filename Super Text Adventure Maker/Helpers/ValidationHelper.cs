using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Super_Text_Adventure_Maker.DTOs;

namespace Super_Text_Adventure_Maker.Helpers
{
    public static class ValidationHelper
    {
        public static List<Exception> ValidateFiles(List<string> paths)
        {
            var exceptions = new List<Exception>();
            var scenes = SceneParseHelper.GetScenes(paths).ToList();

            // Adventure-wide validation methods
            exceptions.Add(OneEntrySceneExists(scenes));

            // Scene-specific validation methods
            foreach (var scene in scenes)
            {
                exceptions.Add(SceneHasDescription(scene));

                // Scene-wide action validation methods
                var actionBlocks = SceneParseHelper.GetActions(scene.Text).ToList();
                var actions = ActionParseHelper.GetSceneActions(actionBlocks).ToList();

                exceptions.Add(AtLeastOneActionHasNextScene(scene, actions));
                exceptions.Add(DefaultActionIsValid(scene, actions));

                // Action-specific validation methods
                foreach (var action in actions)
                {
                    exceptions.Add(ActionHasDescription(scene, action));
                    exceptions.Add(ActionHasResultOrNextScene(scene, action));
                }
            }

            // TODO: Validate that every "next scene" actually exists
            // TODO: Validate that there are no duplicate scene names
            // TODO: Validate that there are no duplicate action abbreviations or action descriptions in a scene
            // TODO: Validate that a scene doesn't only have itself as a next scene

            return exceptions.Where(ex => ex != null).ToList();
        }

        private static Exception ActionHasDescription(Scene scene, SceneAction action)
        {
            if (!string.IsNullOrWhiteSpace(action.Abbreviation) && string.IsNullOrWhiteSpace(action.Description))
            {
                return ActionValidationError("The action does not have a description.", scene, action);
            }

            return null;
        }

        private static Exception ActionHasResultOrNextScene(Scene scene, SceneAction action)
        {
            if (string.IsNullOrWhiteSpace(action.Result) && string.IsNullOrWhiteSpace(action.NextScene))
            {
                return ActionValidationError("The action does not have a result or a next scene.", scene, action);
            }

            return null;
        }

        private static Exception AtLeastOneActionHasNextScene(Scene scene, List<SceneAction> actions)
        {
            if (actions.Count == 0)
            {
                return null;
            }

            var actionsWithNextScenes = actions.Where(action => !string.IsNullOrWhiteSpace(action.NextScene)).ToList();
            if (actionsWithNextScenes.Count < 1)
            {
                return
                    SceneValidationError(
                        "Actions were found, but none of them has a next scene. If a scene has actions, at least one of them must have a next scene.",
                        scene);
            }

            return null;
        }

        private static Exception DefaultActionIsValid(Scene scene, List<SceneAction> actions)
        {
            var defaultActions = actions.Count(action => string.IsNullOrWhiteSpace(action.Abbreviation));

            if (defaultActions > 1)
            {
                return
                    SceneValidationError(
                        "More than one blank action abbreviation was found. If a blank action abbreviation is used, it must be the only action.",
                        scene);
            }
            else if (defaultActions == 1 && actions.Count > 1)
            {
                return
                    SceneValidationError(
                        "A blank action abbreviation was found among other actions. If a blank action abbreviation is used, it must be the only action.",
                        scene);
            }

            return null;
        }

        private static Exception OneEntrySceneExists(IEnumerable<Scene> scenes)
        {
            var entryScenes = scenes.Where(scene => scene.SceneName.Trim() == string.Empty).ToList();
            
            if (entryScenes.Count == 0)
            {
                const string errorMessage =
                    "You haven't created any entry scenes. STAM doesn't know where to begin the adventure. An entry scene is a scene without a name.";
                return GeneralValidationError(errorMessage);
            }
            else if (entryScenes.Count > 1)
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine(
                    "You have more than one entry scene. STAM doesn't know where to begin the adventure. An entry scene is a scene without a name.");
                errorMessage.AppendLine("Entry scenes found in:");

                // Include each file that has an entry scene in the error readout
                foreach (var scene in entryScenes)
                {
                    errorMessage.AppendLine(scene.FilePath);
                }

                return GeneralValidationError(errorMessage.ToString());
            }

            return null;
        }

        private static Exception SceneHasDescription(Scene scene)
        {
            var sceneDescription = SceneParseHelper.GetSceneDescription(scene.Text);

            if (string.IsNullOrWhiteSpace(sceneDescription))
            {
                return SceneValidationError("No scene description found.", scene);
            }

            return null;
        }

        private static Exception GeneralValidationError(string message)
        {
            return new Exception($"ERROR: {message}");
        }

        private static Exception SceneValidationError(string message, Scene errorScene)
        {
            return new Exception($"ERROR in '{errorScene.FilePath}', scene '{errorScene.SceneName}': {message}");
        }

        private static Exception ActionValidationError(string message, Scene errorScene, SceneAction errorAction)
        {
            return
                new Exception(
                    $"ERROR in '{errorScene.FilePath}', scene '{errorScene.SceneName}, action '{errorAction.Abbreviation}': {message}");
        }
    }
}
