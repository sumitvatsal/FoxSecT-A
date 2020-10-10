using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoxSec.Web.Controllers
{
    public class LocationController : Controller
    {
        private readonly IUserRepository _userRepository;
        public LocationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        FoxSecDBContext db = new FoxSecDBContext();
       
        [HttpGet]
        public ActionResult NotMovedAfterToDate(NotMovedAfterToDateModel notMovedAfterToDateModel)
        {
            var userLastMoves = db.UserLastMoves.ToList();
            return null;
        }


        [HttpPost]        
        public ActionResult NotMovedAfterToDate(List<int> usersToDeactivate)
        {
            //var companiesRoles = db.UserLastMoves.Include("User").ToList();
            //db.User.Where(x => x.Id == 38).SingleOrDefault().IsDeleted=false;
            //userUpdate.IsDeleted = false;
            //db.SaveChanges();
            //db.Entry(userUpdate).State= System.Data.Entity.EntityState.Modified;

            //var filterCompaniesRoles = companiesRoles.Where(x => x.LastMoveTime <= DateTime.Now.AddDays(-60)).Distinct().ToList();
           if(usersToDeactivate != null)
            {
                foreach (int user in usersToDeactivate)
                {
                    db.User.Where(x => x.Id == (user)).SingleOrDefault().Active = false;
                    db.SaveChanges();
                }
            }
            
           
            return null;
        }
    }
}