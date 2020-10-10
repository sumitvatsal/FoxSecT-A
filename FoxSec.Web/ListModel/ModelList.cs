using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ListModel
{
    public class ModelList
    {
        public IEnumerable<FoxSec.Web.ViewModels.datatable> datatables { get; set; }
    }
    public class box
    {
        public DateTime RegistredStartDate { get; set; }
        public DateTime RegistredEndDate { get; set; }
        public int companyId { get; internal set; }
    }
    public class ViewAccessDetails
    {
        public string CameraName { get; set; }
    }

    public class box2
    {
        public int id { get; set; }
        
    }
    public class CameraDetals
    {
        public string CameraName { get; set; }
        public int Id { get; set; }
        public int CompanyId { get; set; }
    }
    public class cameraData
    {
      
        public int CameraID { get; set; }
        public string CameraName { get; set; }
        public string Status { get; set; }
    }
    public class box3
    {
        public int CompanyId { get; set; }

    }
    public class box4
    {
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public int CompanyId { get; set; }

    }
    public class box5
    {
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
       

    }
    public class box6
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }


    }
    public class box7
    {
        public int ID { get; set; }
        public string Name { get; set; }


    }

    public class box8
    {
        public int CompanyID { get; set; }
        public int RoleID { get; set; }
        public string Name { get; set; }


    }

    public class box9
    {
        public int CameraID { get; set; }
        public int CompanyID { get; set; }
        


    }
    public class box10
    {
       
        public int ID { get; set; }
        //public int BuildingObjectid { get; set; }

       public int BuildingId { get; set; }

        public int CameraId { get; set; }

        public int count { get; set; }
    }

    public class UserAccessUnit_temp
    {
        // (ValidFrom,ValidTo,Active,IsDeleted,CreatedBy,BuildingId,Free,UserId) 

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public int BuildingId { get; set; }
        public bool Free { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
    }
    public class UserAccessUnitInsert
    {
       // (ValidFrom,ValidTo,Active,IsDeleted,CreatedBy,BuildingId,Free,UserId) 

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public int BuildingId { get; set; }
        public bool Free { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
    }
}