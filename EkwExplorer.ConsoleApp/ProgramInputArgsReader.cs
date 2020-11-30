using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EkwExplorer.ConsoleApp
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
            ReadArgs(args, (key, value) =>
            {
                var propertyName = CamelCaseToPascalCase(key);
                var property = typeof(ProgramInput).GetProperty(propertyName);

                if (property != null)
                {
                    var propertyType = property.PropertyType;

                    if (propertyType.Name == "Nullable`1")
                    {
                        propertyType = propertyType.GenericTypeArguments[0];
                    }

                    var convertedValue = ConvertArgValue(value, propertyType);

                    property.SetValue(_input, convertedValue);
                }
            });
        }

        private static void ReadArgs(IReadOnlyList<string> args, Action<string, string> onArg)
        {
            for (int i = 0; i < args.Count - 1; i++)
            {
                var argName = args[i];
                var argPotentialValue = args[i + 1];

                if (argName.StartsWith("--") && !argPotentialValue.StartsWith("--"))
                {
                    argName = argName.Substring(2);
                    var argValue = argPotentialValue;

                    onArg(argName, argValue);
                }
            }
        }

        private static string CamelCaseToPascalCase(string input)
        {
            var parts = input
                .ToLower()
                .Split('-')
                .Where(part => part.Length > 0)
                .Select(part =>
                {
                    var firstChar = part[0];
                    if (char.IsLetter(firstChar))
                    {
                        return string.Concat(char.ToUpper(firstChar), part.Remove(0, 1));
                    }
                    return part;
                })
                .ToArray();

            return string.Concat(parts);
        }

        private static readonly IReadOnlyDictionary<Type, Func<string, object>> Converters =
            new Dictionary<Type, Func<string, object>>()
            {
                [typeof(int)] = input => int.Parse(input, CultureInfo.InvariantCulture.NumberFormat),
                [typeof(string)]  = input => input,
                [typeof(bool)] = input => input == "1" || input.ToLower() == "true",
                [typeof(float)] = input => float.Parse(input, CultureInfo.InvariantCulture.NumberFormat),
                [typeof(double)] = input => double.Parse(input, CultureInfo.InvariantCulture.NumberFormat)
            };

        private static object ConvertArgValue(string input, Type toType)
        {
            if (!Converters.TryGetValue(toType, out var converter))
            {
                throw new TypeLoadException(
                    $"cannot parse argument of type {toType.Name} in {nameof(ProgramInput)}");
            }

            return converter(input);
        }
    }
}