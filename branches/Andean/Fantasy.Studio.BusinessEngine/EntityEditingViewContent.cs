using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.ComponentModel;
using Fantasy.Windows;

namespace Fantasy.Studio.BusinessEngine
{
    public abstract class EntityEditingViewContent : MultiPageEditingViewContent
    {

        private INamedBusinessEntity _entity;

        public override void Load(object entity)
        {
            base.Load(entity);

            if ((entity is INamedBusinessEntity))
            {
                _entity = (INamedBusinessEntity)entity;
                this.Title = _entity.Name;

                if (_entity is INotifyPropertyChanged)
                {
                    this._nameListener = new WeakEventListener((type, sender, e) =>
                    {
                        this.Title = _entity.Name;
                        return true;
                    });

                    PropertyChangedEventManager.AddListener(((INotifyPropertyChanged)this._entity), this._nameListener, "Name");
                }
            }
        }

        private WeakEventListener _nameListener;

    }
}
