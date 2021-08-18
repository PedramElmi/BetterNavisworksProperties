using Autodesk.Navisworks.Api;
using BetterPropertiesDockpane.Helper;
using System;

namespace BetterPropertiesDockpane.MVVM.ViewModels
{
    public class BetterPropertiesViewModel : ObservableObject
    {


        private ModelItemCollection _selectedModelItems;

        public ModelItemCollection SelectedModelItems
        {
            get => _selectedModelItems;
            private set { _selectedModelItems = value; OnPropertyChanged(); }
        }





        public BetterPropertiesViewModel()
        {
            Application.ActiveDocument.CurrentSelection.Changed += this.CurrentSelection_Changed;
            Application.ActiveDocumentChanged += this.CurrentSelection_Changed;
        }

        private void CurrentSelection_Changed(object sender, EventArgs e)
        {
            // if a real document still not loaded then do nothing
            if (Application.ActiveDocument != null && !Application.ActiveDocument.IsClear)
            {
                SelectedModelItems = ((Document)sender).CurrentSelection.SelectedItems;
            }
        }
    }
}
