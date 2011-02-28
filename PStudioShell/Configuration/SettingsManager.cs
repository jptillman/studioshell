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
using A=System.Management.Automation;
using System.Management.Automation.Runspaces;
using CodeOwls.StudioShell.Utility;

namespace CodeOwls.StudioShell.Configuration
{
    static class SettingsManager
    {
        internal static void Save( Settings settings )
        {
            const string path = Scripts.SaveSettings;
            
            var runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            using (A.PowerShell ps = A.PowerShell.Create())
            {
                ps.Runspace = runspace;

                ps.AddCommand(path);

                try
                {
                    var results = ps.Invoke( new[]{settings} );
                }
                catch
                {

                }
            }
        }

        internal static Settings Load()
        {
            var path = Scripts.GetSettings;
            var settings = new Settings();

            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();
                using (A.PowerShell ps = A.PowerShell.Create())
                {
                    ps.Runspace = runspace;

                    var script = String.Format("new-object -property ({0}) -typename '{1}'",
                                               path,
                                               typeof (Settings).FullName);
                    ps.AddScript(script);

                    try
                    {
                        var results = ps.Invoke();
                        if (results.Any())
                        {
                            settings = (results.First().BaseObject as Settings) ?? settings;
                        }
                    }
                    catch
                    {

                    }
                }
            }
            return settings;
        }
    }
}
