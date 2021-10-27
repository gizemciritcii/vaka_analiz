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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _env;
        public UserController(IConfiguration configuration, IWebHostEnvironment env)
        {

            _configuration = configuration;
            _env = env;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select HastaId,HastaAd,Hastasoyad,Email,Yaş,Departman,
                                 convert(varchar(20),giris,120) as giris,PhotoFileName
                                   from 
                               dbo.Hasta";
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
        public JsonResult Post(User use)
        {
            string query = @"insert into dbo.Hasta
                           (HastaAd,Hastasoyad,Email,Yaş,Departman,giris,PhotoFileName) values(@HastaAd,@Hastasoyad,@Yaş,@Email,@Departman,@giris,@PhotoFileName)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("VakaAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {

                    myCommand.Parameters.AddWithValue("@HastaAd", use.HastaAd);
                    myCommand.Parameters.AddWithValue("@Hastasoyad", use.HastaSoyad);
                    myCommand.Parameters.AddWithValue("@Email", use.Email);
                    myCommand.Parameters.AddWithValue("@Yaş", use.Yaş);
                    myCommand.Parameters.AddWithValue("@Departman", use.Departman);
                    myCommand.Parameters.AddWithValue("@giris", use.Giris);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", use.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");

        }
        [HttpPut]
        public JsonResult Put(User use)
        {
            string query = @"update  dbo.Hasta
                              set HastaAd= @HastaAd,
                                Hastasoyad=@Hastasoyad,
                                   Email=@Email,
                                    Yaş=@Yaş,
                                      Departman=@Departman,
                                       giris=@giris,
                                           PhotoFileName=@PhotoFileName

                                 where HastaId=@HastaId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("VakaAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@HastaId", use.HastaId);
                    myCommand.Parameters.AddWithValue("@HastaAd", use.HastaAd);
                    myCommand.Parameters.AddWithValue("@Hastasoyad", use.HastaSoyad);
                    myCommand.Parameters.AddWithValue("@Email", use.Email);
                    myCommand.Parameters.AddWithValue("@Yaş", use.Yaş);
                    myCommand.Parameters.AddWithValue("@Departman", use.Departman);
                    myCommand.Parameters.AddWithValue("@giris", use.Giris);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", use.PhotoFileName);
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
            string query = @" delete from dbo.Hasta
                              where HastaId=@HastaId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("VakaAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@HastaId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");

        }
        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("aaa.png");
            }

        }
    }
}