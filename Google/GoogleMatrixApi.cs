using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WpfApplication1.Google
{
    /// <summary>
    /// The Google Matrix API client.
    /// </summary>
    public class GoogleMatrixApiClient
    {
        private const string requestUrlTemplate = "https://maps.googleapis.com/maps/api/distancematrix/json?origins={0}&destinations={1}&key={2}";
        private string key;


        /// <summary>
        /// Instantiates the <see cref="GoogleMatrixApiClient"/>.
        /// </summary>
        /// <param name="key"></param>
        public GoogleMatrixApiClient(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentOutOfRangeException("key", "Value can't be null or empty string.");

            // Trim the given key
            this.key = key.Trim();
        }



        /// <summary>
        /// Requests the Google Matrix APi for new data.
        /// </summary>
        /// <param name="origins">The origins points.</param>
        /// <param name="destinations">The destination points.</param>
        /// <returns>
        /// Returns collection of duration values (in seconds) between given points.
        /// First dimension contains origins in order as given.
        /// Second dimension contains duration value from origin to destination in order as given.
        /// </returns>
        public async Task<IEnumerable<IEnumerable<int>>> RequestMatrix(IEnumerable<string> origins, IEnumerable<string> destinations)
        {
            using (var client = new HttpClient())
            {
                // Construct the request URL
                var requestUrl = string.Format(requestUrlTemplate,
                    origins.Aggregate(string.Empty, (c, s) => c + (c == string.Empty ? string.Empty : "|") + s),
                    destinations.Aggregate(string.Empty, (c, s) => c + (c == string.Empty ? string.Empty : "|") + s),
                    this.key);

                // Send the request and deserialize the response
                var response = await client.GetAsync(requestUrl);
                var responseJson = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<GoogleResponse>(responseJson);

                // Return only duration values
                return data.rows.Select(r => r.elements.Select(e => e.duration.value));
            }
        }

        private class GoogleResponse
        {
            public string[] destination_addresses { get; set; }

            public string[] origin_addresses { get; set; }

            public GoogleResponseRow[] rows { get; set; }
        }
        
        private class GoogleResponseRow
        {
            public GoogleResponseElement[] elements { get; set; }
        }

        private class GoogleResponseElement
        {
            public GoogleResponseElementDistance distance { get; set; }

            public GoogleResponseElementDuration duration { get; set; }

            public string status { get; set; }
        }

        private class GoogleResponseElementDistance
        {
            public string text { get; set; }

            public int value { get; set; }
        }

        private class GoogleResponseElementDuration
        {
            public string text { get; set; }

            public int value { get; set; }
        }
    }
}
