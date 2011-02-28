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
using System.Text;
using EnvDTE;

namespace CodeOwls.StudioShell.DTE.Debugger
{
    public class ShellExpression
    {
        private readonly Expression _expression;

        internal ShellExpression(Expression expression)
        {
            _expression = expression;
        }

        public string Name
        {
            get { return _expression.Name; }
        }

        public string Type
        {
            get { return _expression.Type; }
        }

        public IEnumerable<ShellExpression> DataMembers
        {
            get
            {
                if( null != _expression.DataMembers )
                {
                    foreach( Expression x in _expression.DataMembers )
                    {
                        yield return new ShellExpression(x);
                    }
                }
            }
        }

        public string Value
        {
            get { return _expression.Value; }
            set { _expression.Value = value; }
        }

        public bool IsValidValue
        {
            get { return _expression.IsValidValue; }
        }
    }
}
