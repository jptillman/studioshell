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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeOwls.StudioShell.Utility
{
    class EnvironmentConfiguration
    {
        static internal void UpdateProcessEnvironment()
        {
            var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            AddEnvironmentPath("psmodulepath", rootPath, "Module");
            AddEnvironmentPath("path", rootPath, "Scripts");
        }

        static private void AddEnvironmentPath(string environmentVariable, string rootPath, string subdir)
        {                       
            var pspaths = (Environment.GetEnvironmentVariable(environmentVariable) ?? "").Split(';').ToList();
            var newPath = Path.Combine(
                rootPath,
                subdir
                );
            if( pspaths.Contains( newPath, StringComparer.InvariantCultureIgnoreCase ))
            {
                return;
            }
            pspaths.Add( newPath );
            Environment.SetEnvironmentVariable(environmentVariable, String.Join(";", pspaths.ToArray()));
        }


    }
}
