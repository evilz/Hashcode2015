using System;
using System.Collections.Generic;

namespace Kunai.ConsoleExtentions
{
    public enum ConsoleColorSet
    {
        Green,
        Blue,
        Cyan,
        Gray,
        Magenta,
        Red,
        Yellow
    }


    public static class ConsoleExtensions
    {
        private static readonly Dictionary<ConsoleColorSet,ConsoleColor[]> _dicoColorSet = new Dictionary<ConsoleColorSet, ConsoleColor[]>
        {
            {ConsoleColorSet.Green, new []{ ConsoleColor.Green,ConsoleColor.DarkGreen}},
            {ConsoleColorSet.Blue, new []{ ConsoleColor.Blue,ConsoleColor.DarkBlue}},
            {ConsoleColorSet.Cyan, new []{ ConsoleColor.Cyan,ConsoleColor.DarkCyan}},
            {ConsoleColorSet.Gray, new []{ ConsoleColor.Gray,ConsoleColor.DarkGray}},
            {ConsoleColorSet.Magenta, new []{ ConsoleColor.Magenta,ConsoleColor.DarkMagenta}},
            {ConsoleColorSet.Red, new []{ ConsoleColor.Magenta,ConsoleColor.DarkRed}},
            {ConsoleColorSet.Yellow, new []{ ConsoleColor.Yellow,ConsoleColor.DarkYellow}},
        };

        public static ConsoleColor LightColor(this ConsoleColorSet colorSet)
        {
            return _dicoColorSet[colorSet][0];
        }

        public static ConsoleColor DarkColor(this ConsoleColorSet colorSet)
        {
            return _dicoColorSet[colorSet][1];
        }

        // add logger

        // add progress


        // TODO MAKE UNIT TEST !
        public static void ProgressBar(int complete, int maxVal, int barSize, ConsoleColorSet colorSet = ConsoleColorSet.Green, char progressCharacter = '█' )
        {
            Console.CursorVisible = false;
            var left = Console.CursorLeft;
            var perc = (decimal)complete / (decimal)maxVal;
            var chars = 0;
            if (barSize > 0)
                chars = (int)Math.Floor(perc / ((decimal)1 / (decimal)barSize));
            string p1 = String.Empty, p2 = String.Empty;

            for (int i = 0; i < chars; i++) p1 += progressCharacter;
            for (int i = 0; i < barSize - chars; i++) p2 += progressCharacter;

            Console.ForegroundColor = colorSet.LightColor();
            Console.Write(p1);
            Console.ForegroundColor = colorSet.DarkColor();
            Console.Write(p2);

            Console.ResetColor();
            Console.Write(" {0}%", (perc * 100).ToString("N2"));
            Console.CursorLeft = left;
        }

        // add spine


    }
}
