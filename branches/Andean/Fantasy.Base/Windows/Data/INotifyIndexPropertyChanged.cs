using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.ComponentModel;


namespace Fantasy.Windows.Data
{

    public class IndexPropertyChangedArgs : EventArgs 
    {
        public IndexPropertyChangedArgs(object index, object value)
        {
            this.Index = index;
            this.Value = value;
        }

        public object Index { get; private set; }

        public object Value { get; private set; }
    }

    public interface INotifyIndexPropertyChanged
    {
        object GetIndexValue(object index);

        event EventHandler<IndexPropertyChangedArgs> IndexPropertyChanged;
    }

    public static class IndexProperty
    {
        public static IndexPropertyWrapper Item(INotifyIndexPropertyChanged owner, object index)
        {
            return new IndexPropertyWrapper(owner, index);
        }
    }


    public class IndexPropertyWrapper : INotifyPropertyChanged
    {

        internal IndexPropertyWrapper(INotifyIndexPropertyChanged owner, object index)
        {
            _owner = owner;
            _index = index;
            _owner.IndexPropertyChanged += new EventHandler<IndexPropertyChangedArgs>(OwnerIndexPropertyChanged);
        }

        void OwnerIndexPropertyChanged(object sender, IndexPropertyChangedArgs e)
        {
            if (e.Index == this._index)
            {
                this.OnPropertyChanged("Value");
            }
        }

        private INotifyIndexPropertyChanged _owner;

        private object _index;

        public object Value {
            get
            {
                return _owner.GetIndexValue(_index); 
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged(string propertyName)
        {

            if (this.PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }

       

    }
}
