using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Data;

namespace Midgard.CompositeCollection
{
    public class CompositeCollection : ObservableCollection<object>, ICollectionViewFactory
    {

        public event NotifyCollectionChangedEventHandler ContainedCollectionChanged;
        private void OnContainedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ContainedCollectionChanged != null)
                ContainedCollectionChanged(sender, e);
        }

        private void AddCollectionContainer(CollectionContainer cc)
        {
            cc.CollectionChanged += OnContainedCollectionChanged;

        }

        private void RemoveCollectionContainer(CollectionContainer cc)
        {
            cc.CollectionChanged+=  OnContainedCollectionChanged;

        }

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                var cc = item as CollectionContainer;
                if (cc != null)
                RemoveCollectionContainer(cc);
            }
            base.ClearItems();
        }

        protected override void InsertItem(int index, object item)
        {
            var cc = item as CollectionContainer;
            if (cc != null)
                AddCollectionContainer(cc);

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            var cc = this[index] as CollectionContainer;
            if (cc != null)
                RemoveCollectionContainer(cc);

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, object item)
        {
            var cc = this[index] as CollectionContainer;
            if (cc != null)
                RemoveCollectionContainer(cc);

            cc = item as CollectionContainer;
            if (cc != null)
                AddCollectionContainer(cc);
            base.SetItem(index, item);
        }


        public ICollectionView CreateView()
        {
            return new CompositeCollectionView(this);
        }




    }
}
