using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace FoxSec.Web.ViewModels
{
    public class HRmodel
    {
        public DataTable xmlTable { get; set; }
    }

    public class xmlHR
    {
        //public List<string> xmlFields { get; set; }
        public List<xmlfieldVal> xmlField { get; set; }
        //public List<string> xmlField { get; set; }
        public List<xmlfieldVal> xmlRowVal { get; set; }
    }

    public class xmlfieldVal
    {
        public int ColumnIndex { get; set; }
        public string columnName { get; set; }
        public string ColVal { get; set; }
    }

    public class HRtableFields
    {
        public string ColumnName { get; set; }
        public bool is_nullable { get; set; }
        public string Datatype { get; set; }
    }

}