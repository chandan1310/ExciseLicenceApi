using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ExciseLicenceApi.Data;
using ExciseLicenceApi.Models;
using ExciseLicenceApi.DTOs; // FIX: Brings in SaveApplicantDetailsDTO namespace reference
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExciseLicenceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChallanCheckController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration; // FIX: Defined missing configuration field 

        // FIX: Injected IConfiguration interface via Dependency Injection
        public ChallanCheckController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/ChallanCheck
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChallanCheck>>> GetChallans([FromQuery] string challanNo = null)
        {
            IQueryable<ChallanCheck> query = _context.ChallanChecks;

            if (!string.IsNullOrEmpty(challanNo))
            {
                query = query.Where(c => c.ChallanNo == challanNo);
            }

            var results = await query.ToListAsync();
            return Ok(results);
        }


        // ==========================================
        // 1. GET API PIPELINE (Enforced Integer Constraints)
        // ==========================================
        [HttpGet("get-applicant-details/{webformNo:int}")] // <-- Added ':int' constraint here
        public async Task<IActionResult> GetApplicantDetails([FromRoute] int webformNo) // <-- Explicitly added [FromRoute]
        {
            string connectionString = _configuration.GetConnectionString("sqlServrConnStrng")!;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Insert_LicenceApplicantMaster_MB1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GET");
                        cmd.Parameters.AddWithValue("@WebformNo", webformNo);

                        await conn.OpenAsync();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Columns.Contains("ResponseMessage") && dt.Rows[0]["ResponseMessage"].ToString() == "NOT_FOUND")
                                {
                                    return NotFound(new { success = false, message = "No records found matching this tracking key." });
                                }

                                var responsePayload = dt.AsEnumerable()
                                    .Select(row => dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => row[col]))
                                    .First();

                                return Ok(new { success = true, data = responsePayload });
                            }
                        }
                    }
                }
                return NotFound(new { success = false, message = "Application context record is empty." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }




        // ==========================================
        // 1. GET ALL API PIPELINE (By Financial Year)
        // ==========================================
        [HttpGet("get-applicant-details-by-year/{financialYear}")]
        public async Task<IActionResult> GetApplicantDetailsByYear([FromRoute] string financialYear)
        {
            if (string.IsNullOrEmpty(financialYear))
                return BadRequest(new { success = false, message = "Financial Year route parameter is missing." });

            string connectionString = _configuration.GetConnectionString("sqlServrConnStrng")!;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Note: Reuse your base SP or execute an explicit query matching your financial year criteria
                    using (SqlCommand cmd = new SqlCommand("sp_Insert_LicenceApplicantMaster_MB1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GET_BY_YEAR"); // New routing target action tag for your SP
                        cmd.Parameters.AddWithValue("@WebformNo", DBNull.Value); // Bypassing WebformNo tracking key context
                        cmd.Parameters.AddWithValue("@FinancialYear", financialYear);

                        await conn.OpenAsync();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                // Convert all matched rows into a scannable JSON list payload layout array
                                var responsePayload = dt.AsEnumerable()
                                    .Select(row => dt.Columns.Cast<DataColumn>()
                                        .ToDictionary(col => col.ColumnName, col => row[col]))
                                    .ToList(); // FIX: Changed from .First() to .ToList() to return all records

                                return Ok(new { success = true, count = responsePayload.Count, data = responsePayload });
                            }
                        }
                    }
                }
                return NotFound(new { success = false, message = $"No application master records found for Financial Year {financialYear}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }



        // ==========================================
        // 2. POST API PIPELINE
        // ==========================================
        [HttpPost("save-applicant-step")]
        public async Task<IActionResult> SaveApplicantStep([FromBody] SaveApplicantDetailsDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Model state requirements invalid." });

            string connectionString = _configuration.GetConnectionString("sqlServrConnStrng")!;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Insert_LicenceApplicantMaster_MB1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "POST");

                        cmd.Parameters.AddWithValue("@WebformNo", model.WebformNo);
                        cmd.Parameters.AddWithValue("@FinancialYear", model.FinancialYear ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ApplicantCategory", model.ApplicantCategory ?? (object)DBNull.Value);

                        // Individual params
                        cmd.Parameters.AddWithValue("@ApplicantNameTitle", (object)model.ApplicantNameTitle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FirstName", (object)model.FirstName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Gender", (object)model.Gender ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaritalStatus", (object)model.MaritalStatus ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FatherSpouseName", (object)model.FatherSpouseName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Relationship", (object)model.Relationship ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Dob", (object)model.Dob ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Age", model.Age != null ? (object)model.Age.ToString() : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Nationality", (object)model.Nationality ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@IndividualPan", (object)model.IndividualPan ?? DBNull.Value);

                        // Company params
                        cmd.Parameters.AddWithValue("@CompanyName", (object)model.CompanyName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CompanyPan", (object)model.CompanyPan ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DateOfIncorporation", (object)model.DateOfIncorporation ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@RegistrationNumber", (object)model.RegistrationNumber ?? DBNull.Value);

                        // Address params (Safely Handling String-to-Int mapping for SQL IDs)
                        cmd.Parameters.AddWithValue("@StreetAddress", model.PresentAddress.StreetAddress ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BuildingNumber", model.PresentAddress.BuildingNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Landmark", model.PresentAddress.Landmark ?? (object)DBNull.Value);

                        // In fields me agar string numeric ID hai (jaise "12"), toh ye int me convert ho jayegi, warna DBNull jayega
                        cmd.Parameters.AddWithValue("@District", int.TryParse(model.PresentAddress.District, out int dId) ? (object)dId : DBNull.Value);
                        cmd.Parameters.AddWithValue("@TehsilName", int.TryParse(model.PresentAddress.TehsilName, out int tId) ? (object)tId : DBNull.Value);
                        cmd.Parameters.AddWithValue("@PoliceStation", int.TryParse(model.PresentAddress.PoliceStation, out int pId) ? (object)pId : DBNull.Value);
                        cmd.Parameters.AddWithValue("@VillageCity", int.TryParse(model.PresentAddress.VillageCity, out int vId) ? (object)vId : DBNull.Value);

                        cmd.Parameters.AddWithValue("@AreaType", model.PresentAddress.AreaType ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Parishad", (object)model.PresentAddress.Parishad ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ParishadName", (object)model.PresentAddress.ParishadName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CityWardNo", (object)model.PresentAddress.CityWardNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CityWardName", (object)model.PresentAddress.CityWardName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Pincode", model.PresentAddress.Pincode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TelephoneNumber", (object)model.PresentAddress.TelephoneNumber ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MobileNumber", model.PresentAddress.MobileNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", model.PresentAddress.Email ?? (object)DBNull.Value);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                string msg = reader["ResponseMessage"].ToString()!;
                                int id = Convert.ToInt32(reader["RecordId"]);

                                if (msg == "SUCCESS")
                                {
                                    return Ok(new { success = true, message = "Saved successfully via integrated SP framework flow!", recordId = id });
                                }
                                return BadRequest(new { success = false, message = msg });
                            }
                        }
                    }
                }
                return BadRequest(new { success = false, message = "Execution callback frame empty." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

    }
}