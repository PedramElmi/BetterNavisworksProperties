using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using BetterPropertiesDockpane.MVVM.Views;

namespace BetterPropertiesDockpane.DockPanes
{
    [Plugin(name: "BetterProperties", developerId: "PedramElmi", DisplayName = "Better Properties")]
    [DockPanePlugin(preferredWidth: 300, preferredHeight: 500, AutoScroll = false, FixedSize = false, MinimumHeight = 0, MinimumWidth = 0)]
    public class BetterPropertiesDockPanePlugin : DockPanePlugin
    {

        public override Control CreateControlPane()
        {
            //create an ElementHost
            ElementHost elementHost = new ElementHost
            {

                //assign the control
                AutoSize = true,
                Child = new BetterPropertiesView()

            };

            elementHost.CreateControl();

            elementHost.ParentChanged += this.ElementHost_ParentChanged;


            //return the ElementHost
            return elementHost;
        }

        /// <summary>
        /// Resizing DockPane implies resizing WPF-based ElementHost
        /// </summary>
        /// <param name="sender">ElementHost</param>
        /// <param name="e"></param>
        private void ElementHost_ParentChanged(object sender, EventArgs e)
        {
            if (sender is ElementHost elementHost)
            {
                elementHost.Dock = DockStyle.Fill;
            }
        }


        public override void DestroyControlPane(Control pane)
        {
            try
            {

                pane?.Dispose();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }

    }
}
