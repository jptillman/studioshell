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
using CodeOwls.StudioShell.DTE;

namespace CodeOwls.StudioShell.PathNodes
{
    class OutputPaneCollectionNodeFactory : CollectionNodeFactoryBase, INewItem
    {
        private OutputWindowPanes _panes;

        public OutputPaneCollectionNodeFactory(OutputWindowPanes panes)
        {
            _panes = panes;
        }

        #region Overrides of NodeFactoryBase

        public override IEnumerable<INodeFactory> GetNodeChildren()
        {
            List<INodeFactory> factories = new List<INodeFactory>();
            foreach( OutputWindowPane pane in _panes )
            {
                factories.Add( new OutputPaneNodeFactory( pane ));
            }

            return factories;
        }

        public override string Name
        {
            get { return "OutputPanes"; }
        }

        #endregion

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
            var pane = _panes.Add(path);
            var factory = new OutputPaneNodeFactory(pane);
            return factory.GetNodeValue();
        }

        #endregion        
    }
}
