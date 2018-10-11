using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace BBL.Feature.Form.Siriraj.Areas.FormSiriraj.Models
{
    public class SirirajModel
    {
        public string CitizenID { get; set; }
        public int Car  { get; set; }
        public int Camera { get; set; }
        public int Radio { get; set; }
        public int Receipt { get; set; }
        public int DeliveryType { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BranchProvince { get; set; }
        public string Barcode { get; set; }
        public string QRcode { get; set; }
        public float Amount { get; set; }
        public float Donate { get; set; }
        public string TaxID { get; set; }
        public string Suffix { get; set; }
    }

    public class Province
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string NameInThai { get; set; }
        public string NameInEnglish { get; set; }
        public List<Branch> Branches { get; set; }
    }
    public class Branch
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string NameInThai { get; set; }
        public string NameInEnglish { get; set; }

        [ForeignKey("ProvinceId")]
        public virtual Province Province { get; set; }
    }
}