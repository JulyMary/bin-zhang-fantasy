using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org._12306.Tickets.Pooling
{
    public interface ITicketPool
    {
        TicketPoolInfo GetPoolInfo();

        void Initialize(TicketPoolInitArgs args);

        void Uninitialize();

        SoldTicket[] Buy();

        SoldTicket[] QueryTickets();



    }
}
