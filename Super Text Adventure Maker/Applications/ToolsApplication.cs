using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Super_Text_Adventure_Maker.Configuration;
using Super_Text_Adventure_Maker.DTOs;
using Super_Text_Adventure_Maker.FileSystem;
using Super_Text_Adventure_Maker.Parsing;
using Super_Text_Adventure_Maker.UserInterface;
using Super_Text_Adventure_Maker.Validation;

namespace Super_Text_Adventure_Maker.Applications
{
    public class ToolsApplication
    {
        public static void Initialize()
        {
            UserInterfaceHelper.SetTitle(Strings.Tools_Title);

            UserInterfaceHelper.OutputLine(Strings.Tools_Welcome);
            UserInterfaceHelper.OutputLine();
            ShowMenu(false);
        }

        private static string ChoosePackage(List<string> packages)
        {
            if (packages.Count < 1)
            {
                UserInterfaceHelper.OutputLine(Strings.Tools_NoPackageFound);
                UserInterfaceHelper.Pause();
                return null;
            }

            if (packages.Count == 1)
            {
                UserInterfaceHelper.OutputLine(string.Format(Strings.Tools_LoadingOnlyPackage, packages.First()));
                UserInterfaceHelper.Pause();
                return packages.First();
            }

            UserInterfaceHelper.OutputLine();
            UserInterfaceHelper.OutputLine(Strings.Tools_ChoosePackage);
            UserInterfaceHelper.OutputLine();

            for (var index = 0; index < packages.Count; index++)
            {
                UserInterfaceHelper.OutputLine($"({index + 1}) {packages[index]}");
            }

            UserInterfaceHelper.OutputLine();
            int choice;
            var choiceIsInt = int.TryParse(UserInterfaceHelper.GetInput(), out choice);

            if (choiceIsInt && choice > 0 && choice <= packages.Count)
            {
                return packages[choice];
            }

            // If an invalid number was chosen, show an error and try again.
            UserInterfaceHelper.OutputLine(Strings.Tools_InvalidEntry);
            UserInterfaceHelper.Pause();
            return ChoosePackage(packages);
        }

        private static List<StamFile> ChooseProject(Dictionary<string, List<StamFile>> projects)
        {
            if (projects.Count < 1)
            {
                UserInterfaceHelper.OutputLine(Strings.Tools_NoProjectFound);
                UserInterfaceHelper.Pause();
                return null;
            }

            var projectNames = projects.Keys.ToArray();
            if (projectNames.Length == 1)
            {
                return projects[projectNames.First()];
            }

            UserInterfaceHelper.OutputLine();
            UserInterfaceHelper.OutputLine(Strings.Tools_ChooseProject);
            UserInterfaceHelper.OutputLine();

            for (var index = 0; index < projectNames.Length; index++)
            {
                UserInterfaceHelper.OutputLine($"({index + 1}) {projectNames[index]}");
            }

            UserInterfaceHelper.OutputLine();
            int choice;
            var choiceIsInt = int.TryParse(UserInterfaceHelper.GetInput(), out choice);

            if (choiceIsInt && choice > 0 && choice <= projectNames.Length)
            {
                return projects[projectNames[choice - 1]];
            }
            
            // If an invalid number was chosen, show an error and try again.
            UserInterfaceHelper.OutputLine(Strings.Tools_InvalidEntry);
            UserInterfaceHelper.Pause();
            return ChooseProject(projects);
        }

        private static string ChooseSaveGame(List<string> saveGames)
        {
            if (saveGames.Count < 1)
            {
                UserInterfaceHelper.OutputLine(Strings.Tools_NoSaveGameFound);
                UserInterfaceHelper.Pause();
                return null;
            }

            if (saveGames.Count == 1)
            {
                return saveGames.First();
            }

            UserInterfaceHelper.OutputLine();
            UserInterfaceHelper.OutputLine(Strings.Tools_ChooseSaveGame);
            UserInterfaceHelper.OutputLine();

            for (var index = 0; index < saveGames.Count; index++)
            {
                UserInterfaceHelper.OutputLine($"({index + 1}) {saveGames[index]}");
            }

            UserInterfaceHelper.OutputLine();
            int choice;
            var choiceIsInt = int.TryParse(UserInterfaceHelper.GetInput(), out choice);

            if (choiceIsInt && choice > 0 && choice <= saveGames.Count)
            {
                return saveGames[choice - 1];
            }

            // If an invalid number was chosen, show an error and try again.
            UserInterfaceHelper.OutputLine(Strings.Tools_InvalidEntry);
            UserInterfaceHelper.Pause();
            return ChooseSaveGame(saveGames);
        }

        private static void CreatePackage()
        {
            var projects = FileSystemHelper.GetStamProjects();
            var files = ChooseProject(projects);
            if (files == null)
            {
                ShowMenu();
                return;
            }

            var isValid = ValidationHelper.ValidateFiles(files);

            if (!isValid)
            {
                ShowMenu();
                return;
            }

            var scenes = FileParseHelper.GetScenes(files).ToList();

            FileSystemHelper.WritePackage(scenes);
            ShowMenu();
        }

        private static void LoadSaved()
        {
            var saveGames = FileSystemHelper.SearchSaveGames(FileSystemHelper.GetCurrentPath()).ToList();
            var path = ChooseSaveGame(saveGames);
            if (path == null)
            {
                ShowMenu();
                return;
            }

            var env = FileSystemHelper.ReadSavedGame(path);
            GameApplication.Init(env.AllScenes, env.PackageName, env.CurrentScene);
            ShowMenu();
        }

        private static void PlayPackage()
        {
            var packages = FileSystemHelper.SearchPackages(FileSystemHelper.GetCurrentPath()).ToList();
            var path = ChoosePackage(packages);
            if (path == null)
            {
                ShowMenu();
                return;
            }

            var scenes = FileSystemHelper.ReadPackage(path);

            GameApplication.Init(scenes, path);
            ShowMenu();
        }

        private static void ShowHelp()
        {
            UserInterfaceHelper.OutputLine(Strings.Tools_OpeningWebHelp);
            Process.Start("https://github.com/isaaclyman/SuperTextAdventureMaker/wiki");
            UserInterfaceHelper.Pause();
            ShowMenu();
        }

        private static void ShowMenu(bool clearWindow = true)
        {
            if (clearWindow)
            {
                UserInterfaceHelper.ClearWindow();
            }
            
            UserInterfaceHelper.OutputLine(Strings.Tools_WhatToDo);
            UserInterfaceHelper.OutputLine(Strings.Tools_Menu);
            var selection = UserInterfaceHelper.GetNextKey();

            // (?) for help, (q) to quit, (p) to play a game package, (l) to load a saved game,
            // (v) to validate a project, (b) to build / test a project, (k) to package up a finished project

            switch (selection)
            {
                case '?':
                    ShowHelp();
                    break;
                case 'q':
                    return;
                case 'p':
                    PlayPackage();
                    break;
                case 'l':
                    LoadSaved();
                    break;
                case 'v':
                    ValidateProject();
                    break;
                case 'b':
                    TestProject();
                    break;
                case 'k':
                    CreatePackage();
                    break;
                default:
                    UnrecognizedOption();
                    break;
            }
        }
        
        private static void TestProject()
        {
            var projects = FileSystemHelper.GetStamProjects();
            var files = ChooseProject(projects);
            if (files == null)
            {
                ShowMenu();
                return;
            }

            var isValid = ValidationHelper.ValidateFiles(files);

            if (!isValid)
            {
                ShowMenu();
                return;
            }

            var scenes = FileParseHelper.GetScenes(files).ToList();
            GameApplication.Init(scenes, null);
            ShowMenu();
        }

        private static void ValidateProject()
        {
            var projects = FileSystemHelper.GetStamProjects();
            var files = ChooseProject(projects);
            if (files == null)
            {
                ShowMenu();
                return;
            }

            ValidationHelper.ValidateFiles(files);
            ShowMenu();
        }

        private static void UnrecognizedOption()
        {
            UserInterfaceHelper.OutputLine(Strings.Tools_Unrecognized);
            UserInterfaceHelper.Pause();
            ShowMenu();
        }
    }
}
