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
    public class MembersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MembersController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        public JsonResult GetAll()
        {
            string query = @"
                        select id, first_name, last_name, id_card, city, street, number, date_of_birth, phone, mobile_phone from
                         dbo.Members";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CoronaAppCon");
            SqlDataReader myReader;
            using(SqlConnection myCon= new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand=new SqlCommand(query, myCon))
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
            SELECT id, first_name, last_name, id_card, city, street, number, date_of_birth, phone, mobile_phone
            FROM dbo.Members
            WHERE id_card = @id_card";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CoronaAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id_card", id_card);
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Members mem)
        {
            string query = @"
        INSERT INTO dbo.Members
        (first_name, last_name, id_card, city, street, number, date_of_birth, phone, mobile_phone)
        VALUES (@first_name, @last_name, @id_card, @city, @street, @number, @date_of_birth, @phone, @mobile_phone)";
            string countQuery = "SELECT COUNT(*) FROM dbo.Members WHERE id_card = @id_card";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CoronaAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand countCommand = new SqlCommand(countQuery, myCon))
                {
                    countCommand.Parameters.AddWithValue("@id_card", mem.id_card);
                    int count = (int)countCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        return new JsonResult("Error: id_card already exists.");
                    }
                }
                using (SqlCommand insertCommand = new SqlCommand(query, myCon))
                {
                    insertCommand.Parameters.AddWithValue("@first_name", mem.first_name);
                    insertCommand.Parameters.AddWithValue("@last_name", mem.last_name);
                    insertCommand.Parameters.AddWithValue("@id_card", mem.id_card);
                    insertCommand.Parameters.AddWithValue("@city", mem.city);
                    insertCommand.Parameters.AddWithValue("@street", mem.street);
                    insertCommand.Parameters.AddWithValue("@number", mem.number);
                    insertCommand.Parameters.AddWithValue("@date_of_birth", mem.date_of_birth);
                    insertCommand.Parameters.AddWithValue("@phone", mem.phone);
                    insertCommand.Parameters.AddWithValue("@mobile_phone", mem.mobile_phone);

                    insertCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }
            return new JsonResult("Added Successfully");
        }


    }
}
