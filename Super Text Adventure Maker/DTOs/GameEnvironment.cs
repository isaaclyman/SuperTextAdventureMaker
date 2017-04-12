using System.Collections.Generic;

namespace Super_Text_Adventure_Maker.DTOs
{
    public class GameEnvironment
    {
        public List<Scene> AllScenes { get; set; }
        public Scene CurrentScene { get; set; }
    }
}
