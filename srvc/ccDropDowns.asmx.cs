using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using AjaxControlToolkit;
using System.Web.Script.Services;


namespace priceSvg
{
    /// <summary>
    /// Summary description for ccDropDowns
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class ccDropDowns : System.Web.Services.WebService
    {

        [WebMethod]
        public CascadingDropDownNameValue[] getMaterials()
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["zapCartDb"].ConnectionString);
            myConn.Open();
                        
            SqlCommand myCmd = new SqlCommand("SELECT * FROM [tbl_materialTypes] WHERE activestatus = 1 ORDER BY materialname");
            myCmd.Connection = myConn;
            //SqlParameter myParam = new SqlParameter();
            //myParam.ParameterName = "@borders";
            //myParam.Value = pathFill;
            //myCmd.Parameters.Add(myParam);

            SqlDataReader myReader = null;
            myReader = myCmd.ExecuteReader();
            List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();                   

            while (myReader.Read())
            {
                l.Add(new CascadingDropDownNameValue(myReader["materialName"].ToString(), myReader["materialTypeid"].ToString()));                                               
            }

            myReader = null;
            myConn.Close();
            //myCmd = null;
            myConn = null;

            return l.ToArray();
        }


        [WebMethod]
        public CascadingDropDownNameValue[] getMm(string knownCategoryValues, string category)
        {
            int materialTypeId;
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

            if (!kv.ContainsKey("material") || !Int32.TryParse(kv["material"], out materialTypeId))
            {
                throw new ArgumentException("Couldn't find material.");
            };

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["zapCartDb"].ConnectionString);
            myConn.Open();

            SqlCommand myCmd = new SqlCommand("select * from dbo.tbl_materialMm where materialMmId in(select distinct materialMmId from dbo.tbl_zapMaterials where materialTypeId =@mId)");
            myCmd.Connection = myConn;
            SqlParameter myParam = new SqlParameter();
            myParam.ParameterName = "@mId";
            myParam.Value = materialTypeId;
            myCmd.Parameters.Add(myParam);

            SqlDataReader myReader = null;
            myReader = myCmd.ExecuteReader();
            List<CascadingDropDownNameValue> l = new List<CascadingDropDownNameValue>();

            while (myReader.Read())
            {
                l.Add(new CascadingDropDownNameValue(myReader["materialMm"].ToString(), myReader["materialMmId"].ToString()));
            }

            myReader = null;
            myConn.Close();
            //myCmd = null;
            myConn = null;

            return l.ToArray();

        }
            
   }
}

