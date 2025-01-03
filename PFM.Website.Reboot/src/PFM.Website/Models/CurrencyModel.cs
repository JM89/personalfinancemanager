﻿using System.ComponentModel.DataAnnotations;

namespace PFM.Website.Models
{
	public class CurrencyModel
	{
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(3)]
        public string Symbol { get; set; }
    }
}

