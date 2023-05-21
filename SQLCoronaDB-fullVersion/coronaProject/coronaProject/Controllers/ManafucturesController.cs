

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
    public class ManafucturesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ManafucturesController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        select id, manafucture_name from
                         dbo.Manafuctures";
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
        [HttpPost]
        public JsonResult Post(Manafuctures man)
        {
            string query = @"
            INSERT INTO dbo.Manafuctures
            (manafucture_name)
            VALUES (@manafucture_name)";
            //string countQuery = "SELECT COUNT(*) FROM dbo.Manafuctures WHERE manafucture_name = @manafuct_name";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CoronaAppCon");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand insertCommand = new SqlCommand(query, myCon))
                {
                    /*countCommand.Parameters.AddWithValue("@manafucture_name", man.manafucture_name);
                    int count = (int)countCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        return new JsonResult("Error: id_card already exists.");
                    }*/
                    insertCommand.Parameters.AddWithValue("@manafucture_name", man.manafucture_name);

                    insertCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }
            return new JsonResult("Added Successfully");
        }


    }
}


