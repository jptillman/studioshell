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


using System.Diagnostics;
using System.Management.Automation;
using System.Windows.Forms;
using CodeOwls.StudioShell.DataPaneControls;
using EnvDTE;
using EnvDTE80;

namespace CodeOwls.StudioShell
{
    public abstract class OutDataPaneCmdletBase : PSCmdlet
    {
        protected PSObjectBindingList _data;
        private Window _toolWindow;
        private DataPaneControl _dataPaneControl;

        public OutDataPaneCmdletBase()
        {
            Name = "Data";
        }

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public PSObject Input { get; set; }

        [Parameter]
        public string Name { get; set; }

        protected override void BeginProcessing()
        {
            _data = new PSObjectBindingList();
        }

        protected override void ProcessRecord()
        {
            _data.Add(Input);
        }

        protected override void EndProcessing()
        {
            DataPaneControl paneControl = GetDataPaneControl();
            Control gridControl = GetPaneControl();
            gridControl.Name = Name;

            paneControl.AddControl(gridControl);

            _toolWindow.Visible = true;
        }

        protected abstract Control GetPaneControl();

        private DataPaneControl GetDataPaneControl()
        {
            if (null == _dataPaneControl)
            {
                object window = null;
                var asm = typeof(DataGridControl).Assembly;
                Windows2 w = (Windows2)Connect.ApplicationObject.Windows;

                if (null == _toolWindow)
                {
                    try
                    {
                        _toolWindow = w.Item(DataPaneControl.Guid);
                        window = _toolWindow.Object;
                    }
                    catch
                    {
                    }

                    if (null == _toolWindow)
                    {
                        _toolWindow = w.CreateToolWindow2(
                            Connect.AddInInstance,
                            asm.Location,
                            typeof(DataPaneControl).FullName,
                            "Studio Shell Data Panes",
                            DataPaneControl.Guid,
                            ref window);
                    }
                    if (null == _toolWindow || null == window)
                    {
                        Debug.Assert(false, "unable to locate data pane tool window");
                    }
                }

                _dataPaneControl = (DataPaneControl)window;
            }
            return _dataPaneControl;
        }
    }
}
