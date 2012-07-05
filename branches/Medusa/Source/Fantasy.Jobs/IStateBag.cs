using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs
{
    public interface IStateBag : IEnumerable<StateBagItem>
    {
        object this[string name]{get; set;}

        int Count { get; }

        string[] Names { get; }

        void Remove(string name);

        bool ContainsState(string name);

        void Clear();
    }

}
