using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Super_Text_Adventure_Maker.Configuration;
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
                UserInterfaceHelper.OutputLine(Strings.Validation_NoErrors);
                return;
            }

            foreach (var ex in exceptions)
            {
                UserInterfaceHelper.OutputError(ex);
                UserInterfaceHelper.OutputLine();
            }

            UserInterfaceHelper.OutputError(new Exception(Strings.Validation_ErrorCannotBuild));
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

                if (actions.Count <= 0)
                {
                    continue;
                }

                exceptions.Add(AllNextScenesExist(scenes, actions));
                exceptions.Add(AtLeastOneActionHasNextScene(scene, actions));
                exceptions.Add(DefaultActionIsValid(scene, actions));
                exceptions.Add(NoDuplicateActionAbbreviations(scene, actions));
                exceptions.Add(NoDuplicateActionDescriptions(scene, actions));
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
                return ActionValidationError(Strings.Validation_ErrorNoActionDescription, scene, action);
            }

            return null;
        }

        private static Exception ActionHasResultOrNextScene(Scene scene, SceneAction action)
        {
            if (string.IsNullOrWhiteSpace(action.Result) && string.IsNullOrWhiteSpace(action.NextScene))
            {
                return ActionValidationError(Strings.Validation_ErrorNoActionResultOrScene, scene, action);
            }

            return null;
        }

        private static Exception AllNextScenesExist(IEnumerable<Scene> scenes, IEnumerable<SceneAction> sceneActions)
        {
            var sceneNames = scenes.Select(scene => scene.Name);

            var actionsWithNonexistentNextScenes =
                sceneActions.Where(
                        sceneAction =>
                            !string.IsNullOrWhiteSpace(sceneAction.NextScene) && !sceneNames.Contains(sceneAction.NextScene))
                    .ToList();

            if (actionsWithNonexistentNextScenes.Count <= 0)
            {
                return null;
            }

            var errorMessage = new StringBuilder();
            errorMessage.AppendLine(Strings.Validation_ErrorNonExistentScenes);
            errorMessage.AppendLine(Strings.Validation_ScenesNotFound);

            foreach (var action in actionsWithNonexistentNextScenes)
            {
                errorMessage.AppendLine($"'{action.Scene.FilePath}'");
                errorMessage.AppendLine($"{Strings.General_Scene} '{action.Scene.Name}'");
                errorMessage.AppendLine($"{Strings.General_Action} '{action.Abbreviation}'");
            }

            return GeneralValidationError(errorMessage.ToString());
        }

        private static Exception AtLeastOneActionHasNextScene(Scene scene, List<SceneAction> actions)
        {
            var actionsWithNextScenes = actions.Where(action => !string.IsNullOrWhiteSpace(action.NextScene)).ToList();
            if (actionsWithNextScenes.Count < 1)
            {
                return SceneValidationError(Strings.Validation_ErrorNoNextScene, scene);
            }

            return null;
        }

        private static Exception DefaultActionIsValid(Scene scene, List<SceneAction> actions)
        {
            var defaultActions = actions.Count(action => string.IsNullOrWhiteSpace(action.Abbreviation));

            if (defaultActions > 1)
            {
                return SceneValidationError(Strings.Validation_ErrorMoreThanOneDefaultAction, scene);
            }
            else if (defaultActions == 1 && actions.Count > 1)
            {
                return SceneValidationError(Strings.Validation_ErrorDefaultActionNotIsolated, scene);
            }

            return null;
        }

        private static Exception NoDuplicateActionAbbreviations(Scene scene, List<SceneAction> actions)
        {
            var duplicateAbbreviations =
                actions.Where(action => !string.IsNullOrWhiteSpace(action.Abbreviation))
                    .GroupBy(action => action.Abbreviation)
                    .Where(group => group.Count() > 1)
                    .ToList();

            if (duplicateAbbreviations.Count <= 0)
            {
                return null;
            }

            var errorMessage = new StringBuilder();
            if (duplicateAbbreviations.Count > 0)
            {
                errorMessage.AppendLine(Strings.Validation_ErrorDuplicateAbbreviation);
                errorMessage.AppendLine(Strings.Validation_DuplicateAbbreviations);
                errorMessage.Append(string.Join(Environment.NewLine,
                    duplicateAbbreviations.Select(group => group.First().Abbreviation)));
            }

            return SceneValidationError(errorMessage.ToString(), scene);
        }

        private static Exception NoDuplicateActionDescriptions(Scene scene, List<SceneAction> actions)
        {

            var duplicateDescriptions =
                actions.Where(action => !string.IsNullOrWhiteSpace(action.Description))
                    .GroupBy(action => action.Description)
                    .Where(group => group.Count() > 1)
                    .ToList();

            if (duplicateDescriptions.Count <= 0)
            {
                return null;
            }

            var errorMessage = new StringBuilder();
            if (duplicateDescriptions.Count > 0)
            {
                errorMessage.AppendLine(Strings.Validation_ErrorDuplicateActionDescriptions);
                errorMessage.AppendLine(Strings.Validation_DuplicateActionDescriptions);
                errorMessage.Append(string.Join(Environment.NewLine,
                    duplicateDescriptions.Select(group => group.First().Description)));
            }

            return SceneValidationError(errorMessage.ToString(), scene);
        }

        private static Exception NoDuplicateScenesExist(IEnumerable<Scene> scenes)
        {
            var duplicateScenes = scenes.GroupBy(scene => scene.Name).Where(group => group.Count() > 1).ToList();

            if (duplicateScenes.Count > 0)
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine(Strings.Validation_ErrorDuplicateSceneNames);
                errorMessage.AppendLine(Strings.Validation_DuplicateSceneNames);

                foreach (var sceneGroup in duplicateScenes)
                {
                    foreach (var scene in sceneGroup)
                    {
                        errorMessage.AppendLine($"'{scene.FilePath}'");
                        errorMessage.AppendLine($"{Strings.General_Scene} '{scene.Name}'");
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
                return GeneralValidationError(Strings.Validation_ErrorNoEntryScene);
            }
            else if (entryScenes.Count > 1)
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine(Strings.Validation_ErrorMultipleEntryScenes);
                errorMessage.AppendLine(Strings.Validation_EntryScenes);

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
                return SceneValidationError(Strings.Validation_ErrorInfiniteSceneLoop, scene);
            }

            return null;
        }

        private static Exception SceneHasDescription(Scene scene)
        {
            var sceneDescription = SceneParseHelper.GetSceneDescription(scene.Text);

            if (string.IsNullOrWhiteSpace(sceneDescription))
            {
                return SceneValidationError(Strings.Validation_ErrorNoSceneDescription, scene);
            }

            return null;
        }

        private static Exception GeneralValidationError(string message)
        {
            return new Exception($"{Strings.General_Error}: {message}");
        }

        private static Exception SceneValidationError(string message, Scene errorScene)
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine($"{Strings.General_Error}: {message}");
            errorMessage.AppendLine($"'{errorScene.FilePath}'");
            errorMessage.AppendLine($"{Strings.General_Scene} '{errorScene.Name}'");

            return new Exception(errorMessage.ToString());
        }

        private static Exception ActionValidationError(string message, Scene errorScene, SceneAction errorAction)
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine($"{Strings.General_Error}: {message}");
            errorMessage.AppendLine($"'{errorScene.FilePath}'");
            errorMessage.AppendLine($"{Strings.General_Scene} '{errorScene.Name}'");
            errorMessage.AppendLine($"{Strings.General_Action} '{errorAction.Abbreviation}'");

            return new Exception(errorMessage.ToString());
        }
    }
}
