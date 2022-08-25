using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace pushPulldata.Controllers
{
    public class PushPulldataController : ApiController
    {

        string connStr = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        [HttpPost]
        [Route("api/User/AddUser")]
        public string AddUser([FromBody] JObject data)
        {
            SqlConnection con = new SqlConnection(connStr);
            try
            {
                JObject o = (JObject)data;

                string UserName = (string)o.GetValue("UserName");
                string Password = (string)o.GetValue("Password");
                string Role = (string)o.GetValue("Role");
                SqlCommand addusercmd = new SqlCommand("Insert into Users  VALUES('" + UserName + "','" + Password + "','" + Role + "')", con);
                con.Open();
                int i = addusercmd.ExecuteNonQuery();
                con.Close();
                if (i == 1)
                {
                    return "User added successfully";
                }
                else
                {
                    return "Error adding user";
                }
            }
            catch (Exception e)
            {
                return $"Error:{e.Message}";
                //return "User Name already exist";
            }
        }


        [HttpGet]
        [Route("api/User/GetUserData")]
        public HttpResponseMessage GetUserData([FromUri] string UserName)
        { 
            SqlConnection con = new SqlConnection(connStr);
            SqlDataAdapter da = new SqlDataAdapter("select * FROM dbo.Users WHERE UserName = '" + UserName + "';", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No data available for specified user");
            }

        }

        [HttpGet]
        [Route("api/User/GetAllUserData")]
        public HttpResponseMessage GetAllUserData()
        {
            SqlConnection con = new SqlConnection(connStr);
            SqlDataAdapter da = new SqlDataAdapter("select * FROM dbo.Users", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No data available for specified user");
            }
        }




    }
}
