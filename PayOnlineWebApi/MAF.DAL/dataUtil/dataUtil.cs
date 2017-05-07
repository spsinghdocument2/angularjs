using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;

namespace MAF.DAL.dataUtil
{
  public  class dataUtil
    {
         public string CommandText;
    public int RowsAffected = 0;

    private string _conStr = String.Empty;
    private SqlConnection _sqlCon;
    private SqlCommand _sqlCom;

    public dataUtil(string conStr)
	{
        _conStr = conStr;
        InitializeComponent();
        if (_conStr != String.Empty)
            _sqlCon.Open();
    }

    public void Open()
    {
        if (_sqlCon.State == System.Data.ConnectionState.Closed)
            _sqlCon.Open();
    }

	public void Close()
	{
		_sqlCon.Close();
	}

	public SqlConnection getConnection()
	{
		return _sqlCon;
	}

	public SqlCommand getCommand()
	{
		return _sqlCom;
	}

	public void Execute()
	{
		try
		{
			int Tentativa = 0;
			if(_sqlCon.State == ConnectionState.Closed)
				_sqlCon.Open();
            
			while((_sqlCon.State == ConnectionState.Executing || _sqlCon.State == ConnectionState.Fetching) && Tentativa < 5)
			{
				_sqlCom.Cancel();
				Thread.Sleep(500);
				Tentativa++;
			}
			_sqlCom.CommandText = CommandText;
			RowsAffected = _sqlCom.ExecuteNonQuery();
		}
		catch(Exception exc)
		{
			_sqlCom.Cancel(); 
			throw exc;
		}
	}

    public SqlDataReader ExecuteReader()
    {
        int Tentativa = 0;
        if (_sqlCon.State == ConnectionState.Closed)
            _sqlCon.Open();

        while ((_sqlCon.State == ConnectionState.Executing || _sqlCon.State == ConnectionState.Fetching) && Tentativa < 5)
        {
            _sqlCom.Cancel();
            Thread.Sleep(500);
            Tentativa++;
        }
        _sqlCom.CommandText = CommandText;
        return _sqlCom.ExecuteReader();
    }

    private void InitializeComponent()
    {
        this._sqlCon = new SqlConnection();
        this._sqlCom = new SqlCommand();

        this._sqlCon.ConnectionString = _conStr;
        this._sqlCom.Connection = this._sqlCon;
    }
    }
}
