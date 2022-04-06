using BetterPropertiesDockpane.MVVM.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NavisworksDevHelper;
using NavisworksDevHelper.ModelItemHelpers;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Api = Autodesk.Navisworks.Api;
using Forms = System.Windows.Forms;
using Windows = System.Windows;

namespace BetterPropertiesDockpane.MVVM.ViewModels
{
    public class BetterPropertiesViewModel : ObservableObject
    {
        #region Private Fields

        private Document _document = Application.Document;
        private bool _isSavingJsonFile = false;

        #endregion Private Fields

        #region Public Constructors

        public BetterPropertiesViewModel()
        {
            SaveAsJsonFileCommand = new RelayCommand(SaveAsJsonFile, CanBeSavedAsJsonFile);
            Api.Application.ActiveDocument.CurrentSelection.Changed += OnCurrentSelectionChanged;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool IsSavingJsonFile
        {
            get => _isSavingJsonFile;
            set
            {
                SetProperty(ref _isSavingJsonFile, value);
            }
        }

        public RelayCommand SaveAsJsonFileCommand { get; set; }

        #endregion Public Properties

        #region Private Properties

        public Document Document { get => _document; }

        #endregion Private Properties

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
                        Api.Application.ActiveDocument.CurrentSelection.SelectedItems.JsonSerialize(filePath, namingStrategy: NamingStrategy.CamelCase);
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

        private bool CanBeSavedAsJsonFile()
        {
            if (Document.SelectedModelItems is null)
            {
                return false;
            }
            if (IsSavingJsonFile)
            {
                return false;
            }
            else if (Document.SelectedModelItems.Count != 0)
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
            SaveAsJsonFileCommand.NotifyCanExecuteChanged();
        }

        private void SaveAsJsonFile()
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
                    Api.Application.ActiveDocument.CurrentSelection.SelectedItems.JsonSerialize(filePath, namingStrategy: NamingStrategy.CamelCase);
                }
                catch (Exception e)
                {
                    var result = Windows.MessageBox.Show(e.Message, e.StackTrace, Windows.MessageBoxButton.OKCancel);
                    if (result == Windows.MessageBoxResult.OK)
                    {
                        SaveAsJsonFile();
                    }
                }
            }
        }

        #endregion Private Methods
    }
}