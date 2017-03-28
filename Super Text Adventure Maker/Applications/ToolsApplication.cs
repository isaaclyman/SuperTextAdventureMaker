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
        }
    }
}
