using System;
using System.Collections.Generic;
using System.Linq;
using Super_Text_Adventure_Maker.Configuration;
using Super_Text_Adventure_Maker.DTOs;
using Super_Text_Adventure_Maker.Parsing;
using Super_Text_Adventure_Maker.UserInterface;

namespace Super_Text_Adventure_Maker.Applications
{
    public static class GameApplication
    {
        public static void Init(List<Scene> scenes)
        {
            var entryScene = GetEntryScene(scenes);
            PlayScene(scenes, entryScene);
        }

        private static void ChooseAction(List<Scene> allScenes, Scene currentScene, Dictionary<string, SceneAction> actionDict)
        {
            SceneAction chosenAction;

            var actionChoice = UserInterfaceHelper.GetInput();
            var actionExists = actionDict.TryGetValue(actionChoice, out chosenAction);

            if (!actionExists)
            {
                UserInterfaceHelper.OutputLine(Strings.Game_InvalidAction);
                UserInterfaceHelper.Pause();
                PlayScene(allScenes, currentScene);
                return;
            }

            ShowActionResult(chosenAction);

            if (!string.IsNullOrWhiteSpace(chosenAction.NextScene))
            {
                GoToScene(allScenes, chosenAction.NextScene);
                return;
            }

            PlayScene(allScenes, currentScene);
        }

        private static void GameOver()
        {
            UserInterfaceHelper.Pause();
        }
        
        private static Scene GetEntryScene(IEnumerable<Scene> scenes)
        {
            return scenes.First(scene => string.IsNullOrWhiteSpace(scene.Name));
        }

        private static void GoToScene(List<Scene> allScenes, string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                return;
            }

            var nextScene =
                allScenes.First(scene => string.Equals(scene.Name, sceneName, StringComparison.OrdinalIgnoreCase));
            PlayScene(allScenes, nextScene);
        }

        private static void PlayScene(List<Scene> allScenes, Scene scene)
        {
            UserInterfaceHelper.ClearWindow();

            UserInterfaceHelper.OutputLine(scene.Description);
            UserInterfaceHelper.OutputLine();

            var actions = SceneParseHelper.GetSceneActions(scene).ToList();
            PrepareActions(allScenes, scene, actions);
        }

        private static void PrepareActions(List<Scene> allScenes, Scene currentScene, List<SceneAction> actions)
        {
            if (actions.Count == 0)
            {
                GameOver();
                return;
            }

            if (actions.Count == 1)
            {
                var nextSceneName = actions.First().NextScene;
                GoToScene(allScenes, nextSceneName);
                return;
            }

            UserInterfaceHelper.OutputLine(Strings.Game_WhatDoYouDo);
            UserInterfaceHelper.OutputLine();

            foreach (var action in actions)
            {
                UserInterfaceHelper.OutputLine($"({action.Abbreviation}) {action.Description}");
            }

            var actionDict = actions.ToDictionary(action => action.Abbreviation);
            ChooseAction(allScenes, currentScene, actionDict);
        }

        private static void ShowActionResult(SceneAction action)
        {
            if (string.IsNullOrWhiteSpace(action.Result))
            {
                return;
            }

            UserInterfaceHelper.OutputLine();
            UserInterfaceHelper.OutputLine(action.Result);
            UserInterfaceHelper.Pause();
        }
    }
}
