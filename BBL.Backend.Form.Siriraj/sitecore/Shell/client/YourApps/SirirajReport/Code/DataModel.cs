using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BBL.Backend.Form.Siriraj.sitecore.Shell.client.YourApps.SirirajReport.Renderings
{
    public class DataModel
    {
        public class SirirajModel
        {
            [Required]
            [StringLength(50)]
            public string Name { get; set; }

            [Required]
            [StringLength(50)]
            public string Surname { get; set; }

            [Required]
            [StringLength(11)]
            public string Mobile1 { get; set; }

            [Required]
            [StringLength(11)]
            public string Mobile2 { get; set; }

            [Required]
            [StringLength(50)]
            public string Email { get; set; }

            [Required]
            [StringLength(13)]
            public string CitizenID { get; set; }

        }
    }
}