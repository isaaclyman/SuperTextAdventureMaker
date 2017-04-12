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

            UserInterfaceHelper.OutputLine(Strings.Tools_EnterPackageName);
            var filename = UserInterfaceHelper.GetInput().Trim().TrimStart('/', '\\');
            var path = Path.Combine(FileSystemHelper.GetCurrentPath(), filename);
            FileSystemHelper.WritePackage(scenes, path);
            UserInterfaceHelper.OutputLine(Strings.General_Done);
            UserInterfaceHelper.Pause();

            ShowMenu();
        }

        private static void LoadSaved()
        {
            UserInterfaceHelper.OutputLine(Strings.General_ComingSoon);
            UserInterfaceHelper.Pause();
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

            GameApplication.Init(scenes);
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
            GameApplication.Init(scenes);
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
