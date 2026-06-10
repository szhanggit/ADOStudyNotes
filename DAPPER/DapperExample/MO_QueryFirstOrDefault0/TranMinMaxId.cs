using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_QueryFirstOrDefault0
{
    public class TranMinMaxId
    {
        int _minId;
        int _maxId;

        public TranMinMaxId()
        {

        }

        public TranMinMaxId(int MinId, int MaxId)
        {
            this._minId = MinId;
            this._maxId = MaxId;
        }
        public int MinId { get { return _minId; } set { _minId = value; } }
        public int MaxId { get { return _maxId; } set { _maxId = value; } }
    }
}
