using System.IO;
using Newtonsoft.Json;

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
            var fileContents = File.ReadAllText(fileName);

            JsonConvert.PopulateObject(fileContents, _input);
        }
    }
}