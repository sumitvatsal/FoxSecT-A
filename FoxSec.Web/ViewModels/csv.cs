using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Data.OleDb;
using DevExpress.XtraGrid;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;

namespace FoxSec.Web.ViewModels
{
    public class csv
    {

        private static string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Upload.csv");

        private static string name = fileName.Substring(0, fileName.LastIndexOf('.'));
        public static object OpenExcelFile()
        {
            //DataTable dataTable = new DataTable();
            //string connectionString = string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + fileName + "; " + "Extended Properties = 'text;HDR=YES;'", fileName);
            //OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [FS_EMY.CSV$]", connectionString);
            //adapter.Fill(dataTable);
            //return dataTable;

            //     private static string fileName = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Customers.xls");


            //DataTable dataTable = new DataTable();
            //string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"text;HDR=Yes;FMT=CSVDelimited;", fileName);
            //OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [FS_EMY$]", connectionString);
            //adapter.Fill(dataTable);
            //return dataTable;


            FileInfo file = new FileInfo(fileName);
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" +
                    file.DirectoryName + "\";Extended Properties = \"Text;HDR=YES;FMT=CSVDelimited\"");
            //OleDbCommand cmd = new OleDbCommand(string.Format
            //                              ("SELECT * FROM [{0}]", file.Name), conn);
            OleDbDataAdapter adapter = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", file.Name), conn);
            DataTable dataTable = new DataTable();


            //  dataTable.Columns.Add("Selected", typeof(System.Boolean));
            adapter.Fill(dataTable);



            // dataTable.Columns.Add();
            return dataTable;


        }
    }




}