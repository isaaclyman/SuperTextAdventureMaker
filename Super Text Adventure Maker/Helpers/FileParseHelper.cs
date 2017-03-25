using System;
using System.Collections.Generic;
using System.Linq;

namespace Super_Text_Adventure_Maker.Helpers
{
    public static class FileParseHelper
    {
        // Given the text content of a file, returns a list of unstructured scene blocks
        public static List<string> SplitByScene(string text)
        {
            return
                text.Split(new[] {">>>"}, StringSplitOptions.RemoveEmptyEntries)
                    .Where(scene => !string.IsNullOrWhiteSpace(scene))
                    .ToList();
        }
    }
}
