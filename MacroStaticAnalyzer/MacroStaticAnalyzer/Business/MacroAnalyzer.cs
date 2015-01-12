// ========================================================================
// Siemens Audiologische Technik GmbH
// Department H WS AU RD SW
// Gebbertstr. 125
// D-91058 Erlangen, Germany
// 
// Copyright (c) 2010
// ========================================================================
// /*------------------------- Revision-Info --------------------------------
// $HeaderUTC$	
// $NoKeywords$	
// *---------------------------------------------------------------------*/

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MacroStaticAnalyzer.Business
{
    public class MacroAnalyzer
    {
        public static void Main(string[] args)
        {
            var analyzer = new MacroAnalyzer();
            analyzer.Load(@"W:\Workspace\SIFIT-Templates7.4.0.IT54\HIData\HI\hiMacro\com.shs.d9PlatformMacros\models\");
            analyzer.Analyze();
        }

        private const string MacroNamePattern = @"package\s+(?<package>\w+)\s+(?<name>\w+)";
        private const string AppFuncPattern = @"application\s+function\s+(?<type>\w+)\s+(?<name>\w+)\s+\((?<param>.*)\)\s*\{(?<body>[.\s\S]*?)\}";
        private const string FuncParamPattern = @"(?<type>\w+)\s(?<name>\w+)";

        private readonly Dictionary<string, Macro> _macros = new Dictionary<string, Macro>();


        private void Load(string macroFolder)
        {
            foreach (string dir in Directory.GetDirectories(macroFolder))
            {
                Load(dir);
            }
            foreach (string file in Directory.GetFiles(macroFolder))
            {
                Preprocess(file);
            }
        }

        private void Analyze()
        {
            var lexer = new LexicalAnalyzer();
            foreach (var macro in _macros.Values)
            {
                lexer.Load(macro.Code);
                lexer.ReadToEnd();
            }
            //            var parser = new Parser(_macros);
            //            parser.Parse(_macros["InputModeDD"].Functions["SetPos"]);
        }

        private void Preprocess(string fileName)
        {
            var reader = new StreamReader(fileName);
            var content = reader.ReadToEnd();

            if (Regex.IsMatch(content, MacroNamePattern))
            {
                var macroMatch = Regex.Matches(content, MacroNamePattern)[0].Groups;
                var macro = new Macro
                {
                    Package = macroMatch["package"].Value,
                    Name = macroMatch["name"].Value,
                    FilePath = fileName,
                    Code = content,
                };

                //                var funcs = new Dictionary<string, Function>();
                //                foreach (Match match in Regex.Matches(content, AppFuncPattern))
                //                {
                //                    var funcParams = new List<FuncParameter>();
                //                    var paramsStr = match.Groups["param"].Value.Trim(' ');
                //                    if (!string.IsNullOrEmpty(paramsStr))
                //                    {
                //                        foreach (var str in paramsStr.Split(','))
                //                        {
                //                            var groups = Regex.Matches(str, FuncParamPattern)[0].Groups;
                //                            funcParams.Add(new FuncParameter
                //                            {
                //                                Name = groups["name"].Value,
                //                                Type = groups["type"].Value,
                //                            });
                //                        }
                //                    }
                //                    var function = new Function
                //                    {
                //                        Name = match.Groups["name"].Value,
                //                        Type = match.Groups["type"].Value,
                //                        Body = match.Groups["body"].Value,
                //                        Params = funcParams,
                //                    };
                //                    funcs[function.Name] = function;
                //                }

                //                macro.Functions = funcs;

                _macros[macro.Name] = macro;
            }
        }
    }

    public class Condition
    {
    }

    public class Macro
    {
        public string Package;
        public string Name;
        //        public Dictionary<string, Function> Functions;
        public string FilePath;
        public string Code;

        public override string ToString()
        {
            return Package + "." + Name;
        }
    }

    //    public class Function
    //    {
    //        public string Type;
    //        public string Name;
    //        public List<FuncParameter> Params;
    //        public string Body;
    //    }

    //    public class FuncParameter
    //    {
    //        public string Type;
    //        public string Name;
    //        public object Value;
    //    }
}