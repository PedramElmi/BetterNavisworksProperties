using Autodesk.Navisworks.Api;
using BetterPropertiesDockpane.Helper;
using System;
using Forms = System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using Windows = System.Windows;

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


        public ICommand SaveAsJsonFileCommand { get; set; }





        public BetterPropertiesViewModel()
        {
            Application.ActiveDocument.CurrentSelection.Changed += this.CurrentSelection_Changed;
            Application.ActiveDocumentChanged += this.CurrentSelection_Changed;

            SaveAsJsonFileCommand = new RelayCommand(SaveAsJsonFile, CanBeSavedAsJsonFile);

        }

        public void SaveAsJsonFile(object _)
        {

            Forms.SaveFileDialog saveFileDialog = new Forms.SaveFileDialog()
            {
                Filter = "JavaScript Object Notation|* json",
                Title = "Save Selected ModelItems Properties"
            };

            if (saveFileDialog.ShowDialog() == Forms.DialogResult.OK)
            {

                var output = NavisworksDevHelper.ModelItem.CategoriesPropertiesHelper.SerializeModelItemsProperties(SelectedModelItems);

                string pattern = @"^.*\.(json)$";

                try
                {
                    if (!Regex.IsMatch(saveFileDialog.FileName, pattern))
                    {
                        System.IO.File.WriteAllText($"{saveFileDialog.FileName}.json", output.ToString());
                    }
                    else
                    {
                        System.IO.File.WriteAllText(saveFileDialog.FileName, output.ToString());
                    }
                }
                catch (Exception e)
                {

                    var result = Windows.MessageBox.Show(e.Message, e.StackTrace, Windows.MessageBoxButton.OKCancel);
                    if (result == Windows.MessageBoxResult.OK)
                    {
                        SaveAsJsonFile(_);
                    }
                }
                
            }
        }
        
        private bool CanBeSavedAsJsonFile(object _)
        {

            if (SelectedModelItems is null)
            {
                return false;
            }
            else if (SelectedModelItems.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
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
