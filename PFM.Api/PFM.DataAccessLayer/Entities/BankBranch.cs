﻿using PFM.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.Entities
{
    public class BankBranch : PersistedEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public int BankId { get; set; }

        [ForeignKey("BankId")]
        public Bank Bank { get; set; }
    }
}
