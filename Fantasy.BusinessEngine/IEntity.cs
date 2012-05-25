using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Fantasy.BusinessEngine.Events;

namespace Fantasy.BusinessEngine
{
    public interface IEntity : INotifyPropertyChanged, INotifyPropertyChanging 
    {

        void OnCreate(EntityCreateEventArgs e);

        event EventHandler Create;

        void OnLoad(EventArgs e);
        event EventHandler Load;

        void OnInserting(CancelEventArgs e);
        event CancelEventHandler Inserting;

        void OnInserted(EventArgs e);
        event EventHandler Inserted;

        void OnUpdating(CancelEventArgs e);
        event CancelEventHandler Updating;

        void OnUpdated(EventArgs e);
        event EventHandler Updated;

        void OnDeleting(CancelEventArgs e);
        event CancelEventHandler Deleting;

        void OnDeleted(EventArgs e);
        event EventHandler Deleted;

        void OnPropertyChanging(EntityPropertyChangingEventArgs e);
        new event EventHandler<EntityPropertyChangingEventArgs> PropertyChanging;

        void OnPropertyChanged(EntityPropertyChangedEventArgs e);

        new event EventHandler<EntityPropertyChangedEventArgs> PropertyChanged;

        EntityState EntityState { get; set; }

        event EventHandler EntityStateChanged;

    }
}
