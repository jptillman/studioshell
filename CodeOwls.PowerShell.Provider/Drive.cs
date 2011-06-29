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
using System.Text.RegularExpressions;
using CodeOwls.PowerShell.Paths.Processors;
using CodeOwls.PowerShell.Provider.PathNodes;

namespace CodeOwls.PowerShell.Provider
{
    public abstract class Drive : PSDriveInfo
    {
    	private readonly IPathNodeProcessor _pathProcessor;

    	public Drive(PSDriveInfo driveInfo, IPathNodeProcessor pathProcessor)
            : base(driveInfo)
        {
        	_pathProcessor = pathProcessor;
		}

		public INodeFactory GetNodeFromPath( string path )
		{
		    path = EnsurePathIsRooted(path);
            
		    return _pathProcessor.ResolvePath(path);
		}

        private string EnsurePathIsRooted(string path)
        {
            var driveName = GetDriveName(path);

            if (!path.StartsWith(driveName, StringComparison.InvariantCultureIgnoreCase))
            {
                path = EnsurePathIsRooted(path);
            }
            
            var psdrive = (from psd in Provider.Drives
                           where StringComparer.InvariantCultureIgnoreCase.Equals(psd.Name, driveName)
                           select psd).FirstOrDefault();
            if( null == psdrive )
            {
                return path;
            }

            return path.Replace(driveName + ":", psdrive.Root);
        }

        private string GetDriveName(string path)
        {
            Regex re = new Regex( @"^([^:]+):");
            var match = re.Match(path);
            if( ! match.Success)
            {
                return String.Empty;
            }

            return match.Groups[1].Value;
        }

        public IPathNodeProcessor PathProcessor
    	{
    		get { return _pathProcessor; }
    	}
    }
}
