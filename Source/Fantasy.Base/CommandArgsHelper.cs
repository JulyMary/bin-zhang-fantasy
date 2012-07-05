using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Fantasy
{
    public static class CommandArgumentsHelper
    {
        private const string REGEX_CMD = "(/|-)(?<name>\\w+)(:(?<value>.*))?";

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _arguments;

        public static  System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> Arguments
        {
            get { return _arguments; }
        }

        static CommandArgumentsHelper()
        {
            _arguments = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>(StringComparer.OrdinalIgnoreCase);

            InitializeArgPairs();
        }

        public static bool HasArgument(string name)
        {
            return _arguments.ContainsKey(name);
        }

        public static string GetValue(string name)
        {
            string rs = null;
            List<string> values;
            if (_arguments.TryGetValue(name, out values))
            {
                if (values.Count > 0)
                {
                    rs = values[0];
                }
            }
            return rs;

        }

        public static string[] GetValues(string name)
        {
            string[] rs = new string[0];
            List<string> values;
            if (_arguments.TryGetValue(name, out values))
            {
                if (values.Count > 0)
                {
                    rs = values.ToArray();
                }
            }
            return rs;
        }

      

        public  static bool TryGetArgumentValue(string name, out string value)
        {
            bool rs = false;
            value = null;
            List<string> values;
            if (_arguments.TryGetValue(name, out values))
            {
                if (values.Count > 0)
                {
                    rs = true;
                    value = values[0];
                }
            }
            return rs;
        }

        private static void InitializeArgPairs()
        {
            Regex regex = new System.Text.RegularExpressions.Regex(REGEX_CMD);

            foreach (string argument in Environment.GetCommandLineArgs())
            {
                Match match = regex.Match(argument);

                if (match.Success)
                {
                    string name = match.Groups["name"].Value;
                    System.Collections.Generic.List<string> values;
                    if (!_arguments.TryGetValue(name, out values))
                    {
                        values = new System.Collections.Generic.List<string>();
                        _arguments.Add(name, values);
                    }
                    string value = match.Groups["value"].Value.Trim('"', '\'');
                    values.Add(value);
                }
            }

        }

    }
}
