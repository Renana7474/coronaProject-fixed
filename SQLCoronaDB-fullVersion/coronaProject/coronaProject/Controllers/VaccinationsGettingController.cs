using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using coronaProject.Models;

namespace coronaProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinationsGettingController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public VaccinationsGettingController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        public JsonResult GetALL()
        {
            string query = @"
                        
                        select id , member_id, vaccin_date, manafuct_code from dbo.VaccinationsGetting";
            
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CoronaAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
        [HttpGet("{id_card}")]
        public JsonResult GetByID(string id_card)
        {
            string query = @"
        SELECT v.id, v.member_id, v.vaccin_date, v.manafuct_code 
        FROM dbo.VaccinationsGetting v
        INNER JOIN dbo.Members m ON v.member_id = m.id
        WHERE m.id_card = @id_card";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CoronaAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_card", id_card);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

       
        [HttpPost]
        public JsonResult Post(VaccinationsGetting vac)
        {
            string query = @"
                        insert into dbo.VaccinationsGetting
                        (member_id, vaccin_date, manafuct_code)
                        values (@member_id, @vaccin_date, @manafuct_code)";
            string countQuery = "SELECT COUNT(*) FROM dbo.VaccinationsGetting WHERE member_id = @member_id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CoronaAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand countCommand = new SqlCommand(countQuery, myCon))
                {
                    countCommand.Parameters.AddWithValue("@member_id", vac.member_id);
                    int count = (int)countCommand.ExecuteScalar();
                    if (count > 4)
                    {
                        return new JsonResult("Error: id_card already exists.");
                    }
                }
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@member_id", vac.member_id);
                    myCommand.Parameters.AddWithValue("@vaccin_date", vac.vaccin_date);
                    myCommand.Parameters.AddWithValue("@manafuct_code", vac.manafuct_code);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }
        [HttpPost("byidcard")]
        public JsonResult Post(string id_card, DateTime vaccin_date, string manafucture_name)
        {
            string memberIdQuery = "SELECT id FROM dbo.Members WHERE id_card = @id_card";
            string manafuctureIdQuery = "SELECT id FROM dbo.Manafuctures WHERE manafucture_name = @manafucture_name";
            string insertManafuctureQuery = "INSERT INTO dbo.Manafuctures (manafucture_name) VALUES (@manafucture_name); SELECT SCOPE_IDENTITY();";
            string insertQuery = "INSERT INTO dbo.VaccinationsGetting (member_id, vaccin_date, manafuct_code) VALUES (@member_id, @vaccin_date, @manafuct_code)";

            string sqlDataSource = _configuration.GetConnectionString("CoronaAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                int memberId;
                using (SqlCommand memberIdCommand = new SqlCommand(memberIdQuery, myCon))
                {
                    memberIdCommand.Parameters.AddWithValue("@id_card", id_card);
                    object memberIdResult = memberIdCommand.ExecuteScalar();
                    if (memberIdResult == null)
                    {
                        return new JsonResult("Error: member not found.");
                    }
                    memberId = Convert.ToInt32(memberIdResult);

                    using (SqlCommand manafuctureIdCommand = new SqlCommand(manafuctureIdQuery, myCon))
                    {
                        manafuctureIdCommand.Parameters.AddWithValue("@manafucture_name", manafucture_name);
                        object manafuctureId = manafuctureIdCommand.ExecuteScalar();

                        // Check if manufacturer exists
                        if (manafuctureId == null)
                        {
                            // Insert the new manufacturer and get its generated ID
                            using (SqlCommand insertManafuctureCommand = new SqlCommand(insertManafuctureQuery, myCon))
                            {
                                insertManafuctureCommand.Parameters.AddWithValue("@manafucture_name", manafucture_name);
                                manafuctureId = insertManafuctureCommand.ExecuteScalar();
                            }
                        }

                        // Verify that manafuctureId is not null
                        if (manafuctureId != null)
                        {
                            // Convert manafuctureId to the appropriate type
                            int manafuctureIdValue = Convert.ToInt32(manafuctureId);

                            // Insert the vaccination record
                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, myCon))
                            {
                                insertCommand.Parameters.AddWithValue("@member_id", memberId);
                                insertCommand.Parameters.AddWithValue("@vaccin_date", vaccin_date);
                                insertCommand.Parameters.AddWithValue("@manafuct_code", manafuctureIdValue);

                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                myCon.Close();
            }

            return new JsonResult("Added Successfully");
        }


    }
}
/*select id, vaccin_date_1, vaccin_manafuct_1, vaccin_date_2, vaccin_manafuct_2, vaccin_date_3, vaccin_manafuct_3, vaccin_date_4, vaccin_manafuct_4 from*/