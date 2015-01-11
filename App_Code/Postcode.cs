using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Postcode
/// </summary>
public class Postcode
{
    private int _id = 0;
    private string _postCode = string.Empty;
    private double _lat = 0;
    private double _lng = 0;

    public int ID
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }

    public string PostCode
    {
        get
        {
            return _postCode;
        }
        set
        {
            _postCode = value;
        }
    }

    public double Lat
    {
        get
        {
            return _lat;
        }
        set
        {
            _lat = value;
        }
    }

    public double Lng
    {
        get
        {
            return _lng;
        }
        set
        {
            _lng = value;
        }
    }
	public Postcode()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public Postcode(int id, string postcode, double lat, double lng)
    {
        ID = id;
        PostCode = postcode;
        Lat = lat;
        Lng = lng;
   
    }

    public  Postcode(double lat, double lng)
    {
        ID = 0;
        PostCode = string.Empty;
        Lat = lat;
        Lng = lng;
    }

    public  Postcode GetLatLng(string postcode)
    {
        PostCodeDal pDal = new PostCodeDal();
        return pDal.GetLatLng(postcode);
    }
}
