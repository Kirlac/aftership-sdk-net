using System;

using AftershipAPI.Enums;

using Newtonsoft.Json.Linq;


namespace AftershipAPI
{
    public class Checkpoint
    {
        /// <summary>Date and time of the tracking created</summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Date and time of the checkpoint, provided by courier. Value may be:
        /// Empty String,
        /// YYYY-MM-DD,
        /// YYYY-MM-DDTHH:MM:SS, or
        /// YYYY-MM-DDTHH:MM:SS+TIMEZONE
        /// </summary>
        public string CheckpointTime { get; set; }
        /// <summary>Location info (if any)</summary>
        public string City { get; set; }
        /// <summary>Country ISO Alpha-3 (three letters) of the checkpoint</summary>
        public ISO3Country CountryISO3 { get; set; }
        /// <summary>Country name of the checkpoint, may also contain other location info</summary>
        public string CountryName { get; set; }
        /// <summary>Checkpoint message</summary>
        public string Message { get; set; }
        /// <summary>Location info (if any)</summary>
        public string State { get; set; }
        /// <summary>Status of the checkpoint</summary>
        public string Tag { get; set; }
        /// <summary>Location info (if any)</summary>
        public string Zip { get; set; }

        public Checkpoint(JObject checkpointJSON)
        {
            // Console.WriteLibe(typeof(checkpointJSON["created_at"]));
            CreatedAt = checkpointJSON["created_at"] == null ? DateTime.MinValue :
                (DateTime)checkpointJSON["created_at"];
            CheckpointTime = checkpointJSON["checkpoint_time"] == null ? null : (string)checkpointJSON["checkpoint_time"];
            City = checkpointJSON["city"] == null ? null : (string)checkpointJSON["city"];
            CountryISO3 = checkpointJSON["country_iso3"] == null ? 0 :
                (ISO3Country)Enum.Parse(typeof(ISO3Country), (string)checkpointJSON["country_iso3"]);
            CountryName = checkpointJSON["country_name"] == null ? null : (string)checkpointJSON["country_name"];
            Message = checkpointJSON["message"] == null ? null : (string)checkpointJSON["message"];
            State = checkpointJSON["state"] == null ? null : (string)checkpointJSON["state"];
            Tag = checkpointJSON["tag"] == null ? null : (string)checkpointJSON["tag"];
            Zip = checkpointJSON["zip"] == null ? null : (string)checkpointJSON["zip"];
        }
    }
}

