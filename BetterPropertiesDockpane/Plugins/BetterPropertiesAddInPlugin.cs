using Autodesk.Navisworks.Api.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavisworksApiApplication = Autodesk.Navisworks.Api.Application;

namespace BetterPropertiesDockpane.Plugins
{
    [Plugin("BetterPropertiesAddIn", "PedramElmi", DisplayName = "Better Properties")]
    [AddInPlugin(AddInLocation.AddIn, CanToggle =true)]
    public class BetterPropertiesAddInPlugin : AddInPlugin
    {
        public BetterPropertiesAddInPlugin()
        {
            Application.AddInPlugin = this;
        }

        public override int Execute(params string[] parameters)
        {
            if (Application.DockPanePlugin is null)
            {
                // find DockPane plugin
                var dockpane = NavisworksApiApplication.Plugins.FindPlugin("BetterPropertiesDockPane.PedramElmi");
                var plugin = dockpane.TryLoadPlugin();
                (plugin as DockPanePlugin)?.ActivatePane();

            }
            else
            {
                Application.DockPanePlugin.ActivatePane();
            }
            return 0;
        }
    }
}
