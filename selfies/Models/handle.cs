//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace selfies.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    
    public partial class handle
    {
        public int id { get; set; }
        public string name { get; set; }
        public string userGuid { get; set; }
        public Nullable<int> active { get; set; }
        public string publicKey { get; set; }

        [JsonIgnore]
        public string privateKey { get; set; }
        public string tagLine { get; set; }
        public int unreadMessages { get; set; }
    }
}
