using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_ExecuteScalarSP
{
    public class QuotationTemplateVersionSet
    {
        public int Id { get; set; }
        public int ClientQuotationId { get; set; }
        public int LanguageId { get; set; }
        public int TemplateId { get; set; }
        public int TemplateVersionId { get; set; }
        public int TemplateType { get; set; }
        public bool IsEmail { get; set; }

        public List<QuotationContentTagValueSet> QuotationContentTagValueSets { get; set; } = new List<QuotationContentTagValueSet>();
    }
}
