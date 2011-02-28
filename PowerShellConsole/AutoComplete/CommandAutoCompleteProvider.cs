/*
   Copyright (c) 2011 Code Owls LLC, All Rights Reserved.

   Licensed under the Microsoft Reciprocal License (Ms-RL) (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.opensource.org/licenses/ms-rl

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CodeOwls.PowerShell.WinForms.Executors;
using CodeOwls.PowerShell.WinForms.Utility;

namespace CodeOwls.PowerShell.WinForms.AutoComplete
{
    class CommandAutoCompleteProvider : IAutoCompleteProvider
    {
        public class FormattedGuessInformation
        {
            public string Guess { get; private set; }
            public string CommandFormatString { get; private set; }
            public FormattedGuessInformation( string guess, string commandFormat )
            {
                Guess = guess;
                CommandFormatString = commandFormat;
            }
        }
        private readonly string _commandTemplate;
        private readonly Executor _executor;

        protected CommandAutoCompleteProvider(string commandTemplate, Executor executor)
        {
            _commandTemplate = commandTemplate;
            _executor = executor;
        }

        protected virtual string GetCommand( FormattedGuessInformation info )
        {
            return String.Format(_commandTemplate, info.Guess);
        }

        protected string[] BreakIntoWords( string guess )
        {
            return Regex.Split(guess.Trim(), @"\s+");
        }
        protected virtual FormattedGuessInformation FormatGuessInfo(string guess)
        {
            var guessTemplate = BreakIntoWords( guess ).LastOrDefault();

            var commandFormat = guess.Replace(guessTemplate, "{0}");
            if( ! guessTemplate.Contains("*"))
            {
                guessTemplate += "*";
            }
            
            return new FormattedGuessInformation(guessTemplate, commandFormat);
        }

        public virtual IEnumerable<string> GetSuggestions(string guess)
        {
            var info = FormatGuessInfo(guess);
            Exception error;
            var items= _executor.ExecuteCommand(GetCommand(info),out error, ExecutionOptions.None);
            
            return items.ToList().ConvertAll(d => d.ToStringValue()).ConvertAll( d=>String.Format(info.CommandFormatString,d));
        }
    }
}
