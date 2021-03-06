﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using selfies.Models;
using System.Web.Security;
using System.Net.Mail;


namespace selfies.Controllers
{
    public class HandleController : ApiController
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


        // GET api/values
        public IEnumerable<handle> Get()
        {
            if (User.IsInRole("mod"))
            {
                return (from m in db.handles where m.active == 1 select m).ToList();
            }
            else
            {
                string user_id = User.Identity.Name;
                handle logged_in = (from handle r in db.handles where r.userGuid.Equals(User.Identity.Name) select r).FirstOrDefault();
                return new List<handle> { logged_in };
            }
        }

        // GET api/values/5
        public handle Get(int id)
        {
            handle selected = (from m in db.handles where m.id.Equals(id) select m).FirstOrDefault();
            return selected;
        }


        // POST api/values
        public handle Post(handle value)
        {

            handle error = new handle();
            error.active = -1;
            error.name = "There was an error.";


            if (value != null)
            {
                handle currently_exists = (from m in db.handles where m.name.Equals(value.name) && m.active == 1 select m).FirstOrDefault();

                if (currently_exists == null)
                {

                    handle safe_handle = new handle();
                    Guid public_key = Guid.NewGuid();
                    Guid private_key = Guid.NewGuid();

                    // Attempt to register the user
                    MembershipCreateStatus createStatus;
                    Membership.CreateUser(public_key.ToString(), private_key.ToString(), "anon", null, null, true, null, out createStatus);

                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        FormsAuthentication.SetAuthCookie(public_key.ToString(), true /* createPersistentCookie */);
                        safe_handle.name = value.name;
                        safe_handle.userGuid = public_key.ToString();
                        safe_handle.active = 1;
                        // free handle yay

                        safe_handle.publicKey = public_key.ToString();
                        safe_handle.privateKey = private_key.ToString();

                        // send email about it

                        MailMessage Message = new MailMessage();
                        SmtpClient Smtp = new SmtpClient();

                        string password = System.Web.Configuration.WebConfigurationManager.AppSettings["MailPassword"];

                        System.Net.NetworkCredential SmtpUser = new System.Net.NetworkCredential("noreply@letterstocrushes.com", password);

                        string email = "new handle registered: \n\n";
                        email = safe_handle.name + "\n\n";

                        Message.From = new MailAddress("hello@selfies.io");
                        Message.To.Add(new MailAddress("seth.hayward@gmail.com"));
                        Message.IsBodyHtml = false;
                        Message.Subject = "new handle";
                        Message.Body = email;
                        Message.Priority = MailPriority.Normal;
                        Smtp.EnableSsl = false;

                        Smtp.Credentials = SmtpUser;
                        Smtp.Host = "198.57.199.92";
                        Smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        Smtp.Port = 26;

                        Smtp.Send(Message);

                        db.handles.Add(safe_handle);
                        db.SaveChanges();
                        return safe_handle;
                    }
                    else
                    {
                        return error;
                    }

                }
                else
                {

                    handle taken = new handle();
                    taken.active = 0;
                    taken.name = value.name;
                    return taken;

                }

            }

            return error;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

    }
}
