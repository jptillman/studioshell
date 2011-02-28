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
using System.Text;
using CodeOwls.StudioShell.DTE.ProjectModel;
using CodeOwls.StudioShell.Utility;
using VSLangProj;

namespace CodeOwls.StudioShell.PathNodes.ProjectModel
{
    [CmdletHelpPathID("ProjectReference")]
    class ReferenceNodeFactory : NodeFactoryBase, IRemoveItem
    {
        private readonly Reference _reference;

        public ReferenceNodeFactory(Reference reference)
        {
            _reference = reference;
        }

        #region Overrides of NodeFactoryBase

        public override IPathNode GetNodeValue()
        {
            return new PathNode( new ShellReference(_reference), Name, false);
        }

        public override string Name
        {
            get { return _reference.Name; }
        }

        #endregion

        #region Implementation of IRemoveItem

        public object RemoveItemParameters
        {
            get { return null; }
        }

        public void RemoveItem(Context context, string path, bool recurse)
        {           
            _reference.Remove();
        }

        #endregion
    }
}
