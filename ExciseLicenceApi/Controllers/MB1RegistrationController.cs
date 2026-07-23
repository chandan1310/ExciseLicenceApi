using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ExciseLicenceApi.Data;
using ExciseLicenceApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ExciseLicenceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MB1RegistrationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MB1RegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/MB1Registration/save
        [HttpPost("save")]
        public async Task<IActionResult> SaveMB1Registration([FromBody] MB1RegistrationModel model)
        {
            if (model == null)
            {
                return BadRequest(new { success = false, message = "Form payload cannot be null." });
            }

            try
            {
                // Setting up SQL Parameters safely to feed into EF Core execution
                var pFinancialYear = new SqlParameter("@FinancialYear", model.FinancialYear ?? (object)DBNull.Value);
                var pBName = new SqlParameter("@BName", model.BName ?? (object)DBNull.Value);
                var pMobileNumber = new SqlParameter("@MobileNumber", model.MobileNumber ?? (object)DBNull.Value);
                var pEmail = new SqlParameter("@Email", model.Email ?? (object)DBNull.Value);
                var pUdyankNo = new SqlParameter("@UdyankNo", model.UdyankNo ?? (object)DBNull.Value);
                var pInd_DistrictName = new SqlParameter("@Ind_DistrictName", model.Ind_DistrictName ?? (object)DBNull.Value);
                var pPanno = new SqlParameter("@Panno", model.Panno ?? (object)DBNull.Value);

                // Executing SP and mapping tracking variables from row outputs via a dynamic dictionary or direct collection
                string sqlQuery = "EXEC [dbo].[sp_Insert_BarFirstRegistration_MB1] @FinancialYear, @BName, @MobileNumber, @Email, @UdyankNo, @Ind_DistrictName, @Panno";

                // Using an anonymous entity type approach or ADO bridge connection explicitly provided by DbContext
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddRange(new[] { pFinancialYear, pBName, pMobileNumber, pEmail, pUdyankNo, pInd_DistrictName, pPanno });

                    if (command.Connection.State != ConnectionState.Open)
                    {
                        await command.Connection.OpenAsync();
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int sno = Convert.ToInt32(reader["Sno"]);
                            string msg = reader["ResponseMessage"].ToString();
                            string webformNo = reader["webform_no"].ToString();

                            if (sno > 0 && msg == "SUCCESS")
                            {
                                return Ok(new { success = true, message = "M.B-1 Registration Completed Successfully!", webform_no = webformNo });
                            }
                            return Ok(new { success = false, message = msg });
                        }
                    }
                }

                return BadRequest(new { success = false, message = "Database failed to process registration row." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // GET: api/MB1Registration/list?financialYear=2026-2027
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<MB1RegistrationModel>>> GetMB1List([FromQuery] string financialYear)
        {
            if (string.IsNullOrEmpty(financialYear))
            {
                return BadRequest(new { success = false, message = "Financial Year parameter is required." });
            }

            try
            {
                var dataList = new List<MB1RegistrationModel>();
                var pFinancialYear = new SqlParameter("@FinancialYear", financialYear);

                string sqlQuery = "EXEC [dbo].[sp_Get_BarFirstRegistrations_MB1] @FinancialYear";

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    command.Parameters.Add(pFinancialYear);

                    if (command.Connection.State != ConnectionState.Open)
                    {
                        await command.Connection.OpenAsync();
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dataList.Add(new MB1RegistrationModel
                            {
                                Sno = Convert.ToInt32(reader["Sno"]),
                                FinancialYear = reader["FinancialYear"].ToString(),
                                webform_no = reader["webform_no"].ToString(),
                                BName = reader["BName"].ToString(),
                                MobileNumber = reader["MobileNumber"].ToString(),
                                Email = reader["Email"].ToString(),
                                UdyankNo = reader["UdyankNo"].ToString(),
                                Ind_DistrictName = reader["Ind_DistrictName"].ToString(),
                                Panno = reader["Panno"].ToString()
                            });
                        }
                    }
                }

                return Ok(new { success = true, data = dataList });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    
    
    }
}