using System;
using System.Collections.Generic;

namespace EkwClicker
{
    internal class ProgramInputArgsReader
    {
        private readonly ProgramInput _input;

        public ProgramInputArgsReader(ProgramInput input)
        {
            _input = input;
        }

        public void Read(IReadOnlyList<string> args)
        {
            _input.CourtCode = string.Empty;
            throw new NotImplementedException();
        }
    }
}