using FoxSec.Web.ListModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FoxSec.Web.ViewModels
{
    public class Node
    {
        public int ParentId = 0;
        public int MyId = 0;
        public int BuildingId = 0;
        public string Name = String.Empty;
        public string Comment = String.Empty;
        public bool IsDefaultTimeZone = true;
        public int IsRoom = 0;
        public bool IsArming = false;
        public bool IsDefaultArming = false;
        public bool IsDisarming = false;
        public bool IsDefaultDisarming = false;
        public string StatusIcon = String.Empty;
        public string TimeZoneName;

    }

    class NodeEqualityComparer : IEqualityComparer<Node>
    {
        public bool Equals(Node x, Node y)
        {
            return x.ParentId.Equals(y.ParentId) && x.MyId.Equals(y.MyId);
        }

        public int GetHashCode(Node obj)
        {
            return (obj.ParentId & obj.MyId).GetHashCode();
        }
    }

    public class CompanyTreeViewModel : ViewModelBase
    {
        public IEnumerable<Node> Countries;
        public IEnumerable<Node> Towns;
        public IEnumerable<Node> Offices;
        public IEnumerable<Node> Companies;
        public IEnumerable<Node> Partners;
        public IEnumerable<Node> Floors;
         public IEnumerable<Node> Cameras;
       

    }
}