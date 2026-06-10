using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkInsert1
{
    public static class StaticMemberHelper
    {
        private static readonly DataTable _destinationTable;

        static StaticMemberHelper()
        {
            _destinationTable = new DataTable("Destinations");
            _destinationTable.Columns.Add("Name", typeof(string));
            _destinationTable.Columns.Add("Country", typeof(string));
            _destinationTable.Columns.Add("Description", typeof(string));
        }

        public static DataTable DestinationTable()
        {
            return _destinationTable.Clone();
        }
    }
}
