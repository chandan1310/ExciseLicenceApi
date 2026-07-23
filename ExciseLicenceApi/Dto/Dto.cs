namespace ExciseLicenceApi.Dto
{
    public class MB1RegistrationModel
    {
        public int Sno { get; set; }
        public string FinancialYear { get; set; }
        public string BName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string UdyankNo { get; set; }
        public string Ind_DistrictName { get; set; }
        public string Panno { get; set; }
        public string webform_no { get; set; }
    }

    public class OwnerDetailsDTO
    {
        public int ApplicantId { get; set; }
        public int LicTypeId { get; set; }
        public string FinancialYear { get; set; } = null!;
        public string LicenceMode { get; set; } = null!;
        public string LicCategory { get; set; } = null!;
        public string? EstablishmentDate { get; set; }
        public string? NamePrefix { get; set; }
        public string ApplicantName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string MaritalStatus { get; set; } = null!;
        public string? FatherName { get; set; }
        public string? HusbandName { get; set; }
        public string? Relationship { get; set; }
        public string? DOB { get; set; }
        public string? Age { get; set; }
        public string? Nationality { get; set; }
        public string? Designation { get; set; }
        public string PanNo { get; set; } = null!;
        public OwnerAddressBlockDTO PresentAddress { get; set; } = new OwnerAddressBlockDTO();
        public OwnerAddressBlockDTO PermanentAddress { get; set; } = new OwnerAddressBlockDTO();
    }

    public class OwnerAddressBlockDTO
    {
        public string StreetAddress { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public string Landmark { get; set; } = string.Empty;
        public int DistrictId { get; set; }
        public string DistrictName { get; set; } = string.Empty;
        public string TehsilId { get; set; } = string.Empty;
        public string TehsilName { get; set; } = string.Empty;
        public string PoliceStationName { get; set; } = string.Empty;
        public string RuralUrbanArea { get; set; } = string.Empty;
        public string VillageCityName { get; set; } = string.Empty;
        public string? ZilaParishad { get; set; }
        public string? ParishadName { get; set; }
        public string? CityWardNo { get; set; }
        public string? CityWardName { get; set; }
        public int Pincode { get; set; }
        public string? TelephoneNumber { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
    }


    public class AuthorizedDetailsDTO
    {
        public int ApplicantId { get; set; }
        public int LicTypeId { get; set; }
        public string FinancialYear { get; set; } = null!;
        public string LicenceMode { get; set; } = null!;
        public string LicCategory { get; set; } = null!;
        public string? EstablishmentDate { get; set; }
        public string? NamePrefix { get; set; }
        public string ApplicantName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string MaritalStatus { get; set; } = null!;
        public string? FatherName { get; set; }
        public string? HusbandName { get; set; }
        public string? Relationship { get; set; }
        public string? DOB { get; set; }
        public string? Age { get; set; }
        public string? Nationality { get; set; }
        public string? Designation { get; set; }
        public string PanNo { get; set; } = null!;
        public AuthAddressDTO PresentAddress { get; set; } = new AuthAddressDTO();
        public AuthAddressDTO PermanentAddress { get; set; } = new AuthAddressDTO();
    }

    public class AuthAddressDTO
    {
        public string StreetAddress { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public string Landmark { get; set; } = string.Empty;
        public int DistrictId { get; set; }
        public string DistrictName { get; set; } = string.Empty;
        public string TehsilId { get; set; } = string.Empty;
        public string TehsilName { get; set; } = string.Empty;
        public string PoliceStationName { get; set; } = string.Empty;
        public string RuralUrbanArea { get; set; } = string.Empty;
        public string VillageCityName { get; set; } = string.Empty;
        public string? ZilaParishad { get; set; }
        public string? ParishadName { get; set; }
        public string? CityWardNo { get; set; }
        public string? CityWardName { get; set; }
        public int Pincode { get; set; }
        public string? TelephoneNumber { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
    }

}
