using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Data.SqlClient;
/// <summary>
/// Summary description for PostCodeDal
/// </summary>
public class PostCodeDal
{
    private  SqlConnection con = null;
    private SqlCommand cmd = null;

    private string connectionString =
        WebConfigurationManager.ConnectionStrings["PostCodeConnectionString"].ConnectionString;


	public PostCodeDal()
	{
		con = new SqlConnection(connectionString);
	    cmd = new SqlCommand();
	    cmd.Connection = con;
	}

    public  Postcode GetLatLng(string PostCode)
    {
        Postcode p = null;
        try
        {
            con.Open();
            cmd.CommandText = "Select Lat, Lng from PostCode Where PostCode = @Postcode";
            cmd.Parameters.AddWithValue("@Postcode", PostCode);

            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                p = new Postcode((double)reader["Lat"], (double)reader["Lng"]);

            }

            return p;
        }

        catch(SqlException ex)
        {
            
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }

        finally
        {
            con.Close();
            
        }
        return p;
    }

}
