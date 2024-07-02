using System;

namespace Core.Data.Models
{
    public class CompanyInfo : EntityBase
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public string InvoiceNoPrefix { get; set; } 
        public string QuoteNoPrefix { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string ShopNo { get; set; }
        public string StreetName { get; set; }
        public string PostCode { get; set; }
        public string Office { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string TermsAndCondition { get; set; }
        public string TermsAndConditionItemSale { get; set; }
        public string CompanyNumber { get; set; }
        public string VatNumber { get; set; }
        public string CardPercentage { get; set; }
        public bool IsVat { get; set; }
        public string WhitelistIP { get; set; }

    }
}
