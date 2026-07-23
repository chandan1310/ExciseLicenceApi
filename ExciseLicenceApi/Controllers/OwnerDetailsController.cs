using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ExciseLicenceApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ExciseLicenceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerDetailsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OwnerDetailsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ====================================================================
        // 1. GET API PIPELINE
        // ====================================================================
        [HttpGet("get-owner-details/{applicantId:int}")]
        public async Task<IActionResult> GetOwnerDetails([FromRoute] int applicantId)
        {
            string connectionString = _configuration.GetConnectionString("sqlServrConnStrng")!;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("licenceapplicantMaster_owner_MB1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GET");
                        cmd.Parameters.AddWithValue("@ApplicantId", applicantId);

                        await conn.OpenAsync();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                var payloadList = dt.AsEnumerable().Select(row =>
                                    dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => row[col])
                                ).ToList();

                                return Ok(new { success = true, count = payloadList.Count, data = payloadList });
                            }
                        }
                    }
                }
                return NotFound(new { success = false, message = "No registered owner listings matched to this application key." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // ====================================================================
        // 2. POST API PIPELINE
        // ====================================================================
        [HttpPost("save-owner-entry")]
        public async Task<IActionResult> SaveOwnerEntry([FromBody] OwnerDetailsDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Model configurations properties structural state invalid." });

            string connectionString = _configuration.GetConnectionString("sqlServrConnStrng")!;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("licenceapplicantMaster_owner_MB1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "POST");

                        // Outer Frame Matrix Context Binding Mapping
                        cmd.Parameters.AddWithValue("@ApplicantId", model.ApplicantId);
                        cmd.Parameters.AddWithValue("@LicTypeId", model.LicTypeId);
                        cmd.Parameters.AddWithValue("@FinancialYear", model.FinancialYear);
                        cmd.Parameters.AddWithValue("@LicenceMode", model.LicenceMode);
                        cmd.Parameters.AddWithValue("@LicCategory", model.LicCategory);
                        cmd.Parameters.AddWithValue("@EstablishmentDate", (object)model.EstablishmentDate ?? DBNull.Value);

                        // Personal Grid Payload
                        cmd.Parameters.AddWithValue("@Nameprefix", (object)model.NamePrefix ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ApplicantName", model.ApplicantName);
                        cmd.Parameters.AddWithValue("@Gender", model.Gender);
                        cmd.Parameters.AddWithValue("@MaritalStatus", model.MaritalStatus);
                        cmd.Parameters.AddWithValue("@FatherName", (object)model.FatherName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HusbandName", (object)model.HusbandName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Relationship", (object)model.Relationship ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DOB", (object)model.DOB ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Age", (object)model.Age ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Nationality", (object)model.Nationality ?? "Indian");
                        cmd.Parameters.AddWithValue("@Designation", (object)model.Designation ?? "Individual");
                        cmd.Parameters.AddWithValue("@PanNo", model.PanNo);

                        // Present Address Block
                        cmd.Parameters.AddWithValue("@PresentStreetAddress", model.PresentAddress.StreetAddress);
                        cmd.Parameters.AddWithValue("@PresentHouseNo", model.PresentAddress.HouseNumber);
                        cmd.Parameters.AddWithValue("@Presentlandmark", model.PresentAddress.Landmark);
                        cmd.Parameters.AddWithValue("@PresentDistId", model.PresentAddress.DistrictId);
                        cmd.Parameters.AddWithValue("@PresentDistrictName", model.PresentAddress.DistrictName);
                        cmd.Parameters.AddWithValue("@PresentTehsilId", model.PresentAddress.TehsilId);
                        cmd.Parameters.AddWithValue("@PresentTehsilName", model.PresentAddress.TehsilName);
                        cmd.Parameters.AddWithValue("@PresentThanaId", model.PresentAddress.PoliceStationName);
                        cmd.Parameters.AddWithValue("@PresentR_UArea", model.PresentAddress.RuralUrbanArea);
                        cmd.Parameters.AddWithValue("@PresentVillageCityId", model.PresentAddress.VillageCityName);
                        cmd.Parameters.AddWithValue("@PresentZilaParishad", (object)model.PresentAddress.ZilaParishad ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PresentParishadName", (object)model.PresentAddress.ParishadName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PresentWardNo", (object)model.PresentAddress.CityWardNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PresentWardName", (object)model.PresentAddress.CityWardName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PresentPin", model.PresentAddress.Pincode);
                        cmd.Parameters.AddWithValue("@PresentPhn", (object)model.PresentAddress.TelephoneNumber ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PresentMobileNo", Convert.ToInt64(model.PresentAddress.MobileNumber));
                        cmd.Parameters.AddWithValue("@PresentEmail", model.PresentAddress.EmailAddress);

                        // Permanent Address Block
                        cmd.Parameters.AddWithValue("@PermanentStreetAddress", model.PermanentAddress.StreetAddress);
                        cmd.Parameters.AddWithValue("@PermanentHouseNo", model.PermanentAddress.HouseNumber);
                        cmd.Parameters.AddWithValue("@Permanentlandmark", model.PermanentAddress.Landmark);
                        cmd.Parameters.AddWithValue("@PermanentDistId", model.PermanentAddress.DistrictId);
                        cmd.Parameters.AddWithValue("@PermanentDistrictName", model.PermanentAddress.DistrictName);
                        cmd.Parameters.AddWithValue("@PermanentTehsilId", model.PermanentAddress.TehsilId);
                        cmd.Parameters.AddWithValue("@PermanentTehsilName", model.PermanentAddress.TehsilName);
                        cmd.Parameters.AddWithValue("@PermanentThanaId", model.PermanentAddress.PoliceStationName);
                        cmd.Parameters.AddWithValue("@PermanentR_UArea", model.PermanentAddress.RuralUrbanArea);
                        cmd.Parameters.AddWithValue("@PermanentVillageCityId", model.PermanentAddress.VillageCityName);
                        cmd.Parameters.AddWithValue("@PerZilaParishad", (object)model.PermanentAddress.ZilaParishad ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PerParishadName", (object)model.PermanentAddress.ParishadName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PerwardNo", (object)model.PermanentAddress.CityWardNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PerWardName", (object)model.PermanentAddress.CityWardName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PermanentPin", model.PermanentAddress.Pincode);
                        cmd.Parameters.AddWithValue("@PermanentPhn", (object)model.PermanentAddress.TelephoneNumber ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PermanentMobileNo", Convert.ToInt64(model.PermanentAddress.MobileNumber));
                        cmd.Parameters.AddWithValue("@PermanentEmail", model.PermanentAddress.EmailAddress);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                string responseMessage = reader["ResponseMessage"].ToString()!;
                                int assignedRecordId = Convert.ToInt32(reader["RecordId"]);

                                if (responseMessage == "SUCCESS")
                                {
                                    return Ok(new { success = true, message = "Owner array configuration block chunk saved correctly!", ownerId = assignedRecordId });
                                }
                                return BadRequest(new { success = false, message = responseMessage });
                            }
                        }
                    }
                }
                return BadRequest(new { success = false, message = "No data processing response returned from database frame trace." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }



        // ====================================================================
        // 1. GET ROUTE (Fetch Authorized Info by Applicant ID)
        // ====================================================================
        [HttpGet("get-authorized-details/{applicantId:int}")]
        public async Task<IActionResult> GetAuthorizedDetails([FromRoute] int applicantId)
        {
            string connectionString = _configuration.GetConnectionString("sqlServrConnStrng")!;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("licenceapplicantMaster_Autrized_MB1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GET");
                        cmd.Parameters.AddWithValue("@ApplicantId", applicantId);

                        await conn.OpenAsync();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                var responsePayload = dt.AsEnumerable().Select(row =>
                                    dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => row[col])
                                ).First();

                                return Ok(new { success = true, data = responsePayload });
                            }
                        }
                    }
                }
                return NotFound(new { success = false, message = "No authorized tracking details found matching this Applicant key context." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // ====================================================================
        // 2. POST ROUTE (Save Authorized Person Information Master)
        // ====================================================================
        [HttpPost("save-authorized-entry")]
        public async Task<IActionResult> SaveAuthorizedEntry([FromBody] AuthorizedDetailsDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Dynamic structural payload state tracking properties invalid." });

            string connectionString = _configuration.GetConnectionString("sqlServrConnStrng")!;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("licenceapplicantMaster_Autrized_MB1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "POST");

                        // Top Layer Context Headers Mapping Sync
                        cmd.Parameters.AddWithValue("@ApplicantId", model.ApplicantId);
                        cmd.Parameters.AddWithValue("@LicTypeId", model.LicTypeId);
                        cmd.Parameters.AddWithValue("@FinancialYear", model.FinancialYear);
                        cmd.Parameters.AddWithValue("@LicenceMode", model.LicenceMode);
                        cmd.Parameters.AddWithValue("@LicCategory", model.LicCategory);
                        cmd.Parameters.AddWithValue("@EstablishmentDate", (object)model.EstablishmentDate ?? DBNull.Value);

                        // Personal Specific Mappings Base Blocks
                        cmd.Parameters.AddWithValue("@Nameprefix", (object)model.NamePrefix ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ApplicantName", model.ApplicantName);
                        cmd.Parameters.AddWithValue("@Gender", model.Gender);
                        cmd.Parameters.AddWithValue("@MaritalStatus", model.MaritalStatus);
                        cmd.Parameters.AddWithValue("@FatherName", (object)model.FatherName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HusbandName", (object)model.HusbandName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Relationship", (object)model.Relationship ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DOB", (object)model.DOB ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Age", (object)model.Age ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Nationality", (object)model.Nationality ?? "Indian");
                        cmd.Parameters.AddWithValue("@Designation", (object)model.Designation ?? "व्यक्तिगत(Individual)");
                        cmd.Parameters.AddWithValue("@PanNo", model.PanNo);

                        // Present Form Inputs Mapping Group Node
                        cmd.Parameters.AddWithValue("@PresentStreetAddress", model.PresentAddress.StreetAddress);
                        cmd.Parameters.AddWithValue("@PresentHouseNo", model.PresentAddress.HouseNumber);
                        cmd.Parameters.AddWithValue("@Presentlandmark", model.PresentAddress.Landmark);
                        cmd.Parameters.AddWithValue("@PresentDistId", model.PresentAddress.DistrictId);
                        cmd.Parameters.AddWithValue("@PresentDistrictName", model.PresentAddress.DistrictName);
                        cmd.Parameters.AddWithValue("@PresentTehsilId", model.PresentAddress.TehsilId);
                        cmd.Parameters.AddWithValue("@PresentTehsilName", model.PresentAddress.TehsilName);
                        cmd.Parameters.AddWithValue("@PresentThanaId", model.PresentAddress.PoliceStationName);
                        cmd.Parameters.AddWithValue("@PresentR_UArea", model.PresentAddress.RuralUrbanArea);
                        cmd.Parameters.AddWithValue("@PresentVillageCityId", model.PresentAddress.VillageCityName);
                        cmd.Parameters.AddWithValue("@PresentZilaParishad", (object)model.PresentAddress.ZilaParishad ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PresentParishadName", (object)model.PresentAddress.ParishadName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PresentWardNo", (object)model.PresentAddress.CityWardNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PresentWardName", (object)model.PresentAddress.CityWardName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PresentPin", model.PresentAddress.Pincode);
                        cmd.Parameters.AddWithValue("@PresentPhn", (object)model.PresentAddress.TelephoneNumber ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PresentMobileNo", Convert.ToInt64(model.PresentAddress.MobileNumber));
                        cmd.Parameters.AddWithValue("@PresentEmail", model.PresentAddress.EmailAddress);

                        // Permanent Form Inputs Mapping Group Node
                        cmd.Parameters.AddWithValue("@PermanentStreetAddress", model.PermanentAddress.StreetAddress);
                        cmd.Parameters.AddWithValue("@PermanentHouseNo", model.PermanentAddress.HouseNumber);
                        cmd.Parameters.AddWithValue("@Permanentlandmark", model.PermanentAddress.Landmark);
                        cmd.Parameters.AddWithValue("@PermanentDistId", model.PermanentAddress.DistrictId);
                        cmd.Parameters.AddWithValue("@PermanentDistrictName", model.PermanentAddress.DistrictName);
                        cmd.Parameters.AddWithValue("@PermanentTehsilId", model.PermanentAddress.TehsilId);
                        cmd.Parameters.AddWithValue("@PermanentTehsilName", model.PermanentAddress.TehsilName);
                        cmd.Parameters.AddWithValue("@PermanentThanaId", model.PermanentAddress.PoliceStationName);
                        cmd.Parameters.AddWithValue("@PermanentR_UArea", model.PermanentAddress.RuralUrbanArea);
                        cmd.Parameters.AddWithValue("@PermanentVillageCityId", model.PermanentAddress.VillageCityName);
                        cmd.Parameters.AddWithValue("@PerZilaParishad", (object)model.PermanentAddress.ZilaParishad ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PerParishadName", (object)model.PermanentAddress.ParishadName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PerwardNo", (object)model.PermanentAddress.CityWardNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PerWardName", (object)model.PermanentAddress.CityWardName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PermanentPin", model.PermanentAddress.Pincode);
                        cmd.Parameters.AddWithValue("@PermanentPhn", (object)model.PermanentAddress.TelephoneNumber ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PermanentMobileNo", Convert.ToInt64(model.PermanentAddress.MobileNumber));
                        cmd.Parameters.AddWithValue("@PermanentEmail", model.PermanentAddress.EmailAddress);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return Ok(new { success = true, message = "Authorized details system stack compiled successfully!", recordId = Convert.ToInt32(reader["RecordId"]) });
                            }
                        }
                    }
                }
                return BadRequest(new { success = false, message = "Execution terminated without DB logging output trace return context." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }

}