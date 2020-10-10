using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoxSec.Web.ViewModels;
using System.Net;
using System.Xml.Linq;
using System.Web.Mvc;
using System.IO;
using System.Data.OleDb;
using DevExpress.XtraGrid;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using System.Data;
using FoxSec.Web.ListModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;

namespace FoxSec.Web.Controllers
{
    public class CsvData
    {
        public ControllerContext ControllerContext1 { get; internal set; }

        FoxSecDBContext db = new FoxSecDBContext();
        FSINISettings objFSINISettings = new FSINISettings();

        string ois_id_isik; string Personal_code, f_name, l_name, username, datefrom, dateto, email, Address;
        List<datatable> list_usr = new List<datatable>();

        public datatableListViewModel GetUsrs(string url)
        {

            var users = new datatableListViewModel();

            string fileName = "";
            var ResultFSINISettings = db.FSINISettings.Where(x => x.SoftType == 6 && !x.IsDeleted).First();
            if (ResultFSINISettings != null)
            {
                fileName = ResultFSINISettings.Value;
            }
            try
            {
                string csvPath = File.ReadAllText(fileName);

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[] { new DataColumn("ois_id_isik", typeof(string)),
                new DataColumn("Personal_code", typeof(string)),
                new DataColumn("f_name", typeof(string)),
                new DataColumn("l_name", typeof(string)),
                new DataColumn("username", typeof(string)),
                new DataColumn("datefrom", typeof(string)),
                new DataColumn("dateto", typeof(string)),
                new DataColumn("email", typeof(string)),
                new DataColumn("Address", typeof(string)) });

                foreach (string row in csvPath.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        dt.Rows.Add();
                        int i = 0;
                        //int k = 0;
                        foreach (string cell in row.Split(','))
                        {
                            if (i != 9)
                            {
                                if (i == 9)
                                {
                                }
                                else
                                    dt.Rows[dt.Rows.Count - 1][i] = cell;
                                if (dt.Rows.Count > 1)
                                {
                                    if (i == 0)
                                    {
                                        ois_id_isik = (cell);
                                    }
                                    else if (i == 1)
                                    {
                                        Personal_code = (cell);
                                    }
                                    else if (i == 2)
                                    {
                                        f_name = cell;
                                    }
                                    else if (i == 3)
                                    {
                                        l_name = cell;
                                    }
                                    else if (i == 4)
                                    {
                                        username = cell;
                                    }
                                    else if (i == 5)
                                    {
                                        datefrom = (cell);
                                    }
                                    else if (i == 6)
                                    {
                                        dateto = (cell);
                                    }
                                    else if (i == 7)
                                    {
                                        email = (cell);
                                    }
                                    else if (i == 8)
                                    {
                                        Address = cell;
                                    }
                                    if (i == 8)
                                    {
                                        saveintoDb();
                                    }
                                }
                                i++;
                            }
                        }
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            //List<datatable> usr = new List<datatable>();
            //var result = db.HrClones.ToList();
            //foreach (var items in result)
            //{
            //    usr.Add(new datatable() { Id = items.Id, ois_id_isik = items.ois_id_isik, isikukood = items.Personal_code, e_nimi = items.f_name, p_nimi = items.l_name, kasutajatunnus = items.username, toosuhte_algus = items.dateform, toosuhte_lopp = items.dateto, ylikooli_e_post = items.email, tookoha_aadress = items.Address });
            //}
            users.datatables = list_usr;
            return users;
        }

        private void saveintoDb()
        {
            //SqlCommand cmd = new SqlCommand("insert into Hr_Clone (ois_id_isik,Personal_code,f_name,l_name,username,dateform,dateto,email,Address) values (@ois_id_isik,@Personal_code,@f_name,@l_name,@username,@dateform,@dateto,@email,@Address)", con);
            try
            {
                //var e = new Hr_Clone();
                //string oidid = ois_id_isik.Replace("\"", "");
                //e.ois_id_isik = oidid;
                //string personalcode= Personal_code.Replace("\"", "");
                //e.Personal_code = personalcode;
                //string firstname= f_name.Replace("\"", "");
                //string lastname=l_name.Replace("\"", "");
                //string Uname= username.Replace("\"", "");
                //string startdate= datefrom.Replace("\"", "");
                //string enddate= dateto.Replace("\"", "");
                //string Email= email.Replace("\"", "");
                //string address= Address.Replace("\"", "");
                //e.f_name = firstname;
                //e.l_name = lastname;
                //e.username = Uname;
                //e.dateform = startdate;
                //e.dateto = enddate;
                //e.email = Email;
                //e.Address = address.Trim();
                //db.HrClones.Add(e);
                //int k = db.SaveChanges();

                string oidid = ois_id_isik.Replace("\"", "");
                string personalcode = Personal_code.Replace("\"", "");
                string firstname = f_name.Replace("\"", "");
                string lastname = l_name.Replace("\"", "");
                string Uname = username.Replace("\"", "");
                string startdate = datefrom.Replace("\"", "");
                string enddate = dateto.Replace("\"", "");
                string Email = email.Replace("\"", "");
                string address = Address.Replace("\"", "");

                list_usr.Add(new datatable() { ois_id_isik = oidid, isikukood = personalcode, e_nimi = firstname, p_nimi = lastname, kasutajatunnus = Uname, toosuhte_algus = startdate, toosuhte_lopp = enddate, ylikooli_e_post = Email, tookoha_aadress = address.Trim() });
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //throw ex;
            }
        }
    }
}