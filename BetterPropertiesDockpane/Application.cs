using BetterPropertiesDockpane.MVVM.ViewModels;
using BetterPropertiesDockpane.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterPropertiesDockpane
{
    public static class Application
    {
        public static BetterPropertiesAddInPlugin AddInPlugin { get; set; }
        public static BetterPropertiesDockPanePlugin DockPanePlugin { get; set; }
        public static BetterPropertiesViewModel ViewModel { get; set; }
    }
}
