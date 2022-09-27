using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Cen.Wms.Client.Models.Dtos.GosZNKAK
{

    public class GosZNAKLabels
    {
        public Label label { get; set; }
        public User user { get; set; }
        public Item item { get; set; }
        public string error { get; set; }
        public string message { get; set; }
        public object[] story { get; set; }
    }

    public class Label
    {
        public string uuid { get; set; }
        public string snomer { get; set; }
        public Type type { get; set; }
        public Status status { get; set; }
    }

    public class Type
    {
        public string id { get; set; }
        public string name { get; set; }
        public string method { get; set; }
        public object digits_count { get; set; }
        public bool is_bso { get; set; }
        public string letters_count { get; set; }
        public bool is_autotake { get; set; }
    }

    public class Status
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public class User
    {
        public string name { get; set; }
        public string address { get; set; }
        public string unp { get; set; }
        public string gln { get; set; }
        public string country { get; set; }
        public bool is_rules_agree { get; set; }
    }

    public class Item
    {
        string _gtin;
        public string name { get; set; }
        public string image { get; set; }
        public string articul { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string gtin
        {
            get { return _gtin; }
            set {
                if (string.IsNullOrEmpty(value))
                    _gtin = "Пусто";
                else
                    _gtin = value; 
            }
        }
        public bool gtin_check { get; set; }
        public Group group { get; set; }
        public object document_id { get; set; }
        public object tnved { get; set; }
        public bool is_my { get; set; }
        public Catalog catalog { get; set; }
        public object[] _params { get; set; }
        public object[] mark_params { get; set; }
    }

    public class Group
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Catalog
    {
        public string name { get; set; }
        public string group { get; set; }
        public string code { get; set; }
    }

    public class lblDTO
    {
        [JsonProperty("label")]
        public string label { get; set; }
    }
}
