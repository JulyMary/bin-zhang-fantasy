using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Collections
{

    static class KeyHelper
    {
        public static int GetHashCode(object[] values)
        {
            int rs = 0;
            foreach (object o in values)
            {
                int h = o != null ? o.GetHashCode() : 0;
                rs ^= h;
            }

            return rs;
        }

        public static bool Equals(object[] values1, object[] values2)
        {
            for (int i = 0; i < values1.Length; i++)
            {
                if (!object.Equals(values1[i], values2[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public struct Key<T1, T2> 
    {
        public Key(T1 v1, T2 v2) 
        {
            _v1 = v1;
            _v2 = v2;
        }

        private T1 _v1;

        public T1 V1
        {
            get { return _v1; }
        }


        private T2 _v2;

        public T2 V2
        {
            get { return _v2; }
        }


        public override bool Equals(object obj)
        {
            if (obj is Key<T1, T2>)
            {
                Key<T1, T2> o = (Key<T1, T2>)obj;
                return KeyHelper.Equals(new object[] { this.V1, this.V2 }, new object[] { o.V1, o.V2 });
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return KeyHelper.GetHashCode(new object[] { this.V1, this.V2 }); 
        }

        public static bool operator ==(Key<T1, T2> x, Key<T1, T2> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Key<T1, T2> x, Key<T1, T2> y)
        {
            return !x.Equals(y);
        }
    }

    public struct Key<T1, T2, T3>
    {
        public Key(T1 v1, T2 v2, T3 v3)
        {
            _v1 = v1;
            _v2 = v2;
            _v3 = v3;
            
        }

        private T1 _v1;

        public T1 V1
        {
            get { return _v1; }
        }


        private T2 _v2;

        public T2 V2
        {
            get { return _v2; }
        }

        private T3 _v3;

        public T3 V3
        {
            get { return _v3; }
        }



        public override bool Equals(object obj)
        {
            if (obj is Key<T1, T2, T3>)
            {
                Key<T1, T2, T3> o = (Key<T1, T2, T3>)obj;
                return KeyHelper.Equals(new object[] { this.V1, this.V2, this.V3 }, new object[] { o.V1, o.V2, o.V3 });
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return KeyHelper.GetHashCode(new object[] { this.V1, this.V2, this.V3 });
        }

        public static bool operator ==(Key<T1, T2, T3> x, Key<T1, T2, T3> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Key<T1, T2, T3> x, Key<T1, T2, T3> y)
        {
            return !x.Equals(y);
        }
    }

    public struct Key<T1, T2, T3, T4>
    {
        public Key(T1 v1, T2 v2, T3 v3, T4 v4)
        {
            _v1 = v1;
            _v2 = v2;
            _v3 = v3;
            _v4 = v4;

        }

        private T1 _v1;

        public T1 V1
        {
            get { return _v1; }
        }


        private T2 _v2;

        public T2 V2
        {
            get { return _v2; }
        }

        private T3 _v3;

        public T3 V3
        {
            get { return _v3; }
        }

        private T4 _v4;

        public T4 V4
        {
            get { return _v4; }
        }




        public override bool Equals(object obj)
        {
            if (obj is Key<T1, T2, T3, T4>)
            {
                Key<T1, T2, T3, T4> o = (Key<T1, T2, T3, T4>)obj;
                return KeyHelper.Equals(new object[] { this.V1, this.V2, this.V3, this.V4 }, new object[] { o.V1, o.V2, o.V3, o.V4 });
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return KeyHelper.GetHashCode(new object[] { this.V1, this.V2, this.V3, this.V4 });
        }

        public static bool operator ==(Key<T1, T2, T3, T4> x, Key<T1, T2, T3, T4> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Key<T1, T2, T3, T4> x, Key<T1, T2, T3, T4> y)
        {
            return !x.Equals(y);
        }
    }

    public struct Key<T1, T2, T3, T4, T5>
    {
        public Key(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5)
        {
            _v1 = v1;
            _v2 = v2;
            _v3 = v3;
            _v4 = v4;
            _v5 = v5;
        }

        private T1 _v1;

        public T1 V1
        {
            get { return _v1; }
        }


        private T2 _v2;

        public T2 V2
        {
            get { return _v2; }
        }

        private T3 _v3;

        public T3 V3
        {
            get { return _v3; }
        }

        private T4 _v4;

        public T4 V4
        {
            get { return _v4; }
        }


        private T5 _v5;

        public T5 V5
        {
            get { return _v5; }
        }

        public override bool Equals(object obj)
        {
            if (obj is Key<T1, T2, T3, T4, T5>)
            {
                Key<T1, T2, T3, T4, T5> o = (Key<T1, T2, T3, T4, T5>)obj;
                return KeyHelper.Equals(new object[] { this.V1, this.V2, this.V3, this.V4, this.V5 }, new object[] { o.V1, o.V2, o.V3, o.V4, o.V5 });
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return KeyHelper.GetHashCode(new object[] { this.V1, this.V2, this.V3, this.V4, this.V5 });
        }

        public static bool operator ==(Key<T1, T2, T3, T4, T5> x, Key<T1, T2, T3, T4, T5> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Key<T1, T2, T3, T4, T5> x, Key<T1, T2, T3, T4, T5> y)
        {
            return !x.Equals(y);
        }
    }

   
}
