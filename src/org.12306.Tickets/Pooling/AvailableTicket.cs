using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org._12306.Tickets.Pooling
{
    [Serializable]
    public class AvailableTicket
    {
        public int SeatNo { get; set; }
        public int Range { get; set; }
    }
}
