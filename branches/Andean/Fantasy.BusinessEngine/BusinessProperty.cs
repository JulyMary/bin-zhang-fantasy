using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Fantasy.BusinessEngine
{
    public class BusinessProperty : BusinessEntity
    {
        public virtual string Name
        {
            get
            {
                return (string)this.GetValue("Name", null);
            }
            set
            {
                this.SetValue("Name", value);
            }
        }


        public virtual BusinessClass Class
        {
            get
            {
                return (BusinessClass)this.GetValue("Class", null);
            }
            set
            {
                this.SetValue("Class", value);
            }
        }


        public virtual string CodeName
        {
            get
            {
                return (string)this.GetValue("CodeName", null);
            }
            set
            {
                this.SetValue("CodeName", value);
            }
        }

        public virtual BusinessDataType DataType
        {
            get
            {
                return (BusinessDataType)this.GetValue("DataType", null);
            }
            set
            {
                this.SetValue("DataType", value);
               
                
            }
        }

        public virtual event EventHandler DataTypeNameChanged;

        protected virtual void OnDataTypeNameChanged(EventArgs e)
        {
            if (this.DataTypeNameChanged != null)
            {
                this.DataTypeNameChanged(this, e);
            }
        }

        protected override void OnPropertyChanged(Events.EntityPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            switch (e.PropertyName)
            {
                case "DataType":
                case "DataClassType":
                case "DataEnumType":
                    OnNotifyPropertyChangedPropertyChanged("DataTypeName");
                    break;
            }
            
        }

        public virtual string DataTypeName
        {
            get
            {
                if (this.DataType != null)
                {
                    if (this.DataType.Id == BusinessDataType.WellknownIds.Class && this.DataClassType != null)
                    {
                        return this.DataClassType.FullName;
                    }
                    else if (this.DataType.Id == BusinessDataType.WellknownIds.Enum && this.DataEnumType != null)
                    {
                        return this.DataEnumType.FullName;
                    }
                    else
                    {
                        return this.DataType.Name;
                    }

                }
                else
                {
                    return null;
                }

            }
        }
       
       

        public virtual BusinessClass DataClassType
        {
            get
            {
                return (BusinessClass)this.GetValue("DataClassType", null);
            }
            set
            {
                this.SetValue("DataClassType", value);
               
            }
        }

        public virtual BusinessEnum DataEnumType
        {
            get
            {
                return (BusinessEnum)this.GetValue("DataEnumType", null);
            }
            set
            {
                this.SetValue("DataEnumType", value);
               
            }
        }

        public virtual string FieldName
        {
            get
            {
                return (string)this.GetValue("FieldName", null);
            }
            set
            {
                this.SetValue("FieldName", value);
            }
        }

        public virtual string FieldType
        {
            get
            {
                return (string)this.GetValue("FieldType", null);
            }
            set
            {
                this.SetValue("FieldType", value);
            }
        }

        public virtual int Length
        {
            get
            {
                return (int)this.GetValue("Length", 0);
            }
            set
            {
                this.SetValue("Length", value);
            }
        }

        public virtual int Precision
        {
            get
            {
                return (int)this.GetValue("Precision", 0);
            }
            set
            {
                this.SetValue("Precision", value);
            }
        }

        public virtual bool IsNullable
        {
            get
            {
                return (bool)this.GetValue("IsNullable", true);
            }
            set
            {
                this.SetValue("IsNullable", value);
            }
        }

        public virtual string DefaultValue
        {
            get
            {
                return (string)this.GetValue("DefaultValue", null);
            }
            set
            {
                this.SetValue("DefaultValue", value);
            }
        }

        public virtual bool IsCalculated
        {
            get
            {
                return (bool)this.GetValue("IsCalculated", false);
            }
            set
            {
                this.SetValue("IsCalculated", value);
            }
        }
    }
}
