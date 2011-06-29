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
using System.Linq;
using System.Management.Automation;
using System.Text;
using CodeOwls.StudioShell.Paths.Items.UI;
using EnvDTE80;

namespace CodeOwls.StudioShell.Variables
{
    public class ActiveWindowVariable : DTEPSVariable
    {
        public ActiveWindowVariable(DTE2 dte, string name) : base(dte, name)
        {
        }

        public override object Value
        {
            get
            {
                return new ShellWindow(_dte.ActiveWindow as Window2);
            }
        }
    }
}
