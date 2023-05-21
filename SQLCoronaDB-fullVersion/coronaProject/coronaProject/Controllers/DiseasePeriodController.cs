
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using coronaProject.Models;

namespace coronaProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiseasePeriodController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DiseasePeriodController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        public JsonResult GetAll()
        {
            string query = @"
                        select id, member_id, detected_date, recovery_date from
                         dbo.DiseasePeriod";
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
        SELECT v.id, v.member_id, v.detected_date, v.recovery_date 
        FROM dbo.DiseasePeriod v
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
        public JsonResult Post(DiseasePeriod dis)
        {
            if (DateTime.Parse(dis.detected_date) > DateTime.Parse(dis.recovery_date))
            {
                return new JsonResult("Error: detected date is later than recovery date.");
            }
            string query = @"
        INSERT INTO dbo.DiseasePeriod
        (member_id, detected_date, recovery_date)
        VALUES (@member_id, @detected_date, @recovery_date)";
            string countQuery = "SELECT COUNT(*) FROM dbo.DiseasePeriod WHERE member_id = @member_id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CoronaAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand countCommand = new SqlCommand(countQuery, myCon))
                {
                    countCommand.Parameters.AddWithValue("@member_id", dis.member_id);
                    int count = (int)countCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        return new JsonResult("Error: id_card already exists.");
                    }
                }
                using (SqlCommand insertCommand = new SqlCommand(query, myCon))
                {
                    insertCommand.Parameters.AddWithValue("@member_id", dis.member_id);
                    insertCommand.Parameters.AddWithValue("@detected_date", dis.detected_date);
                    insertCommand.Parameters.AddWithValue("@recovery_date", dis.recovery_date);

                    insertCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }
            return new JsonResult("Added Successfully");
        }
        

        [HttpPost("byidcard")]
        public JsonResult Post(string id_card, string detected_date, string recovery_date)
        {

            string query = @"
        INSERT INTO dbo.DiseasePeriod
        (member_id, detected_date, recovery_date)
        VALUES (@member_id, @detected_date, @recovery_date)";
            string countQuery = "SELECT COUNT(*) FROM dbo.DiseasePeriod WHERE member_id = @member_id";
            string memberIdQuery = "SELECT id FROM dbo.Members WHERE id_card = @id_card";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CoronaAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                int memberId;
                // Get the member ID from the Members table using the id_card
               
                using (SqlCommand memberIdCommand = new SqlCommand(memberIdQuery, myCon))
                {
                    memberIdCommand.Parameters.AddWithValue("@id_card", id_card);
                    object memberIdResult = memberIdCommand.ExecuteScalar();
                    if (memberIdResult == null)
                    {
                        return new JsonResult("Error: member not found.");
                    }
                    memberId = Convert.ToInt32(memberIdResult);
                    using (SqlCommand countCommand = new SqlCommand(countQuery, myCon))
                    {
                        countCommand.Parameters.AddWithValue("@member_id", memberId);
                        int count = (int)countCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            return new JsonResult("Error: member_id already exists.");
                        }
                    }
                    // Insert the new record into the DiseasePeriod table
                    using (SqlCommand insertCommand = new SqlCommand(query, myCon))
                    {
                        insertCommand.Parameters.AddWithValue("@member_id", memberId);
                        insertCommand.Parameters.AddWithValue("@detected_date", detected_date);
                        insertCommand.Parameters.AddWithValue("@recovery_date", recovery_date);

                        insertCommand.ExecuteNonQuery();
                    }
                }
                myCon.Close();
            }
            return new JsonResult("Added Successfully");
        }




    }



}



