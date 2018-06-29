using System;
using System.Collections.Generic;

using AftershipAPI.Enums;

using Newtonsoft.Json.Linq;


namespace AftershipAPI
{

    /// <summary>
    /// Tracking. Keep instances of trackings
    /// </summary>
    public class Tracking
    {
        /// <summary>Tracking ID in the Afthership system</summary>
        public string Id { get; set; }
        /// <summary>
        /// Tracking number of a shipment. 
        /// Duplicate tracking numbers, or tracking number with invalid tracking number format will not be accepted.
        /// </summary>
        public string TrackingNumber { get; set; }
        /// <summary>
        /// Unique code of each courier. If you do not specify a slug, Aftership will automatically detect
        /// the courier based on the tracking number format and your selected couriers
        /// </summary>
        public string Slug { get; set; }
        /// <summary>Email address(es) to receive email notifications. Use comma for multiple emails</summary>
        public List<string> Emails { get; set; }
        /// <summary>
        /// Phone number(s) to receive sms notifications. Use comma for multiple emails.
        /// Enter + area code before phone number
        /// </summary>
        public List<string> Smses { get; set; }
        /// <summary>Title of the tracking. Default value as trackingNumber</summary>
        public string Title { get; set; }
        /// <summary>Customer name of the tracking</summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// ISO Alpha-3(three letters)to specify the destination of the shipment.
        /// If you use postal service to send international shipments, AfterShip will automatically
        /// get tracking results at destination courier as well (e.g. USPS for USA)
        /// </summary>
        public ISO3Country DestinationCountryISO3 { get; set; }
        /// <summary>Origin country of the tracking. ISO Alpha-3</summary>
        public ISO3Country OriginCountryISO3 { get; set; }
        /// <summary>Text field for order ID</summary>
        public string OrderID { get; set; }
        /// <summary>Text field for order path</summary>
        public string OrderIDPath { get; set; }
        /// <summary>Custom fields that accept any TEXT STRING</summary>
        public Dictionary<string, string> CustomFields { get; set; }

        // fields informed by Aftership API
        /// <summary>Date and time of the tracking created</summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>Date and time of the tracking last updated</summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary> 
        /// Whether or not AfterShip will continue tracking the shipments.
        /// Value is `false` when status is `Delivered` or `Expired`
        /// </summary>
        public bool Active { get; set; }
        /// <summary>Expected delivery date (if any)</summary>
        public string ExpectedDelivery { get; set; }
        /// <summary>Number of packages under the tracking</summary>
        public int ShipmentPackageCount { get; set; }
        /// <summary>Shipment type provided by carrier (if any)</summary>
        public string ShipmentType { get; set; }
        /// <summary>Signed by information for delivered shipment (if any)</summary>
        public string SignedBy { get; set; }
        /// <summary>Source of how this tracking is added</summary>
        public string Source { get; set; }
        /// <summary>Current status of tracking</summary>
        public StatusTag Tag { get; set; }
        /// <summary>Number of attempts AfterShip tracks at courier's system</summary>
        public int TrackedCount { get; set; }
        /// <summary>Array of Hash describes the checkpoint information</summary>
        public List<Checkpoint> Checkpoints { get; set; }
        /// <summary>Unique Token</summary>
        public string UniqueToken { get; set; }
        /// <summary>Tracking Account number tracking_account_number</summary>
        public string TrackingAccountNumber { get; set; }
        /// <summary>Tracking postal code tracking_postal_code</summary>
        public string TrackingPostalCode { get; set; }
        /// <summary>Tracking ship date tracking_ship_date</summary>
        public string TrackingShipDate { get; set; }

        public Tracking(string trackingNumber)
        {
            TrackingNumber = trackingNumber;
            Title = trackingNumber;
        }

        public Tracking(JObject trackingJSON)
        {
            string destination_country_iso3;
            string origin_country_iso3;

            Id = trackingJSON["id"] == null ? null : (string)trackingJSON["id"];

            //fields that can be updated by the user
            TrackingNumber = trackingJSON["tracking_number"] == null ? null : (string)trackingJSON["tracking_number"];
            Slug = trackingJSON["slug"] == null ? null : (string)trackingJSON["slug"];
            Title = trackingJSON["title"] == null ? null : (string)trackingJSON["title"];
            CustomerName = trackingJSON["customer_name"] == null ? null : (string)trackingJSON["customer_name"];
            destination_country_iso3 = (string)trackingJSON["destination_country_iso3"];

            if (destination_country_iso3 != null && destination_country_iso3 != string.Empty)
            {
                DestinationCountryISO3 = (ISO3Country)Enum.Parse(typeof(ISO3Country), destination_country_iso3);
            }
            OrderID = trackingJSON["order_id"] == null ? null : (string)trackingJSON["order_id"];
            OrderIDPath = trackingJSON["order_id_path"] == null ? null : (string)trackingJSON["order_id_path"];
            TrackingAccountNumber = trackingJSON["tracking_account_number"] == null ? null :
                (string)trackingJSON["tracking_account_number"];
            TrackingPostalCode = trackingJSON["tracking_postal_code"] == null ? null :
                (string)trackingJSON["tracking_postal_code"];
            TrackingShipDate = trackingJSON["tracking_ship_date"] == null ? null :
                (string)trackingJSON["tracking_ship_date"];

            JArray smsesArray = trackingJSON["smses"] == null ? null : (JArray)trackingJSON["smses"];
            if (smsesArray != null && smsesArray.Count != 0)
            {
                Smses = new List<string>();
                for (var i = 0; i < smsesArray.Count; i++)
                {
                    Smses.Add((string)smsesArray[i]);
                }
            }

            JArray emailsArray = trackingJSON["emails"] == null ? null : (JArray)trackingJSON["emails"];
            if (emailsArray != null && emailsArray.Count != 0)
            {
                Emails = new List<string>();
                for (var i = 0; i < emailsArray.Count; i++)
                {
                    Emails.Add((string)emailsArray[i]);
                }
            }

            JObject customFieldsJSON = trackingJSON["custom_fields"] == null || !trackingJSON["custom_fields"].HasValues ? null :
                (JObject)trackingJSON["custom_fields"];

            if (customFieldsJSON != null)
            {
                CustomFields = new Dictionary<string, string>();
                IEnumerable<JProperty> keys = customFieldsJSON.Properties();
                foreach (var item in keys)
                {
                    CustomFields.Add(item.Name, (string)customFieldsJSON[item.Name]);
                }
            }

            //fields that can't be updated by the user, only retrieve
            CreatedAt = trackingJSON["created_at"] == null ? DateTime.MinValue : (DateTime)trackingJSON["created_at"];
            UpdatedAt = trackingJSON["updated_at"] == null ? DateTime.MinValue : (DateTime)trackingJSON["updated_at"];
            ExpectedDelivery = trackingJSON["expected_delivery"] == null ? null : (string)trackingJSON["expected_delivery"];

            Active = trackingJSON["active"] == null ? false : (bool)trackingJSON["active"];

            origin_country_iso3 = (string)trackingJSON["origin_country_iso3"];

            if (origin_country_iso3 != null && origin_country_iso3 != string.Empty)
            {
                OriginCountryISO3 = (ISO3Country)Enum.Parse(typeof(ISO3Country), origin_country_iso3);
            }
            ShipmentPackageCount = trackingJSON["shipment_package_count"] == null ? 0 :
                (int)trackingJSON["shipment_package_count"];
            ShipmentType = trackingJSON["shipment_type"] == null ? null : (string)trackingJSON["shipment_type"];
            SignedBy = trackingJSON["singned_by"] == null ? null : (string)trackingJSON["signed_by"];
            Source = trackingJSON["source"] == null ? null : (string)trackingJSON["source"];
            Tag = (string)trackingJSON["tag"] == null ? 0 :
                (StatusTag)Enum.Parse(typeof(StatusTag), (string)trackingJSON["tag"]);

            TrackedCount = trackingJSON["tracked_count"] == null ? 0 : (int)trackingJSON["tracked_count"];
            UniqueToken = trackingJSON["unique_token"] == null ? null : (string)trackingJSON["unique_token"];

            // checkpoints
            JArray checkpointsArray = trackingJSON["checkpoints"] == null ? null :
                (JArray)trackingJSON["checkpoints"];
            if (checkpointsArray != null && checkpointsArray.Count != 0)
            {
                Checkpoints = new List<Checkpoint>();
                for (var i = 0; i < checkpointsArray.Count; i++)
                {
                    Checkpoints.Add(new Checkpoint((JObject)checkpointsArray[i]));
                }
            }
        }
        
        public void AddEmails(string emails)
        {
            if (Emails == null)
            {
                Emails = new List<string>
                {
                    emails
                };
            }
            else
            {
                Emails.Add(emails);
            }
        }

        public void DeleteEmails(string email)
        {
            if (Emails != null)
            {
                Emails.Remove(email);
            }
        }

        public void AddSmses(string smses)
        {
            if (Smses == null)
            {
                Smses = new List<string>
                {
                    smses
                };
            }
            else
            {
                Smses.Add(smses);
            }
        }

        public void DeleteSmses(string smses)
        {
            if (Smses != null)
            {
                Smses.Remove(smses);
            }
        }
        
        public void AddCustomFields(string field, string value)
        {

            if (CustomFields == null)
            {
                CustomFields = new Dictionary<string, string>();
            }
            CustomFields.Add(field, value);
        }

        public void DeleteCustomFields(string field)
        {
            if (CustomFields != null)
            {
                CustomFields.Remove(field);
            }
        }
        
        public string GetJSONPost()
        {
            var globalJSON = new JObject();
            var trackingJSON = new JObject
            {
                //	trackingJSON.Add("hola",
                { "tracking_number", new JValue(TrackingNumber) }
            };
            if (Slug != null)
            {
                trackingJSON.Add("slug", new JValue(Slug));
            }

            if (Title != null)
            {
                trackingJSON.Add("title", new JValue(Title));
            }

            if (Emails != null)
            {
                var emailsJSON = new JArray(Emails);
                trackingJSON["emails"] = emailsJSON;
            }
            if (Smses != null)
            {
                var smsesJSON = new JArray(Smses);
                trackingJSON["smses"] = smsesJSON;
            }
            if (CustomerName != null)
            {
                trackingJSON.Add("customer_name", new JValue(CustomerName));
            }

            if (DestinationCountryISO3 != 0)
            {
                trackingJSON.Add("destination_country_iso3", new JValue(DestinationCountryISO3.ToString()));
            }

            if (OrderID != null)
            {
                trackingJSON.Add("order_id", new JValue(OrderID));
            }

            if (OrderIDPath != null)
            {
                trackingJSON.Add("order_id_path", new JValue(OrderIDPath));
            }

            if (TrackingAccountNumber != null)
            {
                trackingJSON.Add("tracking_account_number", new JValue(TrackingAccountNumber));
            }

            if (TrackingPostalCode != null)
            {
                trackingJSON.Add("tracking_postal_code", new JValue(TrackingPostalCode));
            }

            if (TrackingShipDate != null)
            {
                trackingJSON.Add("tracking_ship_date", new JValue(TrackingShipDate));
            }

            if (CustomFields != null)
            {
                var customFieldsJSON = new JObject();
                foreach (KeyValuePair<string, string> pair in CustomFields)
                {
                    customFieldsJSON.Add(pair.Key, new JValue(pair.Value));
                }

                trackingJSON["custom_fields"] = customFieldsJSON;
            }
            
            globalJSON["tracking"] = trackingJSON;

            return globalJSON.ToString();
        }
        
        public string GeneratePutJSON()
        {
            var globalJSON = new JObject();
            var trackingJSON = new JObject();
            JObject customFieldsJSON;

            if (Title != null)
            {
                trackingJSON.Add("title", new JValue(Title));
            }

            if (Emails != null)
            {
                var emailsJSON = new JArray(Emails);
                trackingJSON["emails"] = emailsJSON;
            }
            if (Smses != null)
            {
                var smsesJSON = new JArray(Smses);
                trackingJSON["smses"] = smsesJSON;
            }
            if (CustomerName != null)
            {
                trackingJSON.Add("customer_name", new JValue(CustomerName));
            }

            if (OrderID != null)
            {
                trackingJSON.Add("order_id", new JValue(OrderID));
            }

            if (OrderIDPath != null)
            {
                trackingJSON.Add("order_id_path", new JValue(OrderIDPath));
            }

            if (CustomFields != null)
            {
                customFieldsJSON = new JObject();

                foreach (KeyValuePair<string, string> pair in CustomFields)
                {
                    customFieldsJSON.Add(pair.Key, new JValue(pair.Value));
                }
                trackingJSON["custom_fields"] = customFieldsJSON;
            }
            globalJSON["tracking"] = trackingJSON;

            return globalJSON.ToString();
        }

        public string GetQueryRequiredFields()
        {
            var containsInfo = false;
            var qs = new QueryString();
            if (TrackingAccountNumber != null)
            {
                containsInfo = true;
                qs.Add("tracking_account_number", TrackingAccountNumber);
            }
            if (TrackingPostalCode != null)
            {
                qs.Add("tracking_postal_code", TrackingPostalCode);
                containsInfo = true;
            }
            if (TrackingShipDate != null)
            {
                qs.Add("tracking_ship_date", TrackingShipDate);
                containsInfo = true;
            }
            if (containsInfo)
            {
                return qs.ToString();
            }
            return "";
        }

        public override string ToString() => "_id: " + Id + "\n_trackingNumber: " + TrackingNumber + "\n_slug:" + Slug;
    }
}



