using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Super_Text_Adventure_Maker.DTOs;
using Super_Text_Adventure_Maker.Parsing;
using Super_Text_Adventure_Maker.UserInterface;

namespace Super_Text_Adventure_Maker.Validation
{
    public static class ValidationHelper
    {
        public static void ValidateFiles(IEnumerable<StamFile> files)
        {
            var exceptions = GetExceptions(files);
            if (exceptions.Count <= 0)
            {
                return;
            }

            foreach (var ex in exceptions)
            {
                UserInterfaceHelper.OutputError(ex);
            }

            UserInterfaceHelper.OutputError(new Exception(
                "Your adventure could not be built because of the above errors. Please fix them and try again."));
        }

        private static List<Exception> GetExceptions(IEnumerable<StamFile> files)
        {
            var exceptions = new List<Exception>();
            var scenes = FileParseHelper.GetScenes(files).ToList();

            // Adventure-wide validation methods
            exceptions.Add(NoDuplicateScenesExist(scenes));
            exceptions.Add(OneEntrySceneExists(scenes));

            // Scene-specific validation methods
            foreach (var scene in scenes)
            {
                exceptions.Add(SceneHasDescription(scene));

                // Scene-wide action validation methods
                var actions = SceneParseHelper.GetSceneActions(scene).ToList();

                exceptions.Add(AllNextScenesExist(scenes, actions));
                exceptions.Add(AtLeastOneActionHasNextScene(scene, actions));
                exceptions.Add(DefaultActionIsValid(scene, actions));
                exceptions.Add(NoDuplicateActionsExistInScene(scene, actions));
                exceptions.Add(SceneIsNotEndlessLoop(scene, actions));

                // Action-specific validation methods
                foreach (var action in actions)
                {
                    exceptions.Add(ActionHasDescription(scene, action));
                    exceptions.Add(ActionHasResultOrNextScene(scene, action));
                }
            }

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

        private static Exception AllNextScenesExist(IEnumerable<Scene> scenes, IEnumerable<SceneAction> sceneActions)
        {
            var sceneNames = scenes.Select(scene => scene.Name);

            var actionsWithNonexistentNextScenes =
                sceneActions.Where(sceneAction => !sceneNames.Contains(sceneAction.NextScene)).ToList();

            if (actionsWithNonexistentNextScenes.Count > 0)
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine("Some of the next scenes used in actions do not exist.");
                errorMessage.AppendLine("Scenes not found:");

                foreach (var action in actionsWithNonexistentNextScenes)
                {
                    errorMessage.AppendLine(
                        $"File '{action.Scene.FilePath}, scene '{action.Scene.Name}, action '{action.Abbreviation}'");
                    return GeneralValidationError(errorMessage.ToString());
                }
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

        private static Exception NoDuplicateActionsExistInScene(Scene scene, List<SceneAction> actions)
        {
            var duplicateAbbreviations =
                actions.GroupBy(action => action.Abbreviation).Where(group => group.Count() > 1).ToList();
            var duplicateDescriptions =
                actions.GroupBy(action => action.Description).Where(group => group.Count() > 1).ToList();

            if (duplicateAbbreviations.Count <= 0 && duplicateDescriptions.Count <= 0)
            {
                return null;
            }

            var errorMessage = new StringBuilder();
            if (duplicateAbbreviations.Count > 0)
            {
                errorMessage.AppendLine(
                    "Duplicate action abbreviations were found in the scene. Each action should have a unique abbreviation.");
                errorMessage.AppendLine("Duplicate abbreviations:");

                foreach (var group in duplicateAbbreviations)
                {
                    var action = group.First();
                    errorMessage.AppendLine(action.Abbreviation);
                }
            }

            if (duplicateDescriptions.Count > 0)
            {
                errorMessage.AppendLine(
                    "Duplicate action descriptions were found in the scene. Each action should have a unique description.");
                errorMessage.AppendLine("Duplicate descriptions:");

                foreach (var group in duplicateDescriptions)
                {
                    var action = group.First();
                    errorMessage.AppendLine(action.Description);
                }
            }

            return SceneValidationError(errorMessage.ToString(), scene);
        }

        private static Exception NoDuplicateScenesExist(IEnumerable<Scene> scenes)
        {
            var duplicateScenes = scenes.GroupBy(scene => scene.Name).Where(group => group.Count() > 1).ToList();

            if (duplicateScenes.Count > 0)
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine("More than one scene has the same name. Each scene should have a unique name.");
                errorMessage.AppendLine("Duplicate scene names:");

                foreach (var sceneGroup in duplicateScenes)
                {
                    foreach (var scene in sceneGroup)
                    {
                        errorMessage.AppendLine($"File '{scene.FilePath}', scene '{scene.Name}'");
                    }
                }

                return GeneralValidationError(errorMessage.ToString());
            }
            
            return null;
        }

        private static Exception OneEntrySceneExists(IEnumerable<Scene> scenes)
        {
            var entryScenes = scenes.Where(scene => scene.Name.Trim() == string.Empty).ToList();
            
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

        private static Exception SceneIsNotEndlessLoop(Scene scene, List<SceneAction> actions)
        {
            if (actions.All(action => action.NextScene == scene.Name))
            {
                return SceneValidationError("The scene is an endless loop because all its actions use it as a next scene.", scene);
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
            return new Exception($"ERROR in '{errorScene.FilePath}', scene '{errorScene.Name}': {message}");
        }

        private static Exception ActionValidationError(string message, Scene errorScene, SceneAction errorAction)
        {
            return
                new Exception(
                    $"ERROR in '{errorScene.FilePath}', scene '{errorScene.Name}, action '{errorAction.Abbreviation}': {message}");
        }
    }
}
