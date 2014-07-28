using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Midgard.CompositeCollection
{
    [ContentProperty(Name = "Collection")]
    public   class CollectionContainer : DependencyObject, INotifyCollectionChanged
    {


        public IEnumerable Collection
        {
            get { return (IEnumerable)GetValue(CollectionProperty); }
            set { SetValue(CollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Collection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CollectionProperty =
            DependencyProperty.Register("Collection", typeof(IEnumerable), typeof(CollectionContainer), new PropertyMetadata(null, CollectioPropertyChanged));

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private static void CollectioPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as CollectionContainer;
            var oldValue = e.OldValue as INotifyCollectionChanged;
            var newValue = e.NewValue as INotifyCollectionChanged;

            if (oldValue != null)
            {
                oldValue.CollectionChanged -= me.OnCollectionChanged;
            }
            if (newValue != null)
            {
                newValue.CollectionChanged += me.OnCollectionChanged;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }
    }


}
