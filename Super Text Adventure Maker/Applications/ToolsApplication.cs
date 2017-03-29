using Super_Text_Adventure_Maker.Configuration;
using Super_Text_Adventure_Maker.UserInterface;

namespace Super_Text_Adventure_Maker.Applications
{
    public class ToolsApplication
    {
        public static void Initialize()
        {
            UserInterfaceHelper.OutputLine(Strings.Tools_Welcome);
            UserInterfaceHelper.OutputLine();
            ShowMenu();
        }

        private static void CreatePackage()
        {
            UserInterfaceHelper.OutputLine(Strings.General_ComingSoon);
            UserInterfaceHelper.Pause();
            UserInterfaceHelper.ClearWindow();
            ShowMenu();
        }

        private static void LoadSaved()
        {
            UserInterfaceHelper.OutputLine(Strings.General_ComingSoon);
            UserInterfaceHelper.Pause();
            UserInterfaceHelper.ClearWindow();
            ShowMenu();
        }

        private static void PlayPackage()
        {
            UserInterfaceHelper.OutputLine(Strings.General_ComingSoon);
            UserInterfaceHelper.Pause();
            UserInterfaceHelper.ClearWindow();
            ShowMenu();
        }

        private static void ShowHelp()
        {
            UserInterfaceHelper.OutputLine(Strings.General_ComingSoon);
            UserInterfaceHelper.Pause();
            UserInterfaceHelper.ClearWindow();
            ShowMenu();
        }

        private static void ShowMenu()
        {
            UserInterfaceHelper.OutputLine(Strings.Tools_WhatToDo);
            UserInterfaceHelper.OutputLine(Strings.Tools_Menu);
            var selection = UserInterfaceHelper.GetNextKey();

            // (?) for help, (p) to play a game package, (l) to load a saved game,
            // (v) to validate a project, (b) to build / test a project, (k) to package up a finished project

            switch (selection)
            {
                case '?':
                    ShowHelp();
                    break;
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
            UserInterfaceHelper.OutputLine(Strings.General_ComingSoon);
            UserInterfaceHelper.Pause();
            UserInterfaceHelper.ClearWindow();
            ShowMenu();
        }

        private static void ValidateProject()
        {
            UserInterfaceHelper.OutputLine(Strings.General_ComingSoon);
            UserInterfaceHelper.Pause();
            UserInterfaceHelper.ClearWindow();
            ShowMenu();
        }

        private static void UnrecognizedOption()
        {
            UserInterfaceHelper.OutputLine(Strings.Tools_Unrecognized);
            UserInterfaceHelper.Pause();
            UserInterfaceHelper.ClearWindow();
            ShowMenu();
        }
    }
}
