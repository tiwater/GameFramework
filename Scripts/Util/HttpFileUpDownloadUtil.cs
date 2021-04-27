using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework.GameStructure.Util
{
    namespace Assets.Scripts.Util
    {
        class HttpFileUpDownloadUtil : IFileUpDownloadUtil
        {
            public IEnumerator DownloadFile(string remotePath, string localPath, HttpUtil.RequestProcess processor)
            {
                yield return DownloadFile(remotePath, localPath, null, processor);
            }

            public IEnumerator DownloadFile(string remotePath, string localPath, DownloadHandler downloadhandler, HttpUtil.RequestProcess processor)
            {
                //Get the file
                UnityWebRequest request = UnityWebRequest.Get(remotePath);
                if (downloadhandler != null)
                {
                    request.downloadHandler = downloadhandler;
                }
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    // Retrieve results as binary data
                    byte[] data = request.downloadHandler.data;

                    //Save to disk
                    File.WriteAllBytes(localPath, data);
                }
                if (processor != null)
                {
                    processor.Invoke(request);
                }
            }

            public string GetDownloadPrefix()
            {
                throw new NotImplementedException();
            }

            public string GetUploadPrefix()
            {
                throw new NotImplementedException();
            }

            public IEnumerator UploadFile(string localPath, string uploadPath, HttpUtil.RequestProcess processor)
            {
                throw new NotImplementedException();
            }
        }
    }
}