using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_ExecuteScalarSP
{
    public class QuotationContentTagValueSet
    {
        public int Id { get; set; }
        public int QuotationTemplateVersionSetId { get; set; }
        public int ContentTagId { get; set; }
        public string Value { get; set; }
        public string TextValue { get; set; }
    }
}
