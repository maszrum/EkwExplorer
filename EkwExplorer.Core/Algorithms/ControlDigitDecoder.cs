using System;
using System.Collections.Generic;
using EkwExplorer.Core.Models;

namespace EkwExplorer.Core.Algorithms
{
    public class ControlDigitDecoder
    {
        private readonly int[] _weights = new[] { 1, 3, 7, 1, 3, 7, 1, 3, 7, 1, 3, 7 };

        private readonly IReadOnlyDictionary<char, int> _decodingValues = new Dictionary<char, int>()
        {
            ['0'] = 0,
            ['1'] = 1,
            ['2'] = 2,
            ['3'] = 3,
            ['4'] = 4,
            ['5'] = 5,
            ['6'] = 6,
            ['7'] = 7,
            ['8'] = 8,
            ['9'] = 9,
            ['X'] = 10,
            ['A'] = 11,
            ['B'] = 12,
            ['C'] = 13,
            ['D'] = 14,
            ['E'] = 15,
            ['F'] = 16,
            ['G'] = 17,
            ['H'] = 18,
            ['I'] = 19,
            ['J'] = 20,
            ['K'] = 21,
            ['L'] = 22,
            ['M'] = 23,
            ['N'] = 24,
            ['O'] = 25,
            ['P'] = 26,
            ['R'] = 27,
            ['S'] = 28,
            ['T'] = 29,
            ['U'] = 30,
            ['W'] = 31,
            ['Y'] = 32,
            ['Z'] = 33
        };

        public int Decode(BookNumber bookNumber)
        {
            if (bookNumber.Length != _weights.Length)
            {
                throw new ArgumentException(
                    $"length of code ({bookNumber.Length}) is different than weights length ({_weights.Length})", nameof(bookNumber));
            }

            var result = 0;
            for (var i = 0; i < bookNumber.CourtCode.Length; i++)
            {
                var ch = bookNumber.CourtCode[i];
                result += _decodingValues[ch] * _weights[i];
            }

            for (var i = bookNumber.CourtCode.Length; i < bookNumber.Length; i++)
            {
                var chIndex = i - bookNumber.CourtCode.Length;
                var ch = bookNumber.Number[chIndex];
                result += _decodingValues[ch] * _weights[i];
            }

            return result % 10;
        }
    }
}