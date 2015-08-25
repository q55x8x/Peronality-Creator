using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Personality_Creator.PersonaFiles.Scripts;
using System.IO;

namespace Personality_Creator.UI
{
    public partial class VocabFileEditor : UserControl
    {
        public string[] VocabItems
        {
            get
            {
                string[] items = new string[this.lstVocabItems.Items.Count];
                this.lstVocabItems.Items.CopyTo(items, 0);
                return items;
            }
        }

        public delegate void VocabItemCollectionChangedEventHandler(object sender, VocabItemsChangedEventArgs eventArgs);
        public event VocabItemCollectionChangedEventHandler VocabItemCollectionChanged;
        public VocabFileEditor()
        {
            InitializeComponent();
            this.VocabItemCollectionChanged += VocabFileEditor_VocabItemCollectionChanged;
        }

        private void VocabFileEditor_VocabItemCollectionChanged(object sender, VocabItemsChangedEventArgs e)
        {
            this.lblVocabItemCount.Text = $"VocabItems: {this.lstVocabItems.Items.Count}";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            removeSelectedItems();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            addItem(this.txtNewVocabItem.Text);
        }

        private void txtNewVocabItem_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                addItem(this.txtNewVocabItem.Text);
            }
        }

        private void lstVocabItems_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                removeSelectedItems();
            }
        }

        #region

        private void removeAllItems()
        {
            string[] removedItems = new string[this.lstVocabItems.Items.Count];
            this.lstVocabItems.Items.CopyTo(removedItems, 0);
            this.lstVocabItems.Items.Clear();

            this.VocabItemCollectionChanged(this, new VocabItemsChangedEventArgs()
                                                    {
                                                        AddedItems = new string[] { },
                                                        RemovedItems = removedItems,
                                                        Operation = VocabItemsChangedEventArgs.OperationType.Removed
                                                    });
        }

        private void removeSelectedItems()
        {
            List<string> removedItems = new List<string>();

            foreach (string item in this.lstVocabItems.SelectedItems)
            {
                if (this.lstVocabItems.Items.Contains(item))
                {
                    removedItems.Add(item);
                }
            }

            foreach(string item in removedItems) //again we can't change the collection inside a foreach that is looping through it
            {
                this.lstVocabItems.Items.Remove(item);
            }

            this.VocabItemCollectionChanged(this, new VocabItemsChangedEventArgs()
                                                    {
                                                        AddedItems = new string[] { },
                                                        RemovedItems = removedItems.ToArray(),
                                                        Operation = VocabItemsChangedEventArgs.OperationType.Removed
                                                    });
        }

        private void addItems(string[] items)
        {
            List<string> addedItems = new List<string>();

            foreach(string itemToAdd in items)
            {
                if(!lstVocabItems.Items.Contains(itemToAdd))
                {
                    this.lstVocabItems.Items.Add(itemToAdd);
                    addedItems.Add(itemToAdd);
                }
            }

            this.VocabItemCollectionChanged(this, new VocabItemsChangedEventArgs()
                                                    {
                                                        AddedItems = addedItems.ToArray(),
                                                        RemovedItems = new string[] { },
                                                        Operation = VocabItemsChangedEventArgs.OperationType.Added
                                                    });
        }

        private void removeItems(string[] items)
        {
            List<string> removedItems = new List<string>();

            foreach(string itemToRemove in items)
            {
                if(lstVocabItems.Items.Contains(itemToRemove))
                {
                    this.lstVocabItems.Items.Remove(itemToRemove);
                    removedItems.Add(itemToRemove);
                }
            }

            this.VocabItemCollectionChanged(this, new VocabItemsChangedEventArgs()
                                                    {
                                                        AddedItems = new string[] { },
                                                        RemovedItems = removedItems.ToArray(),
                                                        Operation = VocabItemsChangedEventArgs.OperationType.Removed
                                                    });
        }

        private void removeItem(string item)
        {
            if (!this.lstVocabItems.Items.Contains(item))
            {
                return;
            }

            this.lstVocabItems.Items.Remove(item);

            this.VocabItemCollectionChanged(this, new VocabItemsChangedEventArgs()
                                                    {
                                                        AddedItems = new string[] {  },
                                                        RemovedItems = new string[] { item },
                                                        Operation = VocabItemsChangedEventArgs.OperationType.Removed
                                                    });
        }

        private void addItem(string item)
        {
            //check for duplicate
            if(this.lstVocabItems.Items.Contains(item))
            {
                return;
            }

            this.lstVocabItems.Items.Add(item);

            this.VocabItemCollectionChanged(this, new VocabItemsChangedEventArgs()
                                                    {
                                                        AddedItems = new string[] { item },
                                                        RemovedItems = new string[] { },
                                                        Operation = VocabItemsChangedEventArgs.OperationType.Added
                                                    });
        }

        public void loadVocabfile(Vocabfile vocabfile)
        {
            addItems(File.ReadAllLines(vocabfile.File.FullName));
            this.lblVocabKexword.Text = vocabfile.Keyword;
        }

        #endregion

        #region eventArgsClass

        public class VocabItemsChangedEventArgs : EventArgs
        {
            private string[] addedItems;
            private string[] removedItems;
            private OperationType operation;

            public string[] AddedItems
            {
                get
                {
                    return addedItems;
                }

                set
                {
                    addedItems = value;
                }
            }

            public string[] RemovedItems
            {
                get
                {
                    return removedItems;
                }

                set
                {
                    removedItems = value;
                }
            }

            public OperationType Operation
            {
                get
                {
                    return operation;
                }

                set
                {
                    operation = value;
                }
            }

            public enum OperationType
            {
                Added,
                Removed,
                AddedAndRemoved
            }
        }

        #endregion

    }

}
