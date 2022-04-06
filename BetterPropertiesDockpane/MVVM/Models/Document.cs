using Autodesk.Navisworks.Api;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Api = Autodesk.Navisworks.Api;

namespace BetterPropertiesDockpane.MVVM.Models
{
    public class Document : ObservableObject
    {
        #region Private Fields

        private bool _isSubscribedToCurrentSelectionChanged = false;

        private int _modelItemsCapacity;

        private ObservableCollection<ModelItem> _selectedModelItems = new ObservableCollection<ModelItem>();

        #endregion Private Fields

        #region Public Constructors

        public Document()
        {
            Api.Application.ActiveDocumentChanging += OnApiActiveDocumentChanging;
            Api.Application.ActiveDocument.Models.CollectionChanging += OnApiActiveDocumentChanging;

            Api.Application.ActiveDocumentChanged += OnApiApplicationActiveDocumentChanged;
            Api.Application.ActiveDocument.Models.CollectionChanged += OnApiApplicationActiveDocumentChanged;

        }

        #endregion Public Constructors

        #region Public Properties

        public int ModelItemsCapacity
        {
            get { return _modelItemsCapacity; }
            set { SetProperty(ref _modelItemsCapacity, value); }
        }

        public ObservableCollection<ModelItem> SelectedModelItems
        {
            get { return _selectedModelItems; }
            set { SetProperty(ref _selectedModelItems, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public void OnCurrentSelectionChanged(object sender, EventArgs e)
        {
            // if a real document still not loaded then do nothing
            if (Api.Application.ActiveDocument != null)
            {
                if (!Api.Application.ActiveDocument.IsClear)
                {
                    var addedItems = from modelItem in ((Api.Document)sender).CurrentSelection.SelectedItems
                                     where !SelectedModelItems.Contains(modelItem)
                                     select modelItem;

                    var removedItems = from modelItem in SelectedModelItems
                                       where !((Api.Document)sender).CurrentSelection.SelectedItems.Contains(modelItem)
                                       select modelItem;

                    foreach (var item in addedItems.ToList())
                    {
                        SelectedModelItems.Add(item);
                    }

                    try
                    {
                        foreach (var item in removedItems.ToList())
                        {
                            SelectedModelItems.Remove(item);
                        }
                    }
                    catch (InvalidOperationException ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void OnApiActiveDocumentChanging(object sender, EventArgs e)
        {
            SelectedModelItems.Clear();
            if (_isSubscribedToCurrentSelectionChanged && Api.Application.ActiveDocument != null)
            {
                Api.Application.ActiveDocument.CurrentSelection.Changed -= OnCurrentSelectionChanged;
                _isSubscribedToCurrentSelectionChanged = false;
            }
        }

        private void OnApiApplicationActiveDocumentChanged(object sender, EventArgs e)
        {
            if (!_isSubscribedToCurrentSelectionChanged && Api.Application.ActiveDocument != null)
            {
                Api.Application.ActiveDocument.CurrentSelection.Changed += OnCurrentSelectionChanged;
                _isSubscribedToCurrentSelectionChanged = true;
            }
        }

        public void OnDockPaneVisibilityChanged(object sender, bool visible)
        {
            if (visible && !_isSubscribedToCurrentSelectionChanged)
            {
                Api.Application.ActiveDocument.CurrentSelection.Changed += Application.Document.OnCurrentSelectionChanged;
                _isSubscribedToCurrentSelectionChanged = true;
            }
            else if (!visible && _isSubscribedToCurrentSelectionChanged)
            {
                Application.Document.SelectedModelItems?.Clear();
                Api.Application.ActiveDocument.CurrentSelection.Changed -= Application.Document.OnCurrentSelectionChanged;
                _isSubscribedToCurrentSelectionChanged = false;
            }
        }

        #endregion Private Methods
    }
}