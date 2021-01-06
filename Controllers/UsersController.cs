using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Glostest;

namespace Glostest.Controllers
{
    public class UsersController : ApiController
    {
        private WordModel db = new WordModel();

        // GET: api/Users
        public IQueryable<User> GetUser()
        {
            return db.User;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}