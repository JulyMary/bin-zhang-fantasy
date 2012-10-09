using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org._12306.Tickets.Pooling
{
    //站位值类
    [Serializable]
    public class AvailableRange
    {
        public AvailableRange()
        {
            this.Tickets = new List<AvailableTicket>();
        }

        public int Value { get; set; }
        public List<AvailableTicket> Tickets { get; private set; }
    }
}
