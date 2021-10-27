using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApplication1.models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmanController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public DepartmanController(IConfiguration configuration)
        {

            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select DepartmanId,DepartmanName from 
                               dbo.Departman";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("VakaAppCon");
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
        public JsonResult Post(Departman dep)
        {
            string query = @"insert into dbo.Departman
                               values(@DepartmanName)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("VakaAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmanName", dep.DepartmanName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");

        }
        [HttpPut]
        public JsonResult Put(Departman dep)
        {
            string query = @"update  dbo.Departman
                              set DepartmanName= @DepartmanName
                                 where DepartmanId=@DepartmanId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("VakaAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmanId", dep.DepartmanId);
                    myCommand.Parameters.AddWithValue("@DepartmanName", dep.DepartmanName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("update Successfully");

        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @" delete from dbo.Departman
                              where DepartmanId=@DepartmanId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("VakaAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmanId",id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");

        }
    }
}
