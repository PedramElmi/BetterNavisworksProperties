using Autodesk.Navisworks.Api;
using BetterPropertiesDockpane.Helper;
using System;
using Forms = System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using Windows = System.Windows;
using System.Threading.Tasks;

namespace BetterPropertiesDockpane.MVVM.ViewModels
{
    public class BetterPropertiesViewModel : ObservableObject
    {
        #region Fields
        private ModelItemCollection _selectedModelItems;
        private bool _isSavingJsonFile;
        #endregion

        #region Properties
        public ModelItemCollection SelectedModelItems
        {
            get => _selectedModelItems;
            private set { _selectedModelItems = value; OnPropertyChanged(); }
        }
        public bool IsSavingJsonFile
        {
            get => _isSavingJsonFile;
            set
            {
                _isSavingJsonFile = value;
                OnPropertyChanged();
            }
        }
        public ICommand SaveAsJsonFileCommand { get; set; }

        #endregion

        #region Constructors

        public BetterPropertiesViewModel()
        {
            IsSavingJsonFile = false;

            Application.ActiveDocument.CurrentSelection.Changed += this.OnCurrentSelectionChanged;
            
            SaveAsJsonFileAsyncCommand = new AsyncRelayCommand(SaveAsJsonFileAsync, CanBeSavedAsJsonFile, true);

            SaveAsJsonFileCommand = new RelayCommand(SaveAsJsonFile, CanBeSavedAsJsonFile);
        }

        #endregion

        #region Develop
        private void TestExe()
        {
            System.Windows.MessageBox.Show("RAN!");
        }

        private bool CanTestExe()
        {
            return false;
        }
        #endregion

        #region Methods

        private void SaveAsJsonFile(object commandParameter)
        {
            var saveFileDialog = new Forms.SaveFileDialog()
            {
                Filter = "JavaScript Object Notation|* json",
                Title = "Save Selected ModelItems Properties"
            };

            if (saveFileDialog.ShowDialog() == Forms.DialogResult.OK)
            {
                string filePath;
                string PATTERN = @"^.*\.(json)$";

                filePath = !Regex.IsMatch(saveFileDialog.FileName, PATTERN, RegexOptions.IgnoreCase) ? $"{saveFileDialog.FileName}.json" : saveFileDialog.FileName;


                try
                {
                    NavisworksDevHelper.ModelItem.CategoriesPropertiesHelper.SerializeModelItems(SelectedModelItems, filePath, false, false);
                }
                catch (Exception e)
                {

                    var result = Windows.MessageBox.Show(e.Message, e.StackTrace, Windows.MessageBoxButton.OKCancel);
                    if (result == Windows.MessageBoxResult.OK)
                    {
                        SaveAsJsonFile(commandParameter);
                    }
                }

            }
        }
        private bool CanBeSavedAsJsonFile(object commandParameter)
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
        private void OnCurrentSelectionChanged(object sender, EventArgs e)
        {
            // if a real document still not loaded then do nothing
            if (Application.ActiveDocument != null)
            {
                if (!Application.ActiveDocument.IsClear)
                {
                    SelectedModelItems = ((Document)sender).CurrentSelection.SelectedItems;
                }
            }
        }

        #endregion

        #region Unused Members
        public ICommand SaveAsJsonFileAsyncCommand { get; set; }
        public async Task SaveAsJsonFileAsync(object commandParameter)
        {

            var saveFileDialog = new Forms.SaveFileDialog()
            {
                Filter = "JavaScript Object Notation|* json",
                Title = "Save Selected ModelItems Properties"
            };

            if (saveFileDialog.ShowDialog() == Forms.DialogResult.OK)
            {
                string filePath;
                string PATTERN = @"^.*\.(json)$";

                filePath = !Regex.IsMatch(saveFileDialog.FileName, PATTERN, RegexOptions.IgnoreCase) ? $"{saveFileDialog.FileName}.json" : saveFileDialog.FileName;


                try
                {
                    IsSavingJsonFile = true;
                    await Task.Run(() =>
                    {
                        NavisworksDevHelper.ModelItem.CategoriesPropertiesHelper.SerializeModelItems(SelectedModelItems, filePath, false, true);
                        System.Windows.MessageBox.Show("File Saved Successfully");
                    });
                }
                catch (Exception e)
                {
                    var result = Windows.MessageBox.Show(e.Message, e.StackTrace, Windows.MessageBoxButton.OKCancel);
                    if (result == Windows.MessageBoxResult.OK)
                    {
                        await SaveAsJsonFileAsync(commandParameter);
                    }
                }
                finally
                {
                    IsSavingJsonFile = false;
                }
            }
        }

        #endregion
    }
}
