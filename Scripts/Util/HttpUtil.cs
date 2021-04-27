using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework.GameStructure.Util
{
    /// <summary>
    /// Utitlity for Http Processing
    /// </summary>
    public class HttpUtil
    {

        public const int RESP_OK = 2;
        public const int RESP_NOT_FOUND = 4;
        public const int RESP_INTER_ERR = 5;

        //The callback when request is done
        public delegate void RequestProcess(UnityWebRequest www);

        /// <summary>
        /// Return the first number of the response code as the response type
        /// </summary>
        /// <param name="respondCode"></param>
        /// <returns>2 - OK; 4 - Not found; 5 - Internal Error</returns>
        public static int ResponseType(long respondCode)
        {
            return (int)respondCode / 100;
        }

        public static bool IsNormalResponse(long respondCode)
        {
            return ResponseType(respondCode) == RESP_OK;
        }

        public static IEnumerator Post(string api, string data, RequestProcess processor)
        {

            //Option 2
            UnityWebRequest webRequest = new UnityWebRequest(api, "POST");
            if (data != null)
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            SetHeader(webRequest);
            yield return webRequest.SendWebRequest();
            HandleWebResponse(webRequest, processor);
        }

        public static IEnumerator Get(string api, RequestProcess processor)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(api);
            SetHeader(webRequest);
            yield return webRequest.SendWebRequest();
            HandleWebResponse(webRequest, processor);
        }


        public static IEnumerator GetDummy(string api, RequestProcess processor)
        {
            yield return null;
            if (processor != null)
            {
                //Callback
                processor(null);
            }
        }

        public static async Task<T> PostAsync<T>(string api, string data)
        {

            UnityWebRequest webRequest = new UnityWebRequest(api, "POST");
            if (data != null)
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            SetHeader(webRequest);
            await webRequest.SendWebRequest();

            return HandleWebResponse<T>(webRequest);
        }

        public static async Task<T> GetAsync<T>(string api)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(api);
            SetHeader(webRequest);
            await webRequest.SendWebRequest();
            return HandleWebResponse<T>(webRequest);
        }


        public static async Task<T> GetDummyAsync<T>(string api)
        {
            await Task.Yield();
            T o = default(T);
            return o;
        }

        private static void HandleWebResponse(UnityWebRequest webRequest, RequestProcess processor)
        {
            if (webRequest == null || !IsNormalResponse(webRequest.responseCode))
            {
                //TODO: error handling
            }
            else if (processor != null)
            {
                //Callback
                processor(webRequest);
            }
        }

        private static T HandleWebResponse<T>(UnityWebRequest webRequest)
        {


            T o;

            if (webRequest == null || !IsNormalResponse(webRequest.responseCode))
            {
                //TODO: error handling
                o = default(T);
            }
            else
            {
                //Callback
                o = JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
            }
            return o;
        }

        /// <summary>
        /// Set Http header which includes token
        /// </summary>
        /// <returns></returns>
        public static void SetHeader(UnityWebRequest webRequest)
        {
            //TODO: For the logged in uer, put Token into header
            if (true)
            {
                webRequest.SetRequestHeader("NToken", "token");
                webRequest.SetRequestHeader("UserId", "userId");
            }
            webRequest.SetRequestHeader("Content-Type", "text/json");
        }


        public static async Task<UnityWebRequest> GetDummyAsync(string api)
        {
            await Task.Yield();
            UnityWebRequest webRequest = new UnityWebRequest("Dummy", "POST");
            var opr = webRequest.SendWebRequest();
            await opr;
            return webRequest;
        }
    }
}