using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml;


namespace Midgard.CompositeCollection
{
    public sealed class CollectionContainer : DependencyObject, INotifyCollectionChanged
    {



        public ObservableCollection<object> Collection
        {
            get { return (ObservableCollection<object>)GetValue(CollectionProperty); }
            set { SetValue(CollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Collection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CollectionProperty =
            DependencyProperty.Register("Collection", typeof(ObservableCollection<object>), typeof(CollectionContainer), new PropertyMetadata(null, CollectionPropChanged));

        private static void CollectionPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as CollectionContainer;
            var oldValue = e.OldValue as INotifyCollectionChanged;
            var newValue = e.NewValue as INotifyCollectionChanged;

            if (oldValue != null)
                oldValue.CollectionChanged -= me.OnCollectionChanged;
            if (newValue != null)
                newValue.CollectionChanged += me.OnCollectionChanged;


        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(sender, e);
        }

    }
}
