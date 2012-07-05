using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
namespace Fantasy.Jobs
{
    [XSerializable("status", NamespaceUri=Consts.XNamespaceURI)]
    public class RuntimeStatus
    {
        [XArray(Name="stack")]
        [XArrayItem(Name = "stack.frame", Type = typeof(StateBag))]
        private List<StateBag> _stack = new List<StateBag>();


        public void PushStack()
        {
            _position++;
            if (this._stack.Count <= _position)
            {
                this._stack.Add(new StateBag());
            }
        }

        public void PopStack()
        {
            if (this._stack.Count > 0)
            {
                this._stack.RemoveRange(_position, this._stack.Count - _position);
                _position--;
            }
        }

        public bool IsRestoring
        {
            get
            {
                return _position < this._stack.Count - 1;
            }
        }

        private int _position = -1;

        public StateBag Local
        {
            get
            {
                return _position < this._stack.Count ? this._stack[this._position] : null;
            }
        }

        [XElement("heap")]
        private StateBag _global = new StateBag();
        public StateBag Global
        {
            get
            {
                return _global; 
            }
        }

        public bool TryGetValue(string name, out object value)
        {
            value = null;
            bool rs = false;
            for(int i = this._position; i >= 0; i --)
            {
                StateBag bag = this._stack[i]; 
                if (bag.ContainsState(name))
                {
                    value = bag[name];
                    
                    rs = true;
                    break;
                }
            }
            if (!rs)
            {
                if (this.Global.ContainsState(name))
                {
                    value = this.Global[name];
                  
                    rs = true;
                }
            }

            return rs;
        }
    }
}
