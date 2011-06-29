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


using System.Collections.Generic;
using System.Linq;
using EnvDTE80;
using CodeOwls.StudioShell.DTE;

namespace CodeOwls.StudioShell.PathNodes
{
	public class SolutionNodeFactory : NodeFactoryBase
	{
		private readonly DTE2 _dte;
		private readonly DTE.ShellSolution _shellObject;
		
		public SolutionNodeFactory(DTE2 dte)
		{
			_dte = dte;
			_shellObject = new ShellSolution( _dte.Solution as Solution2 );
		}

		public override IPathNode GetNodeValue()
		{
			return new PathNode( _shellObject, Name, true );
		}

		public override IEnumerable<INodeFactory> GetNodeChildren()
		{
			return new INodeFactory[]
			           {
                            new SolutionProjectsNodeFactory(_dte)
			           };
		}

		public override string Name
		{
			get { return "solution"; }
		}
	}
}
