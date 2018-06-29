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

        /// <summary>
        /// Tracking number of a shipment. 
        /// Duplicate tracking numbers, or tracking number with invalid tracking number format will not be accepted.
        /// </summary>

        /// <summary>
        /// Unique code of each courier. If you do not specify a slug, Aftership will automatically detect
        /// the courier based on the tracking number format and your selected couriers
        /// </summary>

        /// <summary>Email address(es) to receive email notifications. Use comma for multiple emails</summary>

        /// <summary>
        /// Phone number(s) to receive sms notifications. Use comma for multiple emails.
        /// Enter + area code before phone number
        /// </summary>

        /// <summary>Title of the tracking. Default value as trackingNumber</summary>

        /// <summary>Customer name of the tracking</summary>

        /// <summary>
        /// ISO Alpha-3(three letters)to specify the destination of the shipment.
        /// If you use postal service to send international shipments, AfterShip will automatically
        /// get tracking results at destination courier as well (e.g. USPS for USA)
        /// </summary>

        /// <summary>Origin country of the tracking. ISO Alpha-3</summary>

        /// <summary>Text field for order ID</summary>

        /// <summary>Text field for order path</summary>

        /// <summary>Custom fields that accept any TEXT STRING</summary>
        public int i = 0;

        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _trackingNumber;
        public string TrackingNumber
        {
            get { return _trackingNumber; }
            set { _trackingNumber = value; }
        }
        private string _slug;
        public string Slug
        {
            get { return _slug; }
            set { _slug = value; }
        }
        private List<string> _emails;
        public List<string> Emails
        {
            get { return _emails; }
            set { _emails = value; }
        }
        private List<string> _smses;
        public List<string> Smses
        {
            get { return _smses; }
            set { _smses = value; }
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
        }
        private ISO3Country _destinationCountryISO3;
        public ISO3Country DestinationCountryISO3
        {
            get { return _destinationCountryISO3; }
            set { _destinationCountryISO3 = value; }
        }
        private ISO3Country _originCountryISO3;
        public ISO3Country OriginCountryISO3
        {
            get { return _originCountryISO3; }
            set { _originCountryISO3 = value; }
        }
        private string _orderID;
        public string OrderID
        {
            get { return _orderID; }
            set { _orderID = value; }
        }
        private string _orderIDPath;
        public string OrderIDPath
        {
            get { return _orderIDPath; }
            set { _orderIDPath = value; }
        }
        private Dictionary<string, string> _customFields;
        public Dictionary<string, string> CustomFields
        {
            get { return _customFields; }
            set { _customFields = value; }
        }

        // fields informed by Aftership API
        /// <summary>Date and time of the tracking created</summary>

        /// <summary>Date and time of the tracking last updated</summary>

        /// <summary> 
        /// Whether or not AfterShip will continue tracking the shipments.
        /// Value is `false` when status is `Delivered` or `Expired`
        /// </summary>

        /// <summary>Expected delivery date (if any)</summary>

        /// <summary>Number of packages under the tracking</summary>

        /// <summary>Shipment type provided by carrier (if any)</summary>

        /// <summary>Signed by information for delivered shipment (if any)</summary>

        /// <summary>Source of how this tracking is added</summary>

        /// <summary>Current status of tracking</summary>

        /// <summary>Number of attempts AfterShip tracks at courier's system</summary>

        /// <summary>Array of Hash describes the checkpoint information</summary>

        /// <summary>Unique Token</summary>

        /// <summary>Tracking Account number tracking_account_number</summary>

        /// <summary>Tracking postal code tracking_postal_code</summary>

        /// <summary>Tracking ship date tracking_ship_date</summary>
        public int apifields = 0;

        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }
        private DateTime _updatedAt;
        public DateTime UpdatedAt
        {
            get { return _updatedAt; }
            set { _updatedAt = value; }
        }
        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }
        private string _expectedDelivery;
        public string ExpectedDelivery
        {
            get { return _expectedDelivery; }
            set { _expectedDelivery = value; }
        }
        private int _shipmentPackageCount;
        public int ShipmentPackageCount
        {
            get { return _shipmentPackageCount; }
            set { _shipmentPackageCount = value; }
        }
        private string _shipmentType;
        public string ShipmentType
        {
            get { return _shipmentType; }
            set { _shipmentType = value; }
        }
        private string _signedBy;
        public string SignedBy
        {
            get { return _signedBy; }
            set { _signedBy = value; }
        }
        private string _source;
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }
        private StatusTag _tag;
        public StatusTag Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        private int _trackedCount;
        public int TrackedCount
        {
            get { return _trackedCount; }
            set { _trackedCount = value; }
        }
        private List<Checkpoint> _checkpoints;
        public List<Checkpoint> Checkpoints
        {
            get { return _checkpoints; }
            set { _checkpoints = value; }
        }
        private string _uniqueToken;
        public string UniqueToken
        {
            get { return _uniqueToken; }
            set { _uniqueToken = value; }
        }
        private string _trackingAccountNumber;
        public string TrackingAccountNumber
        {
            get { return _trackingAccountNumber; }
            set { _trackingAccountNumber = value; }
        }
        private string _trackingPostalCode;
        public string TrackingPostalCode
        {
            get { return _trackingPostalCode; }
            set { _trackingPostalCode = value; }
        }
        private string _trackingShipDate;
        public string TrackingShipDate
        {
            get { return _trackingShipDate; }
            set { _trackingShipDate = value; }
        }
        
        public Tracking(string trackingNumber)
        {
            _trackingNumber = trackingNumber;
            _title = trackingNumber;
        }

        public Tracking(JObject trackingJSON)
        {
            string destination_country_iso3;
            string origin_country_iso3;

            Id = trackingJSON["id"] == null ? null : (string)trackingJSON["id"];

            //fields that can be updated by the user
            _trackingNumber = trackingJSON["tracking_number"] == null ? null : (string)trackingJSON["tracking_number"];
            _slug = trackingJSON["slug"] == null ? null : (string)trackingJSON["slug"];
            _title = trackingJSON["title"] == null ? null : (string)trackingJSON["title"];
            _customerName = trackingJSON["customer_name"] == null ? null : (string)trackingJSON["customer_name"];
            destination_country_iso3 = (string)trackingJSON["destination_country_iso3"];

            if (destination_country_iso3 != null && destination_country_iso3 != string.Empty)
            {
                _destinationCountryISO3 = (ISO3Country)Enum.Parse(typeof(ISO3Country), destination_country_iso3);
            }
            _orderID = trackingJSON["order_id"] == null ? null : (string)trackingJSON["order_id"];
            _orderIDPath = trackingJSON["order_id_path"] == null ? null : (string)trackingJSON["order_id_path"];
            _trackingAccountNumber = trackingJSON["tracking_account_number"] == null ? null :
                (string)trackingJSON["tracking_account_number"];
            _trackingPostalCode = trackingJSON["tracking_postal_code"] == null ? null :
                (string)trackingJSON["tracking_postal_code"];
            _trackingShipDate = trackingJSON["tracking_ship_date"] == null ? null :
                (string)trackingJSON["tracking_ship_date"];

            JArray smsesArray = trackingJSON["smses"] == null ? null : (JArray)trackingJSON["smses"];
            if (smsesArray != null && smsesArray.Count != 0)
            {
                _smses = new List<string>();
                for (var i = 0; i < smsesArray.Count; i++)
                {
                    _smses.Add((string)smsesArray[i]);
                }
            }

            JArray emailsArray = trackingJSON["emails"] == null ? null : (JArray)trackingJSON["emails"];
            if (emailsArray != null && emailsArray.Count != 0)
            {
                _emails = new List<string>();
                for (var i = 0; i < emailsArray.Count; i++)
                {
                    _emails.Add((string)emailsArray[i]);
                }
            }

            JObject customFieldsJSON = trackingJSON["custom_fields"] == null || !trackingJSON["custom_fields"].HasValues ? null :
                (JObject)trackingJSON["custom_fields"];

            if (customFieldsJSON != null)
            {
                _customFields = new Dictionary<string, string>();
                IEnumerable<JProperty> keys = customFieldsJSON.Properties();
                foreach (var item in keys)
                {
                    _customFields.Add(item.Name, (string)customFieldsJSON[item.Name]);
                }
            }

            //fields that can't be updated by the user, only retrieve
            _createdAt = trackingJSON["created_at"] == null ? DateTime.MinValue : (DateTime)trackingJSON["created_at"];
            _updatedAt = trackingJSON["updated_at"] == null ? DateTime.MinValue : (DateTime)trackingJSON["updated_at"];
            _expectedDelivery = trackingJSON["expected_delivery"] == null ? null : (string)trackingJSON["expected_delivery"];

            _active = trackingJSON["active"] == null ? false : (bool)trackingJSON["active"];

            origin_country_iso3 = (string)trackingJSON["origin_country_iso3"];

            if (origin_country_iso3 != null && origin_country_iso3 != string.Empty)
            {
                _originCountryISO3 = (ISO3Country)Enum.Parse(typeof(ISO3Country), origin_country_iso3);
            }
            _shipmentPackageCount = trackingJSON["shipment_package_count"] == null ? 0 :
                (int)trackingJSON["shipment_package_count"];
            _shipmentType = trackingJSON["shipment_type"] == null ? null : (string)trackingJSON["shipment_type"];
            _signedBy = trackingJSON["singned_by"] == null ? null : (string)trackingJSON["signed_by"];
            _source = trackingJSON["source"] == null ? null : (string)trackingJSON["source"];
            _tag = (string)trackingJSON["tag"] == null ? 0 :
                (StatusTag)Enum.Parse(typeof(StatusTag), (string)trackingJSON["tag"]);

            _trackedCount = trackingJSON["tracked_count"] == null ? 0 : (int)trackingJSON["tracked_count"];
            _uniqueToken = trackingJSON["unique_token"] == null ? null : (string)trackingJSON["unique_token"];

            // checkpoints
            JArray checkpointsArray = trackingJSON["checkpoints"] == null ? null :
                (JArray)trackingJSON["checkpoints"];
            if (checkpointsArray != null && checkpointsArray.Count != 0)
            {
                _checkpoints = new List<Checkpoint>();
                for (var i = 0; i < checkpointsArray.Count; i++)
                {
                    _checkpoints.Add(new Checkpoint((JObject)checkpointsArray[i]));
                }
            }
        }
        
        public void AddEmails(string emails)
        {
            if (_emails == null)
            {
                _emails = new List<string>
                {
                    emails
                };
            }
            else
            {
                _emails.Add(emails);
            }
        }

        public void DeleteEmails(string email)
        {
            if (_emails != null)
            {
                _emails.Remove(email);
            }
        }

        public void AddSmses(string smses)
        {
            if (_smses == null)
            {
                _smses = new List<string>
                {
                    smses
                };
            }
            else
            {
                _smses.Add(smses);
            }
        }

        public void DeleteSmses(string smses)
        {
            if (_smses != null)
            {
                _smses.Remove(smses);
            }
        }
        
        public void AddCustomFields(string field, string value)
        {

            if (_customFields == null)
            {
                _customFields = new Dictionary<string, string>();
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
                { "tracking_number", new JValue(_trackingNumber) }
            };
            if (_slug != null)
            {
                trackingJSON.Add("slug", new JValue(_slug));
            }

            if (_title != null)
            {
                trackingJSON.Add("title", new JValue(_title));
            }

            if (_emails != null)
            {
                var emailsJSON = new JArray(_emails);
                trackingJSON["emails"] = emailsJSON;
            }
            if (_smses != null)
            {
                var smsesJSON = new JArray(_smses);
                trackingJSON["smses"] = smsesJSON;
            }
            if (_customerName != null)
            {
                trackingJSON.Add("customer_name", new JValue(_customerName));
            }

            if (_destinationCountryISO3 != 0)
            {
                trackingJSON.Add("destination_country_iso3", new JValue(_destinationCountryISO3.ToString()));
            }

            if (_orderID != null)
            {
                trackingJSON.Add("order_id", new JValue(_orderID));
            }

            if (_orderIDPath != null)
            {
                trackingJSON.Add("order_id_path", new JValue(_orderIDPath));
            }

            if (_trackingAccountNumber != null)
            {
                trackingJSON.Add("tracking_account_number", new JValue(_trackingAccountNumber));
            }

            if (_trackingPostalCode != null)
            {
                trackingJSON.Add("tracking_postal_code", new JValue(TrackingPostalCode));
            }

            if (_trackingShipDate != null)
            {
                trackingJSON.Add("tracking_ship_date", new JValue(TrackingShipDate));
            }

            if (_customFields != null)
            {
                var customFieldsJSON = new JObject();
                foreach (KeyValuePair<string, string> pair in _customFields)
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

            if (_title != null)
            {
                trackingJSON.Add("title", new JValue(_title));
            }

            if (_emails != null)
            {
                var emailsJSON = new JArray(_emails);
                trackingJSON["emails"] = emailsJSON;
            }
            if (Smses != null)
            {
                var smsesJSON = new JArray(_smses);
                trackingJSON["smses"] = smsesJSON;
            }
            if (_customerName != null)
            {
                trackingJSON.Add("customer_name", new JValue(_customerName));
            }

            if (_orderID != null)
            {
                trackingJSON.Add("order_id", new JValue(_orderID));
            }

            if (_orderIDPath != null)
            {
                trackingJSON.Add("order_id_path", new JValue(_orderIDPath));
            }

            if (_customFields != null)
            {
                customFieldsJSON = new JObject();

                foreach (KeyValuePair<string, string> pair in _customFields)
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

        public override string ToString() => "_id: " + _id + "\n_trackingNumber: " + _trackingNumber + "\n_slug:" + _slug;
    }
}



