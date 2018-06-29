using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace AftershipAPI
{
    public class Courier
    {
        /** Unique code of courier */
        public string Slug { get; set; }
        /** Name of courier */
        public string Name { get; set; }
        /** Contact phone number of courier */
        public string Phone { get; set; }
        /** Other name of courier, if several they will be separated by commas */
        public string OtherName { get; set; }
        /** Website link of courier */
        public string WebUrl { get; set; }
        /** Require fields for this courier */
        public List<string> RequireFields { get; set; }

        /** Default constructor with all the fields of the class */
        public Courier(string web_url, string slug, string name, string phone, string other_name)
        {
            WebUrl = web_url;
            Slug = slug;
            Name = name;
            Phone = phone;
            OtherName = other_name;
        }

        /**
     * Constructor, creates a Courier from a JSONObject with the information of the Courier,
     * if any field is not specified it will be ""
     *
     * @param jsonCourier   A JSONObject with information of the Courier
     * by the API.
     **/           // _trackingNumber = trackingJSON["tracking_number"]==null?null:(String)trackingJSON["tracking_number"];

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

