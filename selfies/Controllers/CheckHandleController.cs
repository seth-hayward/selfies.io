﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using selfies.Models;

namespace selfies.Controllers
{
    public class CheckHandleController : ApiController
    {

        private selfiesMySQL _db;
        public selfiesMySQL db
        {
            get
            {
                if (_db == null)
                {
                    _db = new selfiesMySQL();
                }
                return _db;
            }
            set
            {
                _db = value;
            }
        }


        // POST api/values
        public RODResponseMessage Post(handle check_handle)
        {

            handle logged_in = (from handle r in db.handles where r.userGuid.Equals(User.Identity.Name) select r).FirstOrDefault();

            if (logged_in == null)
            {
                return new RODResponseMessage { message = "load error", result = 0 };
            }
            else
            {
                return new RODResponseMessage { message = logged_in.privateKey.Substring(0, 5), result = 1 };
            }

        }

    }
}
