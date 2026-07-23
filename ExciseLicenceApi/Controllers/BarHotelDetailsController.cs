using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ExciseLicenceApi.DTOs;

namespace ExciseLicenceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BarHotelDetailsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public BarHotelDetailsController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet("dropdown-shops/{year}")]
        public async Task<IActionResult> GetLookupShops([FromRoute] string year)
        {
            string connectionString = _configuration.GetConnectionString("sqlServrConnStrng")!;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("licenceapplicantMaster_Shop_MB1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GET_SHOPS");
                        cmd.Parameters.AddWithValue("@FinancialYear", year);

                        await conn.OpenAsync();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            var shopDropdownList = dt.AsEnumerable().Select(row => new {
                                shopId = row["ShopId"],
                                shopName = row["ShopName"]
                            }).ToList();
                            return Ok(new { success = true, data = shopDropdownList });
                        }
                    }
                }
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, error = ex.Message }); }
        }

        [HttpPost("save-bar-entry")]
        public async Task<IActionResult> SaveBarEntry([FromBody] BarShopDetailsDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(new { success = false, message = "Payload properties configuration state invalid." });
            string connectionString = _configuration.GetConnectionString("sqlServrConnStrng")!;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("licenceapplicantMaster_Shop_MB1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "POST");
                        cmd.Parameters.AddWithValue("@ApplicantId", model.ApplicantId);
                        cmd.Parameters.AddWithValue("@LicTypeId", model.LicTypeId);
                        cmd.Parameters.AddWithValue("@FinancialYear", model.FinancialYear);
                        cmd.Parameters.AddWithValue("@ShopDistrictId", model.ShopDistrictId);
                        cmd.Parameters.AddWithValue("@ShopName", model.ShopName);
                        cmd.Parameters.AddWithValue("@ShopStreetAddress", model.ShopStreetAddress);
                        cmd.Parameters.AddWithValue("@ShopPhon", (object)model.ShopPhon ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ShopmailId", model.ShopMailId);
                        cmd.Parameters.AddWithValue("@TeshsilName", model.TehsilName);
                        cmd.Parameters.AddWithValue("@BlockName", model.BlockName);
                        cmd.Parameters.AddWithValue("@MandalName", model.MandalName);
                        cmd.Parameters.AddWithValue("@CircleName", model.CircleName);
                        cmd.Parameters.AddWithValue("@ThanaName", model.ThanaName);
                        cmd.Parameters.AddWithValue("@SubdivisionName", model.SubdivisionName);
                        cmd.Parameters.AddWithValue("@Rural_UrbnArea", model.RuralUrbanArea);
                        cmd.Parameters.AddWithValue("@Tribal_NonTrible", model.TribalNonTribal);
                        cmd.Parameters.AddWithValue("@VidhanSabhaName", model.VidhanSabhaName);
                        cmd.Parameters.AddWithValue("@LoksabhName", model.LoksabhaName);
                        cmd.Parameters.AddWithValue("@ShopWardNo", (object)model.ShopWardNo ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ShopWardName", (object)model.ShopWardName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ShopLandMark", (object)model.ShopLandMark ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ShopVillageCityName", model.ShopVillageCityName);
                        cmd.Parameters.AddWithValue("@ZialaParishad", (object)model.ZilaParishad ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ParishadName", (object)model.ParishadName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@north_place", model.NorthPlace);
                        cmd.Parameters.AddWithValue("@south_place", model.SouthPlace);
                        cmd.Parameters.AddWithValue("@east_place", model.EastPlace);
                        cmd.Parameters.AddWithValue("@west_place", model.WestPlace);
                        cmd.Parameters.AddWithValue("@GSTIN", model.GSTIN);
                        cmd.Parameters.AddWithValue("@Bar_NickName", (object)model.BarNickName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Hotel_Food_Lic_No", model.HotelFoodLicNo);
                        cmd.Parameters.AddWithValue("@Hotel_Pan", model.HotelPan);
                        cmd.Parameters.AddWithValue("@No_of_Star", model.NoOfStar);
                        cmd.Parameters.AddWithValue("@No_of_Room", model.NoOfRoom);
                        cmd.Parameters.AddWithValue("@PopulationRangeId", model.PopulationRangeId);
                        cmd.Parameters.AddWithValue("@ShoPPin", model.ShopPin);
                        cmd.Parameters.AddWithValue("@CrntPopulation", model.CrntPopulation);

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return Ok(new { success = true, message = "Hotel/Bar application record saved to licenceapplicantMaster_Shop successfully!" });
                            }
                        }
                    }
                }
                return BadRequest(new { success = false });
            }
            catch (Exception ex) { return StatusCode(500, new { success = false, error = ex.Message }); }
        }
        [HttpGet("get-upload-requirements")]
        public async Task<IActionResult> GetUploadRequirements()
        {
            string connectionString = _configuration.GetConnectionString("sqlServrConnStrng")!;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("licenceapplicantMaster_Upload_MB1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GET_MASTER_REQUIREMENTS");
                        cmd.Parameters.AddWithValue("@ApplicantId", 0);

                        await conn.OpenAsync();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            var requirements = dt.AsEnumerable().Select(row => new
                            {
                                fileId = row["FileId"],
                                name = row["FieUploadName"],
                                type = row["Filetype"],
                                limit = row["ImageSize"]
                            }).ToList();

                            return Ok(new { success = true, data = requirements });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        [HttpPost("upload-single-document")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadSingleDocument([FromForm] DocumentUploadModel payload)
        {
            if (payload.File == null || payload.File.Length == 0)
                return BadRequest(new { success = false, message = "No file stream located." });

            var fileExtension = Path.GetExtension(payload.File.FileName).ToLower();
            if (fileExtension != ".pdf")
                return BadRequest(new { success = false, message = "Only PDF extension streaming allowed." });

            if (payload.File.Length > 1024 * 1024)
                return BadRequest(new { success = false, message = "File size constraint exceeded. Maximum 1 MB allowed." });

            string connectionString = _configuration.GetConnectionString("sqlServrConnStrng")!;

            try
            {
                string projectRootPath = _env.ContentRootPath;
                string targetSubFolder = Path.Combine(projectRootPath, "Uploads", "Documents");

                if (!Directory.Exists(targetSubFolder))
                {
                    Directory.CreateDirectory(targetSubFolder);
                }

                string generatedUniqueFileName = $"{payload.ApplicantId}_{payload.FileMasterId}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
                string absolutePhysicalStoragePath = Path.Combine(targetSubFolder, generatedUniqueFileName);

                using (var stream = new FileStream(absolutePhysicalStoragePath, FileMode.Create))
                {
                    await payload.File.CopyToAsync(stream);
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("licenceapplicantMaster_Upload_MB1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "POST");
                        cmd.Parameters.AddWithValue("@ApplicantId", payload.ApplicantId);
                        cmd.Parameters.AddWithValue("@FileMasterId", payload.FileMasterId);
                        cmd.Parameters.AddWithValue("@FileNameString", payload.File.FileName);
                        cmd.Parameters.AddWithValue("@SavedPhysicalPath", Path.Combine("Uploads", "Documents", generatedUniqueFileName));

                        await conn.OpenAsync();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return Ok(new
                                {
                                    success = true,
                                    message = "File saved successfully to local storage directory.",
                                    savedAs = generatedUniqueFileName
                                });
                            }
                        }
                    }
                }
                return BadRequest(new { success = false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }



}
