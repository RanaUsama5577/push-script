using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using pushscript.Models;
using System.Web.Mvc;
using Google.Cloud.Firestore;

namespace pushscript.Controllers
{
    public class SubscribersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public async Task<string> SendMessageToClient(string fcm_token, string message, string title,string image1,string image2,string url)
        {
            string content = string.Empty;
            HttpClient httpClient = null;
            HttpResponseMessage response = null;
            var id = Guid.NewGuid().ToString().Substring(0, 4);
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var doc_id = unixTimestamp.ToString() + id;
            try
            {
                // Get this from your Firebase Developer Console Login  
                string serverApiKey = "AAAAqNq_sx4:APA91bE5klpC1lXOJHspPCQREkCkodp4eIBnuITQsyM3B8avp-cQYjwfbl9IU50soyUMfCNkiLjm1zreSUxAl-f3DDjFQtdzmx5QZHLCTWE2a7HWk0izKVBDWcFTyRCy9xPlGVH2OOYr";
                string apiEndpoint = "https://fcm.googleapis.com/fcm/send";
                using (httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", (string.Format("key={0}", serverApiKey)));
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    var dynamicPostData = new
                    {
                        to = fcm_token,
                        data = new
                        {
                            sound = "default",
                            body = message,
                            title = title,
                            content_available = true,
                            priority = "high",
                            icon = image1,
                            image = image2,
                            onclick = url,
                            notification_id = doc_id
                        },
                    };
                    response = httpClient.PostAsJsonAsync(new Uri(apiEndpoint), dynamicPostData).Result;

                    content = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();

                    FCMGroupResponse resp = JsonConvert.DeserializeObject<FCMGroupResponse>(content);
                    return doc_id;
                }
            }
            catch (Exception ex)
            {
                return "false";
            }
        }

        [HttpPost]
        public async Task<string> SendMessageToClient2(string fcm_tokens, string message, string title, string image1, string image2,string url)
        {
            string content = string.Empty;
            HttpClient httpClient = null;
            HttpResponseMessage response = null;

            var id = Guid.NewGuid().ToString().Substring(0, 4);
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var doc_id = unixTimestamp.ToString() + id;
            try
            {
                var temp = fcm_tokens.Split(',');
                List<string> vs = new List<string>();
                foreach(var i in temp)
                {
                    vs.Add(i);
                }
                // Get this from your Firebase Developer Console Login  
                string serverApiKey = "AAAAqNq_sx4:APA91bE5klpC1lXOJHspPCQREkCkodp4eIBnuITQsyM3B8avp-cQYjwfbl9IU50soyUMfCNkiLjm1zreSUxAl-f3DDjFQtdzmx5QZHLCTWE2a7HWk0izKVBDWcFTyRCy9xPlGVH2OOYr";
                string apiEndpoint = "https://fcm.googleapis.com/fcm/send";
                using (httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", (string.Format("key={0}", serverApiKey)));
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    var dynamicPostData = new
                    {
                        registration_ids = vs,
                        data = new
                        {
                            sound = "default",
                            body = message,
                            title = title,
                            content_available = true,
                            priority = "high",
                            icon = image1,
                            image = image2,
                            onclick = url,
                            notification_id = doc_id
                        }
                    };
                    response = httpClient.PostAsJsonAsync(new Uri(apiEndpoint), dynamicPostData).Result;

                    content = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();

                    FCMGroupResponse resp = JsonConvert.DeserializeObject<FCMGroupResponse>(content);
                    return doc_id;
                }
            }
            catch (Exception ex)
            {
                return "false";
            }
        }

        public async Task<JsonResult> UpdateNotification(string Id)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"usama-push-firebase-adminsdk-9nwv8-2cf1f4a92a.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            FirestoreDb db = FirestoreDb.Create("usama-push");
            try
            {
                var clicks = 0;
                DocumentReference documentReference1 = db.Collection("notifications").Document(Id);
                DocumentSnapshot snapshot2 = await documentReference1.GetSnapshotAsync();
                if (snapshot2.Exists)
                {
                    Dictionary<string, object> gig_users = snapshot2.ToDictionary();
                    foreach (KeyValuePair<string, object> pair in gig_users)
                    {
                        if (pair.Key == "total_likes")
                        {
                            clicks = Convert.ToInt32(pair.Value.ToString());
                        }
                    }
                }
                clicks++;
                DocumentReference documentReference2 = db.Collection("notifications").Document(Id);
                Dictionary<string, object> counter = new Dictionary<string, object>
                    {
                        { "total_clicks",  clicks },
                    };
                await documentReference2.UpdateAsync(counter);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            
        }

    }
}