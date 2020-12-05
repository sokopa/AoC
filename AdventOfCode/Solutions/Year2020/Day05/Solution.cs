using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day05 : ASolution
    {
        private List<string> _lines;
        private List<int> _seatIds;
        public Day05() : base(05, 2020, "")
        {
            _lines = Input.SplitByNewline().ToList();
            _seatIds = _lines.Select(CalculateSeatId).ToList();
        }

        protected override string SolvePartOne()
        {
            return _seatIds.Max().ToString();
        }

        private int CalculateSeatId(string line)
        {
            var row = line.Substring(0, 7);
            var rowCalculated = BinarySearch(0, 127, 'F', 'B', row);
            
            var col = line.Substring(7, 3);
            var colCalculated = BinarySearch(0, 7, 'L', 'R', col);

            return rowCalculated * 8 + colCalculated;
        }

        private int BinarySearch(int min, int max, char low, char high, string input)
        {
            int mid = (min + max) / 2;
            if (input.Length == 0) return min;
            var key = input[0];
            var rest = input.Substring(1);
            if (key == low) return BinarySearch(min, mid - 1, low, high, rest);
            if (key == high) return BinarySearch(mid + 1, max, low, high, rest);
            return 0;
        }

        protected override string SolvePartTwo()
        {
            var allPossibleSeats = Enumerable.Range(0, 127 * 8 + 7);
            var missingSeats = allPossibleSeats.Except(_seatIds).ToList();
            foreach (var seat in missingSeats)
            {
                var col = seat % 8;
                var row = (seat - col) / 8;
                if (row == 0 || row == 127) continue;
                if (_seatIds.Contains(seat - 1) && _seatIds.Contains(seat + 1)) return seat.ToString();
            }
            return "Not found";
        }
    }
}
