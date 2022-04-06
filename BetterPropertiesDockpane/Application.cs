using Api = Autodesk.Navisworks.Api;
using BetterPropertiesDockpane.MVVM.ViewModels;
using BetterPropertiesDockpane.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterPropertiesDockpane.MVVM.Models;

namespace BetterPropertiesDockpane
{
    public static class Application
    {
        private static Document _document = new Document();

        public static BetterPropertiesAddInPlugin AddInPlugin { get; set; }
        public static BetterPropertiesDockPanePlugin DockPanePlugin { get; set; }
        public static Document Document { get => _document; }
    }
}
