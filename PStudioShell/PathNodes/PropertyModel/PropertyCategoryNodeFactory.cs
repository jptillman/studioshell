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
using Microsoft.Win32;

namespace CodeOwls.StudioShell.PathNodes
{
    public class PropertyCategoryNodeFactory : NodeFactoryBase
    {
        private readonly DTE2 _dte;
        private readonly RegistryKey _subkey;
        private readonly string _category;

        public PropertyCategoryNodeFactory(DTE2 dte2, RegistryKey subkey)
        {
            _dte = dte2;
            _subkey = subkey;

            _category = GetCategoryNameFromKey(_subkey.Name);
        }

        public override IPathNode GetNodeValue()
        {
            return new PathNode(new DTE.ShellContainer(this), Name, true);
        }

        public override IEnumerable<INodeFactory> GetNodeChildren()
        {
            var factories = new List<INodeFactory>();
            foreach (var subkeyName in _subkey.GetSubKeyNames())
            {
                factories.Add(new PropertyPageNodeFactory(_dte, Name, GetCategoryNameFromKey(subkeyName) ));
            }
            return factories;
        }

        public override string Name
        {
            get
            {
                return _category;
            }
        }

        string GetCategoryNameFromKey( string subkeyName )
        {
            return subkeyName.Split('/', '\\').Last();
        }
    }
}
