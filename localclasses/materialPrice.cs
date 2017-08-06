using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace priceSvg
{
    public class materialPrice
    {
        public float timeCutMm;
        public float timeVectorEngraveMm;
        public float timeRasterEngraveMm;
        public float costCutSec;
        public float costVectorSec;
        public float costRasterPerSec;        
        public float pricePerUnit;


        public void getMaterialPrices(int materialTypeId, int templateSizeId, int mmId)
        {

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["zapCartDb"].ConnectionString);
            myConn.Open();

            SqlCommand myCmd = new SqlCommand("SELECT * FROM tbl_zapMaterials where materialTypeId = @materialid AND materialMmId = @materialMmId AND templateSizeId = @templateSizeId AND activeStatus =1");
            myCmd.Connection = myConn;

            SqlParameter myParam = new SqlParameter();
            myParam.ParameterName = "@materialid";
            myParam.Value = materialTypeId;
            myCmd.Parameters.Add(myParam);

            SqlParameter myParam1 = new SqlParameter();
            myParam1.ParameterName = "@materialMmId";
            myParam1.Value = mmId;
            myCmd.Parameters.Add(myParam1);

            SqlParameter myParam2 = new SqlParameter();
            myParam2.ParameterName = "@templateSizeId";
            myParam2.Value = templateSizeId;
            myCmd.Parameters.Add(myParam2);

            SqlDataReader myReader = null;

            myReader = myCmd.ExecuteReader();

            while (myReader.Read())
            {
                timeCutMm = float.Parse((string)myReader["timePerMM"]);

                timeVectorEngraveMm = float.Parse((string)myReader["timeVectorPerMM"]);
                timeRasterEngraveMm = float.Parse((string)myReader["timeRasterPerMm"]);
                costCutSec = float.Parse((string)myReader["costCutPerSec"]);
                costVectorSec = float.Parse((string)myReader["costVectorPerSec"]);
                costRasterPerSec = float.Parse((string)myReader["costRasterPerSec"]);
                pricePerUnit = float.Parse((string)myReader["pricePerUnit"]);

                //timeVectorEngraveMm = myReader.GetFloat(6);
                //timeRasterEngraveMm = myReader.GetFloat(8);
                //costCutSec = myReader.GetFloat(4);
                //costVectorSec = myReader.GetFloat(7);
                //costRasterPerSqCm = myReader.GetFloat(9);
                //costPricePerInch = myReader.GetFloat(11);
            }

           
        }

    }
}