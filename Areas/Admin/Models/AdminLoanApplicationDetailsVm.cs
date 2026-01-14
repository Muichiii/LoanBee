namespace LoanBee.Areas.Admin.Models
{
    public class AdminLoanApplicationDetailsVm
    {
        public Guid ApplicationNo { get; set; }
        public DateTime DateApplied { get; set; }
        public string Status { get; set; } = "";
        public int Amount { get; set; }
        public string Tenor { get; set; } = "";
        public string Purpose { get; set; } = "";

        // Owner
        public string OwnerTin { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public string OwnerGender { get; set; } = "";
        public DateTime OwnerBirthday { get; set; }
        public string OwnerAddress { get; set; } = "";
        public string OwnerPlaceOfBirth { get; set; } = "";
        public string OwnerCitizenship { get; set; } = "";
        public string OwnerCivilStatus { get; set; } = "";
        public string OwnerMobile { get; set; } = "";
        public string OwnerLandline { get; set; } = "";
        public string OwnerEmail { get; set; } = "";
        public string OwnerEducation { get; set; } = "";

        // Business
        public string BusinessTin { get; set; } = "";
        public string BusinessName { get; set; } = "";
        public string BusinessType { get; set; } = "";
        public string OfficeAddress { get; set; } = "";
        public string OfficeZip { get; set; } = "";
        public string BusinessLandline { get; set; } = "";
        public string BusinessMobile { get; set; } = "";
        public string BusinessEmail { get; set; } = "";
        public string BusinessWebsite { get; set; } = "";

        // Bank
        public string AccountNo { get; set; } = "";
        public string AccountType { get; set; } = "";
        public string RelationshipSince { get; set; } = "";
    }
}
