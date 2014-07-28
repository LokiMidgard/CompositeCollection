using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace Midgard.CompositeCollection
{
    [ContentProperty(Name = "Composition")]
    public class CompositeCollection : ObservableCollection<object>, IList<object>
    {

        private readonly Func<object, int> sum = x =>
                        {
                            var cc = x as CollectionContainer;
                            if (cc != null)
                            {
                                if (cc.Collection == null)
                                    return 0;

                                return cc.Collection.Cast<object>().Count();
                            }
                            else
                                return 1;
                        };




        public ObservableCollection<object> Composition
        {
            get { return composition; }
        }



        private void Collection_ContainedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var index = composition.IndexOf(sender);
            var startIndex = composition.Take(index).Sum(sum);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    startIndex += e.NewStartingIndex;
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        Insert(startIndex + i, e.NewItems[i]);
                    }


                    break;
                case NotifyCollectionChangedAction.Move:
                    Reset();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    startIndex += e.OldStartingIndex;
                    for (int i = e.OldItems.Count - 1; i >= 0; i--)
                    {
                        RemoveAt(startIndex + i);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    startIndex += e.OldStartingIndex;
                    for (int i = e.OldItems.Count - 1; i >= 0; i--)
                    {
                        RemoveAt(startIndex + i);
                    }
                    startIndex -= e.OldStartingIndex;
                    startIndex += e.NewStartingIndex;
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        Insert(startIndex + i, e.NewItems[i]);
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



        private void CompositionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {

                        var startIndex = Composition.Cast<object>().Take(e.NewStartingIndex).Sum(sum);
                        foreach (var item in e.NewItems)
                        {
                            var cc = item as CollectionContainer;
                            if (cc != null)
                            {
                                cc.CollectionChanged += Collection_ContainedCollectionChanged;
                                if (cc.Collection != null)
                                    foreach (var c2 in cc.Collection)
                                    {
                                        Add(c2);
                                    }
                            }
                            else
                                Add(item);
                        }

                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    Reset();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {


                        foreach (var item in e.OldItems)
                        {
                            var cc = item as CollectionContainer;
                            if (cc != null)
                            {
                                cc.CollectionChanged -= Collection_ContainedCollectionChanged;
                            }
                        }

                        var startIndex = Composition.Cast<object>().Take(e.OldStartingIndex).Sum(sum);
                        var count = Composition.Cast<object>().Skip(e.OldStartingIndex).Take(e.OldItems.Count).Sum(sum);
                        for (int i = startIndex + count - 1; i >= startIndex; i--)
                        {
                            RemoveAt(i);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    {
                        foreach (var item in e.OldItems)
                        {
                            var cc = item as CollectionContainer;
                            if (cc != null)
                            {
                                cc.CollectionChanged -= Collection_ContainedCollectionChanged;
                            }
                        }
                        foreach (var item in e.NewItems)
                        {
                            var cc = item as CollectionContainer;
                            if (cc != null)
                            {
                                cc.CollectionChanged += Collection_ContainedCollectionChanged;
                            }
                        }

                        var startIndex = Composition.Cast<object>().Take(e.OldStartingIndex).Sum(sum);
                        var count = Composition.Cast<object>().Skip(e.OldStartingIndex).Take(e.OldItems.Count).Sum(sum);
                        for (int i = startIndex + count - 1; i >= startIndex; i--)
                        {
                            RemoveAt(i);
                        }
                    }
                    {

                        var startIndex = Composition.Cast<object>().Take(e.NewStartingIndex).Sum(sum);
                        foreach (var item in e.NewItems)
                        {
                            var cc = item as CollectionContainer;
                            if (cc != null)
                            {
                                foreach (var c2 in cc.Collection)
                                {
                                    Add(c2);
                                }
                            }
                            else
                                Add(item);
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

            foreach (var item in this)
            {
                var cc = item as CollectionContainer;
                if (cc != null)
                {
                    cc.CollectionChanged -= Collection_ContainedCollectionChanged;
                }
            }

            Clear();
            if (Composition != null)
            {
                foreach (var item in Composition)
                {
                    var cc = item as CollectionContainer;
                    if (cc != null)
                    {
                        cc.CollectionChanged += Collection_ContainedCollectionChanged;
                    }
                    Add(item);
                }
            }
        }

        private readonly ObservableCollection<object> composition;



        public CompositeCollection()
        {
            composition = new ObservableCollection<object>();
            composition.CollectionChanged += CompositionChanged;
        }





    }

}
