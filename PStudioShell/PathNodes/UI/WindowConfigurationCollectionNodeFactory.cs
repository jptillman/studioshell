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
using EnvDTE;
using CodeOwls.StudioShell.DTE.UI;

namespace CodeOwls.StudioShell.PathNodes
{
    class WindowConfigurationCollectionNodeFactory : CollectionNodeFactoryBase, INewItem
    {
        private readonly WindowConfigurations _configurations;

        public WindowConfigurationCollectionNodeFactory( WindowConfigurations configurations)
        {
            _configurations = configurations;
        }

        public override IEnumerable<INodeFactory> GetNodeChildren()
        {
            List<INodeFactory> factories = new List<INodeFactory>();
            foreach( WindowConfiguration windowConfiguration in _configurations )
            {
                factories.Add( new WindowConfigurationNodeFactory(windowConfiguration) );
            }
            return factories;
        }

        public override string Name
        {
            get { return "windowconfigurations"; }
        }

        #region Implementation of INewItem

        public IEnumerable<string> NewItemTypeNames
        {
            get { return null; }
        }

        public object NewItemParameters
        {
            get { return null; }
        }

        public IPathNode NewItem(Context context, string path, string itemTypeName, object newItemValue)
        {
            var newConfig = _configurations.Add(path);
            
            return new PathNode( new ShellWindowConfiguration(newConfig), path, false );
        }

        #endregion
    }
}
