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
using EnvDTE;
using CodeOwls.StudioShell.DTE.UI;

namespace CodeOwls.StudioShell.PathNodes
{
    class WindowConfigurationNodeFactory : NodeFactoryBase, IRemoveItem, IInvokeItem, ISetItem
    {
        private readonly WindowConfiguration _windowConfiguration;

        public WindowConfigurationNodeFactory( WindowConfiguration windowConfiguration )
        {
            _windowConfiguration = windowConfiguration;
        }

        public override IPathNode GetNodeValue()
        {
            return new PathNode( new ShellWindowConfiguration( _windowConfiguration ), Name, false );
        }

        public override IEnumerable<INodeFactory> GetNodeChildren()
        {
            return null;
        }

        public override string Name
        {
            get { return _windowConfiguration.Name;  }
        }

        #region Implementation of IRemoveItem

        public object RemoveItemParameters
        {
            get { return null; }
        }

        public void RemoveItem(Context context, string path, bool recurse)
        {
            _windowConfiguration.Delete();
        }

        #endregion

        #region Implementation of IInvokeItem

        public object InvokeItemParameters
        {
            get { return null; }
        }

        public IEnumerable<object> InvokeItem(Context context, string path)
        {
            _windowConfiguration.Apply( true );
            return null;
        }

        #endregion

        #region Implementation of ISetItem

        public object SetItemParameters
        {
            get { return null; }
        }

        public IPathNode SetItem(Context context, string path, object value)
        {
            _windowConfiguration.Update();
            return GetNodeValue();
        }

        #endregion
    }
}
