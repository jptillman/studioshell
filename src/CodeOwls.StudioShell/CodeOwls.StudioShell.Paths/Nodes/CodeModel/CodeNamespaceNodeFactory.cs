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
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.PowerShell.Provider.PathNodes;
using CodeOwls.StudioShell.Paths.Utility;
using EnvDTE;

namespace CodeOwls.StudioShell.Paths.Nodes.CodeModel
{
    public class CodeNamespaceNodeFactory : CodeElementWithChildrenNodeFactory
    {
        private readonly CodeNamespace _namespace;

        public CodeNamespaceNodeFactory(CodeNamespace element) : base(element as CodeElement)
        {
            _namespace = element;
        }

        // we have to override the node enumeration for namespace nodes 
        //  to work around the issue described here: 
        //  http://www.hightechtalks.com/visual-studio-net-general/re-codeelement-children-namespaces-337424.html

        protected override string CodeItemTypeName
        {
            get { return CodeItemTypes.Namespace; }
        }

        public override IEnumerable<string> NewItemTypeNames
        {
            get { return CodeItemTypes.NamespaceNewItemTypeNames; }
        }

        public override IEnumerable<INodeFactory>  GetNodeChildren( IContext context )
        {
            List<INodeFactory> factories = new List<INodeFactory>();

            // enumerable Members instead of Children - srsly??
            foreach (CodeElement e in _namespace.Members)
            {
                factories.Add(FileCodeModelNodeFactory.CreateNodeFactoryFromCodeElement(e));
            }
            return factories;
        }

        protected override object NewStruct(NewCodeElementItemParams newItemParams, string path)
        {
            return _namespace.AddStruct(path,
                                        newItemParams.Position,
                                        newItemParams.Bases.ToCSVDTEParameter(),
                                        newItemParams.ImplementedInterfaces.ToCSVDTEParameter(),
                                        newItemParams.AccessKind);
        }

        protected override object NewNamespace(NewCodeElementItemParams newItemParams, string path)
        {
            return _namespace.AddNamespace(path,
                                           newItemParams.Position);
        }

        protected override object NewInterface(NewCodeElementItemParams newItemParams, string path)
        {
            return _namespace.AddInterface(path,
                                           newItemParams.Position,
                                           newItemParams.ImplementedInterfaces.ToCSVDTEParameter(),
                                           newItemParams.AccessKind);
        }

        protected override object NewEnum(NewCodeElementItemParams newItemParams, string path)
        {
            return _namespace.AddEnum(path,
                                      newItemParams.Position,
                                      newItemParams.Bases.ToCSVDTEParameter(),
                                      newItemParams.AccessKind
                );
        }

        protected override object NewDelegate(NewCodeElementItemParams newItemParams, string path)
        {
            return _namespace.AddDelegate(path,
                                          newItemParams.MemberType,
                                          newItemParams.Position,
                                          newItemParams.AccessKind
                );
        }

        protected override object NewClass(NewCodeElementItemParams newItemParams, string path)
        {
            return _namespace.AddClass(path,
                                       newItemParams.Position,
                                       newItemParams.Bases.ToCSVDTEParameter(),
                                       newItemParams.ImplementedInterfaces.ToCSVDTEParameter(),
                                       newItemParams.AccessKind
                );
        }
    }
}