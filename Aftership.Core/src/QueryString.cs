using System;
using System.Collections.Generic;

namespace AftershipAPI
{

    /// <summary>
    /// Creates a url friendly String
    /// </summary>
    public class QueryString
    {
        private string query = "";

        public QueryString() { }

        public QueryString(string name, string value)
        {
            Encode(name, value);
        }

        public void Add(string name, List<string> list)
        {
            AppendAmpersandToQuery();

            var value = string.Join(",", list.ToArray());
            Encode(name, value);
        }

        public void Add(string name, string value)
        {
            AppendAmpersandToQuery();
            Encode(name, value);
        }

        private void AppendAmpersandToQuery()
        {
            // Don't append an '&' char if we don't have any query params yet
            // eg. when adding the first parameter from the empty ctor
            if (!string.IsNullOrWhiteSpace(query))
            { query += "&"; }
        }

        private void Encode(string name, string value)
        {
            query += System.Uri.EscapeDataString(name);
            query += "=";
            query += System.Uri.EscapeDataString(value);
        }

        public string GetQuery() => query;

        public override string ToString() => GetQuery();
    }
}

