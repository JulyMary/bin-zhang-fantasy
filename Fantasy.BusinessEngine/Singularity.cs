using System;
using Fantasy.BusinessEngine.Collections;


namespace Fantasy.BusinessEngine
{
    public partial class Singularity : Fantasy.BusinessEngine.BusinessObject
    {
        public virtual BusinessObjectCollection<Fantasy.BusinessEngine.BusinessObject> Children
        {
            get
            {
                return GetCollection<Fantasy.BusinessEngine.BusinessObject>("Children");
            }
        }

    }

}