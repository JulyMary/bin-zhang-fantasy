using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org._12306.Tickets.Pooling
{
    [Serializable]
    public class SoldTicket
    {
        public int Range { get; set; }
        public int SeatNo { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }
}
