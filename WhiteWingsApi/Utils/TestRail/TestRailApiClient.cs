// <copyright file="TestRailApiClient.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Utils.TestRail
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using WhiteWingsApi.Exceptions;

    public class TestRailApiClient
    {
        /// <summary>
        /// Gets or sets get/Set User.
        /// Returns/sets the user used for authenticating the API request.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets get/Set Password.
        /// Returns/sets the password used for authenticating the API requests.
        /// </summary>
        public string Password { get; set; }

        private readonly string url;

        /// <summary>
        /// Initializes a new instance of the <see cref="APIClient"/> class.
        /// </summary>
        public TestRailApiClient()
        {
            var baseUrl = Configs.TestrailUrl;
            User = Configs.TestrailUsername;
            Password = Configs.TestrailPassword;

            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }

            url = baseUrl + "index.php?/api/v2/";
        }

        /// <summary>
        /// Send Get
        /// Issues a GET request(read) against the API and returns the result
        /// (as JSON object, i.e.JObject or JArray instance).
        ///
        /// Arguments:
        ///
        /// uri The API method to call including parameters
        /// (e.g.get_case/1)
        /// </summary>
        /// <param name="uri">The Uri to sent a get request</param>
        /// <returns>The gotten object.</returns>
        public object SendGet(string uri)
        {
            return SendRequest("GET", uri, null);
        }

        /// <summary>
        /// Send POST
        ///
        /// Issues a POST request (write) against the API and returns the result
        /// (as JSON object, i.e. JObject or JArray instance).
        ///
        /// Arguments:
        /// </summary>
        /// <param name="uri">The API method to call including parameters (e.g. add_case/1)</param>
        /// <param name="data">The data to submit as part of the request
        /// (asserializable object, e.g. a dictionary)</param>
        /// <returns>The gotten object.</returns>
        public object SendPost(string uri, object data)
        {
            return SendRequest("POST", uri, data);
        }

        /// <summary>
        /// Sends an specific request.
        /// </summary>
        /// <param name="method">Request method.</param>
        /// <param name="uri">The uri wher to make the request.</param>
        /// <param name="data">The data that is used to make the request.</param>
        /// <returns>The response object.</returns>
        private object SendRequest(string method, string uri, object data)
        {
            var url = this.url + uri;

            // Create the request object and set the required HTTP method
            // (GET/POST) and headers (content type and basic auth).
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = method;

            var auth = Convert.ToBase64String(
                Encoding.ASCII.GetBytes(
                    $"{User}:{Password}"));

            request.Headers.Add("Authorization", "Basic " + auth);

            if (method == "POST")
            {
                // Add the POST arguments, if any. We just serialize the passed
                // data object (i.e. a dictionary) and then add it to the request
                // body.
                if (data != null)
                {
                    var block = Encoding.UTF8.GetBytes(
                        JsonConvert.SerializeObject(data));

                    request.GetRequestStream().Write(block, 0, block.Length);
                }
            }

            // Execute the actual web request (GET or POST) and record any
            // occurred errors.
            Exception ex = null;
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    throw;
                }

                response = (HttpWebResponse)e.Response;
                ex = e;
            }

            // Read the response body, if any, and deserialize it from JSON.
            var text = string.Empty;
            if (response != null)
            {
                var reader = new StreamReader(
                    response.GetResponseStream(),
                    Encoding.UTF8);

                using (reader)
                {
                    text = reader.ReadToEnd();
                }
            }

            JContainer result;
            if (text != string.Empty)
            {
                if (text.StartsWith("["))
                {
                    result = JArray.Parse(text);
                }
                else
                {
                    result = JObject.Parse(text);
                }
            }
            else
            {
                result = new JObject();
            }

            // Check for any occurred errors and add additional details to
            // the exception message, if any (e.g. the error message returned
            // by TestRail).
            if (ex != null)
            {
                var error = (string)result["error"];
                if (error != null)
                {
                    error = '"' + error + '"';
                }
                else
                {
                    error = "No additional error message received";
                }

                throw new TestRailApiException(
                    $"TestRail API returned HTTP {(int)response.StatusCode} ({error})");
            }

            return result;
        }
    }
}
