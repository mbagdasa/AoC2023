using AoC2023.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023
{
    internal abstract class DayBase
    {
        protected List<string> _textInput = new();
        public DayBase(int day)
        {
            _textInput = FetchPuzzleInput.GetPuzzleAsList(day);
        }

        public abstract int SolveProblem_1();

        public abstract int SolveProblem_2();
    }
}
