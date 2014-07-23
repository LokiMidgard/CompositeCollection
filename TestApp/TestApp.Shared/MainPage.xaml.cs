using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace TestApp
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Frames navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Test();
        }

        private async void Test()
        {
            var coll = new Midgard.CompositeCollection.CompositeCollection();
            this.list.ItemsSource =  (coll as ICollectionViewFactory).CreateView();
            await new Windows.UI.Popups.MessageDialog("Test 1").ShowAsync();
            var cc = new Midgard.CompositeCollection.CollectionContainer();
            cc.Collection = new System.Collections.ObjectModel.ObservableCollection<object>(new object[] { "t1.1", "t1.2" });
            coll.Add(cc);
            await new Windows.UI.Popups.MessageDialog("Test 2").ShowAsync();
            cc.Collection.Insert(1,"t2");
            await new Windows.UI.Popups.MessageDialog("Test 3").ShowAsync();
            cc = new Midgard.CompositeCollection.CollectionContainer();
            cc.Collection = new System.Collections.ObjectModel.ObservableCollection<object>(new object[] { "t2.1", "t2.2" });
            coll.Add(cc);

        }
    }
}
