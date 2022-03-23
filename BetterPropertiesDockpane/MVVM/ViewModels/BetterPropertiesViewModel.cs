using Autodesk.Navisworks.Api;
using BetterPropertiesDockpane.Helper;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Forms = System.Windows.Forms;
using Windows = System.Windows;

namespace BetterPropertiesDockpane.MVVM.ViewModels
{
    public class BetterPropertiesViewModel : ObservableObject
    {
        #region Private Fields

        private bool _isSavingJsonFile;
        private ModelItemCollection _selectedModelItems;

        #endregion Private Fields

        #region Public Constructors

        public BetterPropertiesViewModel()
        {
            IsSavingJsonFile = false;

            Application.ActiveDocument.CurrentSelection.Changed += this.OnCurrentSelectionChanged;

            SaveAsJsonFileAsyncCommand = new AsyncRelayCommand(SaveAsJsonFileAsync, CanBeSavedAsJsonFile, true);

            SaveAsJsonFileCommand = new RelayCommand(SaveAsJsonFile, CanBeSavedAsJsonFile);
        }

        #endregion Public Constructors

        #region Public Properties

        public bool IsSavingJsonFile
        {
            get => _isSavingJsonFile;
            set
            {
                _isSavingJsonFile = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveAsJsonFileAsyncCommand { get; set; }

        public ICommand SaveAsJsonFileCommand { get; set; }

        public ModelItemCollection SelectedModelItems
        {
            get => _selectedModelItems;
            private set { _selectedModelItems = value; OnPropertyChanged(); }
        }

        #endregion Public Properties

        #region Public Methods

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
                        NavisworksDevHelper.ModelItemHelpers.CategoriesPropertiesHelper.SerializeModelItems(SelectedModelItems, filePath, false, true);
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

        #endregion Public Methods

        #region Private Methods

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
                    NavisworksDevHelper.ModelItemHelpers.CategoriesPropertiesHelper.SerializeModelItems(SelectedModelItems, filePath, false, false);
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

        #endregion Private Methods
    }
}