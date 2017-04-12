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
            UserInterfaceHelper.SetTitle(Strings.Game_Title);

            var env = new GameEnvironment
            {
                AllScenes = scenes,
                CurrentScene = GetEntryScene(scenes)
            };
            PlayScene(env);
        }

        private static void ChooseAction(GameEnvironment env, Dictionary<string, SceneAction> actionDict)
        {
            SceneAction chosenAction;

            var actionChoice = UserInterfaceHelper.GetInput();
            if (actionChoice.StartsWith(":"))
            {
                RunUtility(env, actionChoice);
                return;
            }

            var actionExists = actionDict.TryGetValue(actionChoice, out chosenAction);

            if (!actionExists)
            {
                UserInterfaceHelper.OutputLine(Strings.Game_InvalidAction);
                UserInterfaceHelper.Pause();
                PlayScene(env);
                return;
            }

            ShowActionResult(chosenAction);

            if (!string.IsNullOrWhiteSpace(chosenAction.NextScene))
            {
                GoToScene(env, chosenAction.NextScene);
                return;
            }

            PlayScene(env);
        }

        private static void GameOver()
        {
            UserInterfaceHelper.Pause();
        }
        
        private static Scene GetEntryScene(IEnumerable<Scene> scenes)
        {
            return scenes.First(scene => string.IsNullOrWhiteSpace(scene.Name));
        }

        private static void GoToScene(GameEnvironment env, string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                return;
            }

            var nextScene =
                env.AllScenes.First(scene => string.Equals(scene.Name, sceneName, StringComparison.OrdinalIgnoreCase));
            env.CurrentScene = nextScene;
            PlayScene(env);
        }

        private static void PlayScene(GameEnvironment env)
        {
            UserInterfaceHelper.ClearWindow();

            UserInterfaceHelper.OutputLine(env.CurrentScene.Description);
            UserInterfaceHelper.OutputLine();

            var actions = SceneParseHelper.GetSceneActions(env.CurrentScene).ToList();
            PrepareActions(env, actions);
        }

        private static void PrepareActions(GameEnvironment env, List<SceneAction> actions)
        {
            if (actions.Count == 0)
            {
                GameOver();
                return;
            }

            if (actions.Count == 1)
            {
                var nextSceneName = actions.First().NextScene;
                GoToScene(env, nextSceneName);
                return;
            }

            UserInterfaceHelper.OutputLine(Strings.Game_WhatDoYouDo);
            UserInterfaceHelper.OutputLine(Strings.Game_Options);
            UserInterfaceHelper.OutputLine();

            foreach (var action in actions)
            {
                UserInterfaceHelper.OutputLine($"({action.Abbreviation}) {action.Description}");
            }

            var actionDict = actions.ToDictionary(action => action.Abbreviation);
            ChooseAction(env, actionDict);
        }

        private static void RunUtility(GameEnvironment env, string input)
        {
            var command = input.TrimStart(':');

            switch (command)
            {
                case "s":
                    // Save game
                    SaveGame(env.CurrentScene);
                    break;
                case "q":
                    // Quit game
                    return;
                case "r":
                    // Reset game
                    Init(env.AllScenes);
                    return;
                default:
                    UserInterfaceHelper.OutputLine(Strings.Game_InvalidOption);
                    UserInterfaceHelper.Pause();
                    PlayScene(env);
                    return;
            }
        }

        private static void SaveGame(Scene currentScene)
        {
            
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
