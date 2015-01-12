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
using System.Data;
using System.Text.RegularExpressions;

namespace MacroStaticAnalyzer.Business
{
    public class Parser
    {
        private readonly Dictionary<string, Macro> _macros;
        private LexicalAnalyzer _lexer;

        public Parser(Dictionary<string, Macro> macros)
        {
            _macros = macros;
            _lexer = new LexicalAnalyzer();
        }

        public void Parse(Macro macro)
        {
            _lexer.Load(macro.Code);

            Token token = _lexer.Read();
            // Skip codes before "package" like "namespace shs_sat"
            while (token.ToString() != "package")
            {
                token = _lexer.Read();
            }

            string macroType = _lexer.Read().ToString();
            string macroName = _lexer.Read().ToString();
            Match("{");
            MatchFunction();
            Match("}");
            //            var statementPattern = @"(?<body>.*;)";
            //            var stmtMatches = Regex.Matches(function.Body, statementPattern);
            //            foreach (Match stmtMatch in stmtMatches)
            //            {
            //                var stmtBody = stmtMatch.Groups["body"].Value.Trim('\t').Trim(' ');
            //                //                var commentPattern = @"//.*";
            //                //                var printlnPattern = @"println\s+\(.*.);";
            //                var intAssignmentPattern = @"int\s+\w+\s+;";
            //                var boolAssignmentPattern = @"bool\s+\w+\s+;";
            //                var returnPattern = @"return\s+w+;";
            //                if (Regex.IsMatch(stmtBody, assignmentPattern))
            //                {
            //                    continue;
            //                }
            //                if (Regex.IsMatch(stmtBody, returnPattern))
            //                {
            //                    continue;
            //                }
            //            }
        }

        private void MatchFunction()
        {
            throw new System.NotImplementedException();
        }

        private void Match(string s)
        {
            if (_lexer.Read().ToString() != s)
            {
                throw new SyntaxErrorException();
            }
        }
    }
}