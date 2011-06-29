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
using CodeOwls.PowerShell.Provider.PathNodes;
using EnvDTE;
using EnvDTE80;
using CodeOwls.PowerShell.Provider.PathNodes;

namespace CodeOwls.StudioShell.Paths.Nodes.DTE
{
    internal class SelectedItemsNodeFactory : CollectionNodeFactoryBase
    {
        private readonly DTE2 _dte;

        public SelectedItemsNodeFactory(DTE2 dte)
        {
            _dte = dte;
        }

        #region Overrides of NodeFactoryBase

        public override string Name
        {
            get { return "SelectedItems"; }
        }

        public override IEnumerable<INodeFactory> GetNodeChildren()
        {
            return new INodeFactory[]
                       {
                           new SelectedProjectItemsNodeFactory(_dte.SelectedItems),
                           new SelectedProjectsNodeFactory(_dte.SelectedItems),
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementNamespace, "Namespace"),
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementClass, "Class"),
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementProperty, "Property"),
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementStruct, "Struct"),
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementInterface, "Interface"),
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementFunction, "Method"),
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementEnum, "Enum"),
                       };
        }

        #endregion
    }
}