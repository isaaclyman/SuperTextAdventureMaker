using Super_Text_Adventure_Maker.Applications;

namespace Super_Text_Adventure_Maker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Detect if a game package was passed to the app
            //  If yes, parse and run the game package
            //  If no, pass control to the Tools application

            ToolsApplication.Initialize();
        }
    }
}
