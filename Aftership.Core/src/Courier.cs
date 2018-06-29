using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace AftershipAPI
{
    public class Courier
    {
        /// <summary>Unique code of courier</summary>
        public string Slug { get; set; }
        /// <summary>Name of courier</summary>
        public string Name { get; set; }
        /// <summary>Contact phone number of courier</summary>
        public string Phone { get; set; }
        /// <summary>Other name of courier, if several they will be separated by commas</summary>
        public string OtherName { get; set; }
        /// <summary>Website link of courier</summary>
        public string WebUrl { get; set; }
        /// <summary>Require fields for this courier</summary>
        
        public List<string> RequireFields { get; set; }

        /// <summary>
        /// Default constructor with all the fields of the class
        /// </summary>
        /// <param name="web_url"></param>
        /// <param name="slug"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="other_name"></param>
        public Courier(string web_url, string slug, string name, string phone, string other_name)
        {
            WebUrl = web_url;
            Slug = slug;
            Name = name;
            Phone = phone;
            OtherName = other_name;
        }
       
        /// <summary>
        /// creates a Courier from a JSONObject with the information of the Courier. 
        /// If any field is not specified it will be ""
        /// </summary>
        /// <param name="jsonCourier">A JSONObject with information of the Courier by the API</param>
        public Courier(JObject jsonCourier)
        {
            WebUrl = jsonCourier["web_url"] == null ? null : (string)jsonCourier["web_url"];
            Slug = jsonCourier["slug"] == null ? null : (string)jsonCourier["slug"];
            Name = jsonCourier["name"] == null ? null : (string)jsonCourier["name"];
            Phone = jsonCourier["phone"] == null ? null : (string)jsonCourier["phone"];
            OtherName = jsonCourier["other_name"] == null ? null : (string)jsonCourier["other_name"];

            JArray requireFieldsArray = jsonCourier["required_fields"] == null ? null : (JArray)jsonCourier["required_fields"];
            if (requireFieldsArray != null && requireFieldsArray.Count != 0)
            {
                RequireFields = new List<string>();
                for (var i = 0; i < requireFieldsArray.Count; i++)
                {
                    RequireFields.Add(requireFieldsArray[i].ToString());
                }
            }
        }

        public string TooString()
        {
            return "Courier{" +
                "slug='" + Slug + '\'' +
                ", name='" + Name + '\'' +
                ", phone='" + Phone + '\'' +
                ", other_name='" + OtherName + '\'' +
                ", web_url='" + WebUrl + '\'' +
                '}';
        }

        public void AddRequireField(string requireField)
        {
            if (RequireFields == null)
            {
                RequireFields = new List<string>
                {
                    requireField
                };
            }
            else
            {
                RequireFields.Add(requireField);
            }
        }

        public void DeleteRequireField(string requireField)
        {
            if (RequireFields != null)
            {
                RequireFields.Remove(requireField);
            }
        }

        public void DeleteRequireFields() => RequireFields = null;
    }
}

