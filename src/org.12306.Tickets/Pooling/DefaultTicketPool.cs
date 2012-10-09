using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Org._12306.Tickets.Pooling
{
   
    public class DefaultTicketPool
    {

        public DefaultTicketPool(int stopCount, int seatCount)
        {
            this.StopCount = stopCount;
            this.StopRangeCount = stopCount - 1;
            this.SeatCount = seatCount;

            AvailableRange fullRange = new AvailableRange() { Value = CreateStopRangeValue(0, stopCount - 1) };
            StopRanges.Add(fullRange);

            for (int i = 0; i < seatCount; i++)
            {
                fullRange.Tickets.Add(new AvailableTicket() { SeatNo = i, Range = fullRange.Value});
            }

            CheckMemoryUsage();
        }

        public int SeatCount { get; private set; }
        private int StopRangeCount { get; set; }
        public int StopCount { get; private set; }

     

        private List<AvailableRange> _stopRanges = new List<AvailableRange>();
        public List<AvailableRange> StopRanges
        {
            get
            {
                return _stopRanges;
            }
        }

        [NonSerialized]
        private long _memoryUsage = 0;
        public long MemoryUsage
        {
            get
            {
                return _memoryUsage;
            }
        }
       

        private HashSet<int> _soldOutRange = new HashSet<int>();
        
        [NonSerialized]
        private List<double> _visitCount = new List<double>(20000);
        public List<double> VisitCount
        {
            get
            {
                return _visitCount;
            }
        }

        private void CheckMemoryUsage()
        {
            //MemoryStream ms = new MemoryStream();

            //BinaryFormatter bformatter = new BinaryFormatter();

          
            //bformatter.Serialize(ms, this);

            //this._memoryUsage = Math.Max(this.MemoryUsage, ms.Length);

        }

        /// <summary>
        /// Buy a ticket.
        /// </summary>
        /// <param name="beginId">zero based start stop id</param>
        /// <param name="endId">zero based destination stop id</param>
        /// <returns>A ticket or null</returns>
        public SoldTicket Buy(int beginId, int endId)
        {
            int beginRange = beginId;
            int endRange = endId - 1;
           
            SoldTicket rs = null;
            //Get ticket stop range value.
            int stopRangeValue = CreateStopRangeValue(beginRange, endRange);

            if (_soldOutRange.Contains(stopRangeValue))
            {
               
                return null;
            }
           
            for(int i = 0; i < this.StopRanges.Count; i ++ )
            {
                AvailableRange range = this.StopRanges[i];
                if (range.Tickets.Count > 0 && ((range.Value & stopRangeValue) == stopRangeValue))
                {
                    // Generate ticket
                    AvailableTicket at = range.Tickets[0];
                    range.Tickets.RemoveAt(0);
                    rs = new SoldTicket() { Range = stopRangeValue, SeatNo = at.SeatNo, From=beginId, To=endId };
                    this._visitCount.Add(i);
                    if (range.Tickets.Count == 0)
                    {
                        this.StopRanges.RemoveAt(i);
                    }

                    // Generate fist available ticket

                    int rangeValue1 = CreateStopRangeValue(0, (beginRange - 1));
                   
                    rangeValue1 &= range.Value;

                    if (rangeValue1 > 0)
                    {
                        AvailableTicket newAt = new AvailableTicket() { Range = rangeValue1, SeatNo = at.SeatNo };

                        this.AddAvailableTicket(newAt); 
                    }
                    // Generate second available ticket

                    int rangeValue2 = CreateStopRangeValue((endRange + 1), this.StopRangeCount - 1);
                    rangeValue2 &= range.Value;
                    if (rangeValue2 > 0)
                    {
                        AvailableTicket newAt = new AvailableTicket() { Range = rangeValue2, SeatNo = at.SeatNo };
                        this.AddAvailableTicket(newAt); 
                    }

                    ////Remove range which contains no available ticket.
                    CheckMemoryUsage();
                   
                   
                    return rs;
                }
            }
            this.VisitCount.Add(this.StopRanges.Count);
          
            _soldOutRange.Add(stopRangeValue);
            CheckMemoryUsage();
            return null;
        }

        private class StopRangeComparer : IComparer<AvailableRange>
        {
            #region IComparer<StopRange> Members

            public int Compare(AvailableRange x, AvailableRange y)
            {
                return System.Collections.Comparer.Default.Compare(x.Value, y.Value);
            }

            #endregion

            public static StopRangeComparer Default = new StopRangeComparer();
        }

        private void AddAvailableTicket(AvailableTicket ticket)
        {
            AvailableRange stopRange;

            int pos = this.StopRanges.BinarySearchBy(ticket.Range, r => r.Value);

            if (pos >= 0)
            {
                stopRange = this.StopRanges[pos];
            }
            else
            {
                stopRange = new AvailableRange() { Value = ticket.Range };
                this.StopRanges.Insert(~pos, stopRange);
            }
            stopRange.Tickets.Add(ticket);

        }

        private  int CreateStopRangeValue(int beginStop, int endStop)
        {
            if (beginStop > endStop)
            {
                return 0;
            }
            int stopRangeValue = 0;
            for (int i = beginStop; i <= endStop; i++)
            {
                stopRangeValue += (int)Math.Pow(2, i);
            }
            return stopRangeValue;
        }

       
    }
}
