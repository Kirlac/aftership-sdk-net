using System;
using System.Collections.Generic;

using AftershipAPI.Enums;

namespace AftershipAPI
{
    /// <summary>Keep the information for get trackings from the server, and interact with the results</summary>
    /// <remarks>Created by User on 13/6/14</remarks>
    public class ParametersTracking
    {
        /// <summary>Page to show. (Default: 1)</summary>
        public int Page { get; set; }
        /// <summary>Number of trackings each page contain. (Default and max: 100)</summary>
        public int Limit { get; set; }
        /// <summary>
        /// Search the content of the tracking record fields: 
        /// trackingNumber, title, orderId, customerName, customFields, orderId, emails, smses
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// Start date and time of trackings created. AfterShip only stores data of 90 days.
        /// (Defaults: 30 days ago, Example: 2013-03-15T16:41:56+08:00)
        /// </summary>
        public DateTime CreatedAtMin { get; set; }
        /// <summary>
        /// End date and time of trackings created. (Defaults: now, Example: 2013-04-15T16:41:56+08:00)
        /// </summary>
        public DateTime CreatedAtMax { get; set; }
        /// <summary>
        /// Language, default: ''
        /// Example: 'en' Support Chinese to English translation for  china-ems and  china-post only
        /// </summary>
        public string Lang { get; set; }
        /// <summary>Total of tracking elements from the user that match the ParametersTracking object</summary>
        public int Total { get; set; }

        /// <summary>
        /// Unique courier code Use comma for multiple values. (Example: dhl,ups,usps)
        /// </summary>
        private List<string> _slugs;
        /// <summary>
        /// Origin country of trackings. Use ISO Alpha-3 (three letters). (Example: USA,HKG)
        /// </summary>
        private List<ISO3Country> _origins;
        /// <summary>
        /// Destination country of trackings. Use ISO Alpha-3 (three letters). (Example: USA,HKG)
        /// </summary>
        private List<ISO3Country> _destinations;
        /// <summary>
        /// Current status of tracking
        /// </summary>
        private List<StatusTag> _tags;
        /// <summary>
        /// List of fields to include in the response. Fields to include: 
        /// title, orderId, tag, checkpoints, checkpointTime, message, countryName. 
        /// (Defaults: none, Example: title,orderId)
        /// </summary>
        private List<FieldTracking> _fields;


        public ParametersTracking()
        {
            Page = 1;
            Limit = 100;
        }

        public void AddSlug(string slug)
        {
            if (_slugs == null)
            {
                _slugs = new List<string> { slug };
            }
            else
            {
                _slugs.Add(slug);
            }
        }

        public void DeleteRequireField(string slug)
        {
            if (_slugs != null)
            {
                _slugs.Remove(slug);
            }
        }

        public void DeleteSlugs() => _slugs = null;

        public void AddOrigin(ISO3Country origin)
        {
            if (_origins == null)
            {
                _origins = new List<ISO3Country>
                {
                    origin
                };
            }
            else
            {
                _origins.Add(origin);
            }
        }

        public void DeleteOrigin(ISO3Country origin)
        {
            if (_origins != null)
            {
                _origins.Remove(origin);
            }
        }

        public void DeleteOrigins() => _origins = null;

        public void AddDestination(ISO3Country destination)
        {
            if (_destinations == null)
            {
                _destinations = new List<ISO3Country>
                {
                    destination
                };
            }
            else
            {
                _destinations.Add(destination);
            }
        }

        public void DeleteDestination(ISO3Country destination)
        {
            if (_destinations != null)
            {
                _destinations.Remove(destination);
            }
        }

        public void DeleteDestinations() => _destinations = null;

        public void AddTag(StatusTag tag)
        {
            if (_tags == null)
            {
                _tags = new List<StatusTag>
                {
                    tag
                };
            }
            else
            {
                _tags.Add(tag);
            }
        }

        public void DeleteTag(StatusTag tag)
        {
            if (_tags != null)
            {
                _tags.Remove(tag);
            }
        }

        public void DeleteTags() => _tags = null;

        public void AddField(FieldTracking field)
        {
            if (_fields == null)
            {
                _fields = new List<FieldTracking>
                {
                    field
                };
            }
            else
            {
                _fields.Add(field);
            }
        }

        public void DeleteField(FieldTracking field)
        {
            if (_fields != null)
            {
                _fields.Remove(field);
            }
        }

        public void DeleteFields() => _fields = null;

        /// <summary>
        /// Creates a QueryString with all the fields of this class different of Null
        /// </summary>
        /// <returns>String with the param codified in the QueryString</returns>
        public string GenerateQueryString()
        {

            var qs = new QueryString("page", Page.ToString());
            qs.Add("limit", Limit.ToString());

            if (Keyword != null)
            {
                qs.Add("keyword", Keyword);
            }

            if (CreatedAtMin != default(DateTime))
            {
                qs.Add("created_at_min", DateMethods.ToString(CreatedAtMin));
            }

            if (CreatedAtMax != default(DateTime))
            {
                qs.Add("created_at_max", DateMethods.ToString(CreatedAtMax));
            }

            if (Lang != null)
            {
                qs.Add("lang", Lang);
            }

            if (_slugs != null)
            {
                qs.Add("slug", _slugs);
            }

            if (_origins != null)
            {
                qs.Add("origin", string.Join(",", _origins));
            }

            if (_destinations != null)
            {
                qs.Add("destination", string.Join(",", _destinations));
            }

            if (_tags != null)
            {
                qs.Add("tag", string.Join(",", _tags));
            }

            if (_fields != null)
            {
                qs.Add("fields", string.Join(",", _fields));
            }

            //globalJSON.put("tracking", trackingJSON);

            return qs.GetQuery();
        }
    }
}

