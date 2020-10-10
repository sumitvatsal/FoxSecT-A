using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoxSec.DomainModel.DomainObjects
{
    public class Building : EntityName
    {
       // public int Id { get; set; }
        public virtual string AdressStreet { get; set; }

        public virtual string AdressHouse { get; set; }

        public virtual string AdressIndex { get; set; }

        public virtual int LocationId { get; set; }
        
        public virtual int Floors { get; set; }
        //public virtual int TimediffGMTMinutes { get; set; }
        public Nullable<int> TimediffGMTMinutes { get; set; }

        public virtual string TimezoneId { get; set; }
        

        /*
         * from Building poco class***
        public int Id { get; set; }
        public string AdressStreet { get; set; }
        public string AdressHouse { get; set; }
        public string AdressIndex { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int Floors { get; set; }
        public Nullable<int> TimediffGMTMinutes { get; set; }
        public string TimezoneId { get; set; }
    
             */

        // relations:

        public virtual Location Location { get; set; }
      
        public virtual ICollection<BuildingObject> BuildingObjects { get; set; }

		public virtual ICollection<RoleBuilding> RoleBuildings { get; set; }

		public virtual ICollection<UsersAccessUnit> UsersAccessUnits { get; set; }

		public virtual ICollection<UserBuilding> UserBuildings { get; set; }
        public virtual ICollection<TAReport> TAReports { get; set; }
        
    }
}