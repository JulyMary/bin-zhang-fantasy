﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessEnumValue : BusinessEntity
    {

        public virtual BusinessEnum Enum
        {
            get
            {
                return (BusinessEnum)this.GetValue("Enum", 0);
            }
            set
            {
                this.SetValue("Enum", value);
            }
        }

        public virtual int Value
        {
            get
            {
                return (int)this.GetValue("Value", 0);
            }
            set
            {
                this.SetValue("Value", value);
            }
        }


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
    }
}