﻿using Api.Contracts.Shared;

namespace PFM.Bank.Api.Contracts.Bank
{
    public class BankList : ICanBeDeleted
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CountryName { get; set; }

        public string IconPath { get; set; }

        public bool CanBeDeleted { get; set; }

        public string TooltipResourceName => "BankCantBeDeleted";
    }
}
