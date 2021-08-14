using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using System.Windows.Forms;

namespace BetterPropertiesDockpane
{
    [Plugin(name: "BetterProperties", developerId: "PDEL", DisplayName = "Better Properties")]
    [DockPanePlugin(preferredWidth: 400, preferredHeight: 600, AutoScroll = true, FixedSize = false, MinimumHeight = 500, MinimumWidth = 200)]
    public class BetterPropertiesDockPanePlugin : DockPanePlugin
    {
        public override Control CreateControlPane()
        {
            return base.CreateControlPane();
        }
    }
}
