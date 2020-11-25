using System;

namespace EkwExplorer
{
    internal class ProgramInputJsonReader
    {
        private readonly ProgramInput _input;

        public ProgramInputJsonReader(ProgramInput input)
        {
            _input = input;
        }

        public void Read(string fileName)
        {
            _input.CourtCode = string.Empty;
            throw new NotImplementedException();
        }
    }
}