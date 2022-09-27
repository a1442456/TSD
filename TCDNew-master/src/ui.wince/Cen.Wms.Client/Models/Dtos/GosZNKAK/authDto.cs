using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Cen.Wms.Client.Models.Dtos.GosZNKAK
{

    public class GosZNAKAuthCred
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool is_rules_agree { get; set; }
    }

    public class GosZNAKAuth
    {
        public string token { get; set; }
        public object user { get; set; }
    }

    public class UserAuth
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public int status { get; set; }
        public string expires_in { get; set; }
        public Role role { get; set; }
        public int info_id { get; set; }
        public bool need_change_password { get; set; }
        public Info info { get; set; }
    }

    public class Role
    {
        public string description { get; set; }
        public string code { get; set; }
    }

    public class Info
    {
        public string name { get; set; }
        public string address { get; set; }
        public string unp { get; set; }
        public string gln { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public bool is_rules_agree { get; set; }
    }
}
