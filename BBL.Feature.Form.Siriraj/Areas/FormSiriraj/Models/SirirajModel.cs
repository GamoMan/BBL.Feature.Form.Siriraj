using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BBL.Feature.Form.Siriraj.Areas.FormSiriraj.Models
{
    public class SirirajModel
    {

        public Personal personal { get; set; }

        public string CitizenID { get; set; }
        public string ClearCitizenID { get; set; }
        public int Car { get; set; }
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

    public class Personal
    {
        public int InvoiceID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(50)]
        public string CitizenID { get; set; }

        [StringLength(50)]
        public string Mobile1 { get; set; }

        [StringLength(50)]
        public string Mobile2 { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Address_CZ { get; set; }

        [StringLength(50)]
        public string Building_CZ { get; set; }

        [StringLength(50)]
        public string Soi_CZ { get; set; }
        [StringLength(50)]
        public string Road_CZ { get; set; }

        [StringLength(50)]
        public string Subdistrict_CZ { get; set; }

        [StringLength(50)]
        public string District_CZ { get; set; }

        [StringLength(50)]
        public string Province_CZ { get; set; }

        [StringLength(5)]
        public string Postcode_CZ { get; set; }

        public int Car { get; set; }
        public int Camera { get; set; }

        public int Radio { get; set; }

        public int Receipt { get; set; }

        public int DeliveryType { get; set; }

        [StringLength(10)]
        public string BranchCode { get; set; }

        [StringLength(50)]
        public string BranchName { get; set; }

        [StringLength(50)]
        public string BranchProvince { get; set; }

        [StringLength(50)]
        public string Address_Post { get; set; }

        [StringLength(50)]
        public string Building_Post { get; set; }

        [StringLength(50)]
        public string Soi_Post { get; set; }
        [StringLength(50)]
        public string Road_Post { get; set; }

        [StringLength(50)]
        public string Subdistrict_Post { get; set; }

        [StringLength(50)]
        public string District_Post { get; set; }

        [StringLength(50)]
        public string Province_Post { get; set; }

        [StringLength(5)]
        public string Postcode_Post { get; set; }     

        public float Amount { get; set; }

        //public float Donate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}