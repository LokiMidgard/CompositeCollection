using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;

namespace Midgard.CompositeCollection
{
    public class CompositeCollectionView : ObservableCollection<object>, ICollectionView
    {
        private CompositeCollection collection;


        private int currentIndex = -1;

        public CompositeCollectionView(CompositeCollection compositeCollection)
        {
            this.collection = compositeCollection;
            collection.CollectionChanged += Collection_CollectionChanged;
            collection.ContainedCollectionChanged += Collection_ContainedCollectionChanged;
        }

        private void Collection_ContainedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var index = collection.IndexOf(sender);
            var startIndex = this.collection.Take(index).Sum(x => x is CollectionContainer ? (x as CollectionContainer).Collection.Count : 1);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    startIndex += e.NewStartingIndex;
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        this.Insert(startIndex + i, e.NewItems[i]);
                    }


                    break;
                case NotifyCollectionChangedAction.Move:
                    Reset();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    startIndex += e.OldStartingIndex;
                    for (int i = e.OldItems.Count - 1; i >= 0; i--)
                    {
                        this.RemoveAt(startIndex + i);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    startIndex += e.OldStartingIndex;
                    for (int i = e.OldItems.Count - 1; i >= 0; i--)
                    {
                        this.RemoveAt(startIndex + i);
                    }
                    startIndex -= e.OldStartingIndex;
                    startIndex += e.NewStartingIndex;
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        this.Insert(startIndex + i, e.NewItems[i]);
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    Reset();
                    break;
                default:
                    Reset();
                    break;
            }





        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {

                        var startIndex = this.collection.Take(e.NewStartingIndex).Sum(x => x is CollectionContainer ? (x as CollectionContainer).Collection.Count : 1);
                        foreach (var item in e.NewItems)
                        {
                            var cc = item as CollectionContainer;
                            if (cc != null)
                            {
                                foreach (var c2 in cc.Collection)
                                {
                                    this.Add(c2);
                                }
                            }
                            else
                                this.Add(item);
                        }

                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    Reset();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {

                        var startIndex = this.collection.Take(e.OldStartingIndex).Sum(x => x is CollectionContainer ? (x as CollectionContainer).Collection.Count : 1);
                        var count = this.collection.Skip(e.OldStartingIndex).Take(e.OldItems.Count).Sum(x => x is CollectionContainer ? (x as CollectionContainer).Collection.Count : 1);
                        for (int i = startIndex + count - 1; i >= startIndex; i--)
                        {
                            this.RemoveAt(i);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    {

                        var startIndex = this.collection.Take(e.OldStartingIndex).Sum(x => x is CollectionContainer ? (x as CollectionContainer).Collection.Count : 1);
                        var count = this.collection.Skip(e.OldStartingIndex).Take(e.OldItems.Count).Sum(x => x is CollectionContainer ? (x as CollectionContainer).Collection.Count : 1);
                        for (int i = startIndex + count - 1; i >= startIndex; i--)
                        {
                            this.RemoveAt(i);
                        }
                    }
                    {

                        var startIndex = this.collection.Take(e.NewStartingIndex).Sum(x => x is CollectionContainer ? (x as CollectionContainer).Collection.Count : 1);
                        foreach (var item in e.NewItems)
                        {
                            var cc = item as CollectionContainer;
                            if (cc != null)
                            {
                                foreach (var c2 in cc.Collection)
                                {
                                    this.Add(c2);
                                }
                            }
                            else
                                this.Add(item);
                        }

                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Reset();
                    break;
                default:
                    Reset();
                    break;
            }
        }

        private void Reset()
        {
            this.Clear();
            foreach (var c1 in collection)
            {
                var cc = c1 as CollectionContainer;
                if (cc != null)
                    foreach (var c2 in cc.Collection)
                        this.Add(c2);
                else
                    this.Add(c1);
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {

            base.OnCollectionChanged(e);

            if (VectorChanged == null)
                return;

            var indexes = new List<uint>();
            CollectionChange collectionChange;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    collectionChange = CollectionChange.ItemInserted;
                    for (int i = 0; i < e.NewItems.Count; i++)
                        indexes.Add((uint)(e.NewStartingIndex + i));

                    break;
                case NotifyCollectionChangedAction.Move:
                    collectionChange = CollectionChange.Reset;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    collectionChange = CollectionChange.ItemRemoved;
                    for (int i = e.OldItems.Count - 1; i >= 0; i--)
                        indexes.Add((uint)(e.OldStartingIndex + i));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems.Count > 1)
                    {
                        collectionChange = CollectionChange.Reset;
                    }
                    else
                    {
                        collectionChange = CollectionChange.ItemChanged;
                        indexes.Add((uint)e.NewStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    collectionChange = CollectionChange.Reset;
                    break;
                default:
                    collectionChange = CollectionChange.Reset;
                    break;
            }
            if (indexes.Any())
            {
                foreach (var i in indexes)
                {
                    VectorChanged(this, new VectorChangedEventArgs() { CollectionChange = collectionChange, Index = i });

                }
            }
            else
                VectorChanged(this, new VectorChangedEventArgs() { CollectionChange = CollectionChange.Reset });

        }



        public IObservableVector<object> CollectionGroups
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object CurrentItem
        {
            get
            {
                if (currentIndex == -1)
                {
                    return null;
                }
                return this[currentIndex];
            }
        }

        public int CurrentPosition
        {
            get
            {
                return currentIndex;
            }
        }

        public bool HasMoreItems
        {
            get
            {
                return false;
            }
        }

        public bool IsCurrentAfterLast
        {
            get
            {
                return false;
            }
        }

        public bool IsCurrentBeforeFirst
        {
            get
            {
                return false;
            }
        }

        public event EventHandler<object> CurrentChanged;
        public event CurrentChangingEventHandler CurrentChanging;
        public event VectorChangedEventHandler<object> VectorChanged;

        public bool OnCurrentChanging()
        {
            var e = new CurrentChangingEventArgs(false);
            if (CurrentChanging != null)
                CurrentChanging(this, e);
            return !e.Cancel;
        }
        public void OnCurrentChanged()
        {
            var e = new EventArgs();
            if (CurrentChanged != null)
                CurrentChanged(this, e);
            
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.FromResult(new LoadMoreItemsResult() { Count = 0 }).AsAsyncOperation();
        }

        public bool MoveCurrentTo(object item)
        {
            if (!OnCurrentChanging())
                return currentIndex >= 0;

            currentIndex = this.IndexOf(item);

            OnCurrentChanged();

            return currentIndex >= 0;

        }

        public bool MoveCurrentToFirst()
        {
            if (!OnCurrentChanging())
                return currentIndex >= 0;

            if (this.Count >= 0)
                currentIndex = 0;
            else
                currentIndex = -1;

            OnCurrentChanged();
            return currentIndex >= 0;
        }

        public bool MoveCurrentToLast()
        {
            if (!OnCurrentChanging())
                return currentIndex >= 0;

            currentIndex = this.Count - 1;

            OnCurrentChanged();
            return currentIndex >= 0;
        }

        public bool MoveCurrentToNext()
        {
            if (!OnCurrentChanging())
                return currentIndex >= 0;

            currentIndex++;
            if (currentIndex >= this.Count)
                throw new IndexOutOfRangeException();

            OnCurrentChanged();
            return currentIndex >= 0;
        }

        public bool MoveCurrentToPosition(int index)
        {
            if (!OnCurrentChanging())
                return currentIndex >= 0;

            currentIndex = index;
            if (currentIndex >= this.Count)
                throw new IndexOutOfRangeException();

            OnCurrentChanged();
            return currentIndex >= 0;
        }

        public bool MoveCurrentToPrevious()
        {
            if (!OnCurrentChanging())
                return currentIndex >= 0;

            currentIndex--;
            if (currentIndex < 0)
                throw new IndexOutOfRangeException();

            OnCurrentChanged();
            return currentIndex >= 0;
        }
    }
}
