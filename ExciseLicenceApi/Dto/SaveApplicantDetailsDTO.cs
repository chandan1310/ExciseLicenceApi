using System;

namespace ExciseLicenceApi.DTOs
{   
    public class AddressBlockDTO
    {
        public string StreetAddress { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
        public string Landmark { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string TehsilName { get; set; } = string.Empty;
        public string PoliceStation { get; set; } = string.Empty;
        public string AreaType { get; set; } = "Urban";
        public string VillageCity { get; set; } = string.Empty;
        public string Parishad { get; set; } = string.Empty;
        public string ParishadName { get; set; } = string.Empty;
        public string CityWardNo { get; set; } = string.Empty;
        public string CityWardName { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public string TelephoneNumber { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class SaveApplicantDetailsDTO
    {
        public int WebformNo { get; set; } // Maps directly to ApplicantId
        public string ApplicantCategory { get; set; } = "Individual";
        public string FinancialYear { get; set; } = "2026-2027";

        // Individual Category Fields
        public string ApplicantNameTitle { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Gender { get; set; } = "Male";
        public string MaritalStatus { get; set; } = "Single";
        public string FatherSpouseName { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string Dob { get; set; } = string.Empty;
        public int Age { get; set; } = 25;
        public string Nationality { get; set; } = "Indian";
        public string IndividualPan { get; set; } = string.Empty;

        // Company Category Fields
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyPan { get; set; } = string.Empty;
        public string DateOfIncorporation { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;

        // Nested Address Blocks
        public AddressBlockDTO PresentAddress { get; set; } = new AddressBlockDTO();
        public AddressBlockDTO PermanentAddress { get; set; } = new AddressBlockDTO();
        public bool SameAsPresent { get; set; }
    }
    public class DocumentUploadModel
    {
        public int ApplicantId { get; set; }
        public int FileMasterId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
    public class BarShopDetailsDTO
    {
        public int ApplicantId { get; set; }
        public int LicTypeId { get; set; }
        public string FinancialYear { get; set; } = null!;
        public int ShopDistrictId { get; set; }
        public string ShopName { get; set; } = null!;
        public string ShopStreetAddress { get; set; } = null!;
        public string? ShopPhon { get; set; }
        public string ShopMailId { get; set; } = null!;
        public string TehsilName { get; set; } = null!;
        public string BlockName { get; set; } = null!;
        public string MandalName { get; set; } = null!;
        public string CircleName { get; set; } = null!;
        public string ThanaName { get; set; } = null!;
        public string SubdivisionName { get; set; } = null!;
        public string RuralUrbanArea { get; set; } = null!;
        public string TribalNonTribal { get; set; } = null!;
        public string VidhanSabhaName { get; set; } = null!;
        public string LoksabhaName { get; set; } = null!;
        public string? ShopWardNo { get; set; }
        public string? ShopWardName { get; set; }
        public string? ShopLandMark { get; set; }
        public string ShopVillageCityName { get; set; } = null!;
        public string? ZilaParishad { get; set; }
        public string? ParishadName { get; set; }
        public string NorthPlace { get; set; } = null!;
        public string SouthPlace { get; set; } = null!;
        public string EastPlace { get; set; } = null!;
        public string WestPlace { get; set; } = null!;
        public string GSTIN { get; set; } = null!;
        public string? BarNickName { get; set; }
        public string HotelFoodLicNo { get; set; } = null!;
        public string HotelPan { get; set; } = null!;
        public string NoOfStar { get; set; } = "0";
        public int NoOfRoom { get; set; }
        public string PopulationRangeId { get; set; } = "NA";
        public int ShopPin { get; set; }
        public int CrntPopulation { get; set; }
    }

}