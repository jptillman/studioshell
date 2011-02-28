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
using System.Management.Automation;
using CodeOwls.StudioShell.DTE;
using CodeOwls.StudioShell.DTE.ProjectModel;
using CodeOwls.StudioShell.Utility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Constants = EnvDTE.Constants;

namespace CodeOwls.StudioShell.PathNodes
{
    [CmdletHelpPathID("ErrorTaskCollection")]
    class ErrorListNodeFactory : CollectionNodeFactoryBase, INewItem
    {
        internal static Utility.InternalErrorListProvider ErrorProvider;

        private readonly List<ErrorItem> _items;
        private readonly DTE2 _dte;

        public ErrorListNodeFactory(ErrorItems items, DTE2 dte)
        {
            _dte = dte;
            _items = new List<ErrorItem>( items.Count);

            for( int i = 0; i < items.Count; ++i)
            {
                _items.Add( items.Item( i + 1 ));
            }
        }

        #region Overrides of NodeFactoryBase

        public override IEnumerable<INodeFactory> GetNodeChildren()
        {
            return (from ErrorItem item in _items 
                    select new ErrorListItemNodeFactory(item) as INodeFactory)
                .ToList();
        }
        public override string Name
        {
            get { return "Errors"; }
        }

        #endregion

        #region Implementation of INewItem

        class NewItemParams
        {
            [Parameter(ParameterSetName = "nameSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ParameterSetName = "pathSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ValueFromPipelineByPropertyName = true)]
            public int Line { get; set; }

            [Parameter(ParameterSetName = "nameSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ParameterSetName = "pathSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ValueFromPipelineByPropertyName = true)]            
            public int Column { get; set; }

            [Parameter(ParameterSetName = "nameSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ParameterSetName = "pathSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ValueFromPipelineByPropertyName = true)]
            [Alias( "File", "ProjectItem")]
            public string SourcePath { get; set; }
        }

        public IEnumerable<string> NewItemTypeNames
        {
            get { return Enum.GetNames( typeof (TaskErrorCategory )); }
        }

        public object NewItemParameters
        {
            get { return new NewItemParams(); }
        }

        public IPathNode NewItem(Context context, string path, string itemTypeName, object newItemValue)
        {
            if( null == ErrorProvider )
            {
                ErrorProvider = new InternalErrorListProvider( context.ServiceProvider );
            }

            var p = context.DynamicParameters as NewItemParams;

            var errorCategory = TaskErrorCategory.Error;
            try
            {
                errorCategory = (TaskErrorCategory)Enum.Parse(typeof (TaskErrorCategory), itemTypeName, true);
            }
            catch
            {
            }

            var task = new ErrorTask
                           {
                               ErrorCategory = errorCategory,
                               Text = newItemValue.ToString(),
                               Column = p.Column,
                               Line = p.Line,
                               HierarchyItem = GetHeirarchyItemFromSourcePath( context, p.SourcePath ),
                               Document = GetDocumentFromSourcePath(context, p.SourcePath)
                               
                           };

            task.Navigate += NavigateToTask;
            ErrorProvider.Tasks.Add(task);
            ErrorProvider.Show();

            var errors = _dte.ToolWindows.ErrorList.ErrorItems;

            ErrorItem item = errors.Item(errors.Count);
            if( null != item )
            {
                return new ErrorListItemNodeFactory(item).GetNodeValue();
            }

            return null;
        }

        #endregion

        private IVsHierarchy GetHeirarchyItemFromSourcePath(Context context, string sourcePath)
        {
            IVsSolution sln = context.ServiceProvider.GetService<IVsSolution>();
            if (null == sln)
            {
                return null;
            }

            Project project = GetProjectFromSourcePath(context, sln, sourcePath);
            if (null == project)
            {
                return null;
            }

            IVsHierarchy heirarchy;
            sln.GetProjectOfUniqueName(project.UniqueName, out heirarchy);

            return heirarchy;
        }

        private Project GetProjectFromSourcePath(Context context, IVsSolution sln, string sourcePath)
        {
            if (String.IsNullOrEmpty(sourcePath))
            {
                return null;
            }

            var paths = context.Provider.SessionState.Path.GetResolvedPSPathFromPSPath(sourcePath);
            sourcePath = paths[0].Path;
            var nodeFactory = context.Provider.Drive.GetNodeFromPath(sourcePath);
            var item = nodeFactory.GetNodeValue().Item;
            var p = item as ShellProject;
            var i = item as ShellProjectItem;

            if (null != p)
            {
                return p.Object as Project;
            }

            if (null != i)
            {
                return i.ContainingProject.AsProject();
            }

            return null;
        }

        private string GetDocumentFromSourcePath(Context context, string sourcePath)
        {
            if (String.IsNullOrEmpty(sourcePath))
            {
                return null;
            }

            var paths = context.Provider.SessionState.Path.GetResolvedPSPathFromPSPath(sourcePath);
            sourcePath = paths[0].Path;
            var nodeFactory = context.Provider.Drive.GetNodeFromPath(sourcePath);
            var item = nodeFactory.GetNodeValue().Item;
            var i = item as ShellProjectItem;

            if (null != i)
            {
                return i.FileNames[0];
            }

            return sourcePath;
        }
        
        private static void NavigateToTask(object sender, EventArgs e)
        {
            var task = sender as ErrorTask;
            if (null == task || null == ErrorProvider )
            {
                return;
            }
            try
            {
                ErrorProvider.Navigate(task, new Guid(Constants.vsViewKindCode));
            }
            catch
            {
            }
        }
    }
}
