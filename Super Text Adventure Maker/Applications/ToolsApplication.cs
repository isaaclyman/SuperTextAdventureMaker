﻿using System.Collections.Generic;
using System.Linq;
using Super_Text_Adventure_Maker.Configuration;
using Super_Text_Adventure_Maker.DTOs;
using Super_Text_Adventure_Maker.Parsing;
using Super_Text_Adventure_Maker.UserInterface;
using Super_Text_Adventure_Maker.Validation;

namespace Super_Text_Adventure_Maker.Applications
{
    public class ToolsApplication
    {
        public static void Initialize()
        {
            UserInterfaceHelper.OutputLine(Strings.Tools_Welcome);
            UserInterfaceHelper.OutputLine();
            ShowMenu(false);
        }

        private static List<StamFile> ChooseProject(Dictionary<string, List<StamFile>> projects)
        {
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
            UserInterfaceHelper.OutputLine(Strings.General_ComingSoon);
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
            UserInterfaceHelper.OutputLine(Strings.General_ComingSoon);
            UserInterfaceHelper.Pause();
            ShowMenu();
        }

        private static void ShowHelp()
        {
            UserInterfaceHelper.OutputLine(Strings.General_ComingSoon);
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
