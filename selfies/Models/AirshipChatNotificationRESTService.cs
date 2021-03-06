﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace selfies.Models
{
    public class AirshipChatNotificationRESTService
    {

        readonly string uri = "https://go.urbanairship.com/api/push/";

        public async Task<AirshipResponse> SendChat(string handle_public_key, string alert_message, string chat_message, string group_key, string from_handle_public_key, int lmessageId)
        {

            AirshipResponse arr = new AirshipResponse();

            string username = System.Web.Configuration.WebConfigurationManager.AppSettings["UAUser"];
            string password = System.Web.Configuration.WebConfigurationManager.AppSettings["UAPassword"];

            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);

            var byteArray = Encoding.ASCII.GetBytes(username + ":" + password);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("basic", Convert.ToBase64String(byteArray));


            // ...
            AirshipNotification note = new AirshipNotification
            {
                audience = new AirshipNotification.AirshipAudience
                {
                    alias = handle_public_key
                },
                notification = new AirshipNotification.AirshipNotificationPayload
                {
                    alert = alert_message,
                    ios = new AirshipNotification.AirshipNotificationPayload.AirshipIos {
                        extra = new AirshipNotification.AirshipNotificationPayload.AirshipIos.AirshipThreadKey
                        {
                            threadKey = group_key,
                            fromKey = from_handle_public_key,
                            messageId = lmessageId,
                            message = chat_message
                        },
                        priority = 5
                    }
                },
                device_types = new List<string>() { "ios" }
            };

            string js = JsonConvert.SerializeObject(note);

            System.Diagnostics.Debug.Print(js);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/vnd.urbanairship+json; version=3");            

            StringContent sc =  new StringContent(js, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri,sc);

            response.EnsureSuccessStatusCode();

            arr.result = 1;
            arr.message = "Success";

            return arr;

        }

        private AuthenticationHeaderValue CreateBasicAuthenticationHeader(string username, string password)
        {
            return new AuthenticationHeaderValue(
            "Basic",
            System.Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(
            string.Format("{0}:{1}", username, password)))
            );
        }

    }
}