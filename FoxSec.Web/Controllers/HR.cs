using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoxSec.Web.ViewModels;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Data;

using FoxSec.Infrastructure.EF.Repositories;

namespace FoxSec.Web.Controllers
{
    public class HR
    {

        //string URLString = "https://il-esb1.il.playtech.corp/PTServices/WS/HrDataService.asmx/GetEmployeesFullData";
        // string ServiceString = "http://playtech.com/ISservices/";

        FoxSecDBContext db = new FoxSecDBContext();
        public ControllerContext ControllerContext { get; internal set; }

        public HRListViewModel GetUsers(string url)
        {
            if (!url.Contains("playtech.corp"))
            {
                return null;
            }
            var users = new HRListViewModel();

            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };

            XmlUrlResolver res = new XmlUrlResolver();            res.Credentials = new NetworkCredential("foxsec", "SepaPada890", "EE"); //full user name: domain\\username

            XmlReaderSettings set = new XmlReaderSettings();            set.XmlResolver = res;            var doc = XDocument.Load(XmlReader.Create(url, set));

            //oldcode var doc = XDocument.Load(url);
            XNamespace space = XNamespace.Get("http://playtech.com/ISservices/");

            var usr = (from u in doc.Descendants(space + "FoxsecData")
                       select new HRItem
                       {
                           Id = Int32.Parse(u.Element(space + "EmployeeID").Value),
                           Name = u.Element(space + "FirstName").Value,
                           LastName = u.Element(space + "LastName").Value,
                           Department = u.Element(space + "CostCenterName").Value,
                           LastDateOfWork = u.Element(space + "TerminationDate").Value,
                           CompanyName = u.Element(space + "CompanyName").Value
                       }).ToList();

            //var client1 = new WebClient
            //{
            //    Encoding = Encoding.UTF8
            //};
            // var doc1 = XDocument.Load("https://bi.ptts.com/globalhrapi/report?country=ALL");
            //usr.AddRange(from u in doc1.Descendants("EmployeeDetails")
            //             select new HRItem
            //             {
            //                 Id = Int32.Parse(u.Element("EmployeeID").Value),
            //                 Name = u.Element("FirstName").Value,
            //                 LastName = u.Element("LastName").Value,
            //                 Department = u.Element("Department").Value,
            //                 LastDateOfWork = u.Element("LastDayOfWork").Value,
            //                 CompanyName = u.Element("Company").Value
            //             });

            users.HRItems = usr;
            return users;
        }

        public ControllerContext ControllerContext3 { get; internal set; }
        public DataTable GetUsers_xml(string url)
        {
            string filename = "";
            long pkID = 0;
            //var users = new HRmodel();
            Uri uri = new Uri(url);
            if (uri.IsFile)
            {
                filename = System.IO.Path.GetFileName(uri.LocalPath);
            }
            FSHR f = new FSHR();
            //List<HRItem> hr_list = new List<HRItem>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(url);
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/kasutajad/kasutaja");
            var ChildNodeList = nodeList[0].ChildNodes;

            f.fsHrList = new List<FSHR>();

            //f.fsHrList = db.Database.SqlQuery<FSHR>("Select * from FSHR where IndexFilename='"+ filename+"' order by Id").ToList();
            //f.fsHrList = db.FSHR.Where(x => x.IndexFilename.ToLower().Contains(filename.ToLower())).ToList();

            DataTable dTable = null;
            dTable = new DataTable();
            dTable.Clear();
            dTable.Columns.Add("ID", typeof(long));
            dTable.Columns["ID"].ColumnMapping = MappingType.Hidden;
            //foreach (var hr in f.fsHrList)
            //{
            //    string fieldname = hr.FoxSecFieldName;
            //    dTable.Columns.Add(fieldname, typeof(string));
            //}
            for (int c = 0; c < ChildNodeList.Count; c++)
            {
                string fieldname = ChildNodeList[c].Name;
                dTable.Columns.Add(fieldname, typeof(string));
            }

            foreach (XmlNode node in nodeList)
            {
                //HRItem item = new HRItem();
                DataRow dRow = dTable.NewRow();
                //var PCode = node.SelectSingleNode("PersonalCode");
                //if (PCode != null)
                //{
                //    if (!String.IsNullOrEmpty(PCode.InnerText.Trim()))
                //    {
                //        item.id = Convert.ToInt64(PCode.InnerText);
                //    }
                //    item.PersonalCode = PCode.InnerText;
                //}
                dRow["ID"] = pkID++;
                //foreach (var hr0 in f.fsHrList)
                for (int dc = 1; dc < dTable.Columns.Count; dc++)
                {
                    string columnName = dTable.Columns[dc].ColumnName;
                    var data = node.SelectSingleNode(columnName);
                    string record = (data == null) ? "" : data.InnerText.Trim();
                    dRow[columnName] = record;
                }
                dTable.Rows.Add(dRow);
                //var fName = node.SelectSingleNode("FirstName");
                //item.FirstName = (fName == null) ? "" : fName.InnerText.Trim();

                //var lName = node.SelectSingleNode("LastName");
                //item.LastName = (lName == null) ? "" : lName.InnerText.Trim();

                //var Uname = node.SelectSingleNode("LoginName");
                //item.LoginName = (Uname == null) ? "" : Uname.InnerText.Trim();

                //var Mail = node.SelectSingleNode("Email");
                //item.Email = (Mail == null) ? "" : Mail.InnerText.Trim();

                //var CreateBy = node.SelectSingleNode("CreatedBy");
                //item.CreatedBy = (CreateBy == null) ? "" : CreateBy.InnerText.Trim();

                //var Company = node.SelectSingleNode("CompanyId");
                //item.Name = (Company == null) ? "" : Company.InnerText.Trim();

                //var fromDT = node.SelectSingleNode("ValidFrom");
                //item.ValidFrom = (fromDT == null) ? "" : fromDT.InnerText.Trim();

                //var toDt = node.SelectSingleNode("ValidTo");
                //item.ValidTo = (toDt == null) ? "" : toDt.InnerText.Trim();

                //var _serial = node.SelectSingleNode("Serial");
                //item.Serial = (_serial == null) ? "" : _serial.InnerText.Trim();

                //var _dk = node.SelectSingleNode("Dk");
                //item.Dk = (_dk == null) ? "" : _dk.InnerText.Trim();

                //hr_list.Add(item);
            }
            //users.xmlTable = new DataTable();
            //users.xmlTable = dTable;
            return dTable;
        }
    }
}