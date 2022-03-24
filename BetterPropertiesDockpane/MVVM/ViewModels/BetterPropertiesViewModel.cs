using Autodesk.Navisworks.Api;
using BetterPropertiesDockpane.Helper;
using NavisworksDevHelper;
using NavisworksDevHelper.ModelItemHelpers;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Forms = System.Windows.Forms;
using NavisworksApiApplication = Autodesk.Navisworks.Api.Application;
using Windows = System.Windows;

namespace BetterPropertiesDockpane.MVVM.ViewModels
{
    public class BetterPropertiesViewModel : ObservableObject
    {
        #region Private Fields

        private bool _isSavingJsonFile;
        private int _modelItemscapacity = 100;
        private ModelItemCollection _selectedModelItems;

        #endregion Private Fields

        #region Public Constructors

        public BetterPropertiesViewModel()
        {
            Application.ViewModel = this;

            IsSavingJsonFile = false;

            //NavisworksApiApplication.ActiveDocument.CurrentSelection.Changed += this.OnCurrentSelectionChanged;

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

        public int ModelItemsCapacity
        {
            get { return _modelItemscapacity; }
            set { _modelItemscapacity = value; OnPropertyChanged(); }
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

        public void OnCurrentSelectionChanged(object sender, EventArgs e)
        {
            // if a real document still not loaded then do nothing
            if (NavisworksApiApplication.ActiveDocument != null)
            {
                if (!NavisworksApiApplication.ActiveDocument.IsClear)
                {
                    var modelItemCollection = new ModelItemCollection();
                    modelItemCollection.AddRange(((Document)sender).CurrentSelection.SelectedItems);
                    SelectedModelItems = modelItemCollection;
                }
            }
        }

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
                        NavisworksApiApplication.ActiveDocument.CurrentSelection.SelectedItems.JsonSerialize(filePath, namingStrategy: NamingStrategy.CamelCase);
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
                    NavisworksApiApplication.ActiveDocument.CurrentSelection.SelectedItems.JsonSerialize(filePath, namingStrategy: NamingStrategy.CamelCase);
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