using System.Collections.Generic;

namespace EkwExplorer
{
    internal static class ProgramInputExtensions
    {
        public static ProgramInput ReadFromConsole(this ProgramInput programInput)
        {
            var reader = new ProgramInputConsoleReader(programInput);
            reader.Read();
            
            return programInput;
        }
        
        public static ProgramInput ReadFromArgs(this ProgramInput programInput, IReadOnlyList<string> args)
        {
            var reader = new ProgramInputArgsReader(programInput);
            reader.Read(args);
            
            return programInput;
        }
        
        public static ProgramInput ReadFromJson(this ProgramInput programInput, string fileName)
        {
            var reader = new ProgramInputJsonReader(programInput);
            reader.Read(fileName);
            
            return programInput;
        }
    }
}