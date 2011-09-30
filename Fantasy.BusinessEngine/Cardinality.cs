using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Fantasy.BusinessEngine
{
    public struct Cardinality
    {

        public Cardinality(string expression)
        {
            Match match = Regex.Match(expression, @"^(?<l>\d+|\*)(\.\.(?<h>\d+|\*))?$");
            if (match.Success)
            {
                _minimum = Parse(match.Groups["l"].Value);
                if (match.Groups["h"].Success)
                {
                    _maximum = Parse(match.Groups["h"].Value);
                }
                else
                {
                    _maximum = _minimum;
                }
            }
            else
            {
                throw new ArgumentException("Invalid cardinality expression", expression);
            }
        }

        private static long Parse(string s)
        {
            return s == "*" ? Int64.MaxValue : Int64.Parse(s); 
        }


        public static bool operator ==(Cardinality x, Cardinality y)
        {
            return x._minimum == y._minimum && x._maximum == y._maximum; 
        }

        public static bool operator != (Cardinality x, Cardinality y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return (this._minimum.GetHashCode() ^ this.Maximum.GetHashCode());
        }


        public override bool Equals(object obj)
        {
            if (!(obj is Cardinality))
            {
                return false;
            }
            Cardinality point = (Cardinality)obj;
            return ((point._minimum == this._minimum) && (point._maximum == this._maximum));
        }

        public Cardinality(long minimum, long maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        private long _minimum;

        public long Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }


        private long _maximum;

        public long Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }


        public override string ToString()
        {
            string low = _minimum != long.MaxValue ? _minimum.ToString() : "*";
            string high = _maximum != long.MaxValue ? _maximum.ToString() : "*";

            return low != high ? string.Format("{0}..{1}", low, high) : low;
        }

        public bool IsSingleton
        {
            get
            {
                return _minimum < 2 && _maximum < 2;
            }
        }




       
    }
}
