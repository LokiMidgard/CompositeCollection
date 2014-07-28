using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace TestApp
{
    class Viewmodel
    {

        public Viewmodel()
        {
            back.Add("0");
        }
        readonly ObservableCollection<Object> back = new ObservableCollection<Object>();

        public ObservableCollection<Object> Back
        {
            get
            {
                return back;
            }

        }

        public ICommand Add
        {
            get { return new MyClass(this); }
        }


        class MyClass : ICommand
        {
            private Viewmodel viewmodel;

            public MyClass(Viewmodel viewmodel)
            {
                this.viewmodel = viewmodel;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                viewmodel.back.Add(viewmodel.back.Count.ToString());
            }
        }
    }
}
