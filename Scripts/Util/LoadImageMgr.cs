using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace GameFramework.GameStructure.Util
{
    public class LoadImageMgr
    {
        /// <summary>
        /// download image from web or hard disk
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Texture2D> LoadImage(string url)
        {
            Texture2D texture = null;

            //Load from memory first
            if (imageDic.TryGetValue(url, out texture))
            {
                return texture;
            }
            string savePath = GetLocalPath();
            string filePath = string.Format("file://{0}/{1}.png", savePath, SecurityUtil.Md5Sum(url));
            //Load from hard disk
            if (Directory.Exists(filePath))
            {
                texture = await DownloadImage(filePath);
            }
            //load from web
            if (texture == null)
            {
                texture = await DownloadImage(url);
                if (texture != null)
                {
                    //Save to hard disk
                    Save2LocalPath(url, texture);
                }
            }
            if (texture != null && !imageDic.ContainsKey(url))
            {
                //Put to cache
                imageDic.Add(url, texture);
            }
            return texture;

        }


        /// <summary>
        /// download image from web or hard disk
        /// </summary>
        /// <param name="url"></param>
        /// <param name="loadEnd">callback</param>
        /// <returns></returns>
        public IEnumerator LoadImage(string url, Action<Texture2D> loadEnd)
        {
            Texture2D texture = null;
            //Load from memory first
            if (imageDic.TryGetValue(url, out texture))
            {
                loadEnd.Invoke(texture);
                yield break;
            }
            string savePath = GetLocalPath();
            string filePath = string.Format("file://{0}/{1}.png", savePath, SecurityUtil.Md5Sum(url));
            //from hard disk
            bool hasLoad = false;
            if (Directory.Exists(filePath))
                yield return DownloadImage(filePath, (state, localTexture) =>
                {
                    hasLoad = state;
                    if (state)
                    {
                        loadEnd.Invoke(localTexture);
                        if (!imageDic.ContainsKey(url))
                            imageDic.Add(url, localTexture);
                    }
                });
            if (hasLoad) yield break;
            //load from web
            yield return DownloadImage(url, (state, downloadTexture) =>
            {
                hasLoad = state;
                if (state)
                {
                    loadEnd.Invoke(downloadTexture);
                    if (!imageDic.ContainsKey(url))
                        imageDic.Add(url, downloadTexture);
                    Save2LocalPath(url, downloadTexture);
                }
            });
        }

        /// <summary>
        /// Download image from url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Texture2D> DownloadImage(string url)
        {
            using (UnityWebRequest request = new UnityWebRequest(url))
            {
                Texture2D localTexture = null;
                DownloadHandlerTexture downloadHandlerTexture = new DownloadHandlerTexture(true);
                request.downloadHandler = downloadHandlerTexture;
                try
                {
                    await request.SendWebRequest();
                    //if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError(request.error);
                    }
                    else
                    {
                        localTexture = downloadHandlerTexture.texture;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
                return localTexture;
            }
        }

        /// <summary>
        /// Download image from url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="downloadEnd"></param>
        /// <returns></returns>
        public IEnumerator DownloadImage(string url, Action<bool, Texture2D> downloadEnd)
        {
            using (UnityWebRequest request = new UnityWebRequest(url))
            {
                DownloadHandlerTexture downloadHandlerTexture = new DownloadHandlerTexture(true);
                request.downloadHandler = downloadHandlerTexture;
                yield return request.Send();
                if (string.IsNullOrEmpty(request.error))
                {
                    Texture2D localTexture = downloadHandlerTexture.texture;
                    downloadEnd.Invoke(true, localTexture);
                }
                else
                {
                    downloadEnd.Invoke(false, null);
                    Debug.Log(request.error);
                }
            }
        }
        /// <summary>
        /// save the picture
        /// </summary>
        /// <param name="url"></param>
        /// <param name="texture"></param>
        private void Save2LocalPath(string url, Texture2D texture)
        {
            byte[] bytes = texture.EncodeToPNG();
            string savePath = GetLocalPath();
            try
            {
                File.WriteAllBytes(string.Format("{0}/{1}.png", savePath, SecurityUtil.Md5Sum(url)), bytes);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
            }
        }
        /// <summary>
        /// get which path will save
        /// </summary>
        /// <returns></returns>
        private string GetLocalPath()
        {
            string savePath = Application.persistentDataPath + "/pics";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            return savePath;
        }
        private Dictionary<string, Texture2D> imageDic = new Dictionary<string, Texture2D>();
        public static LoadImageMgr Instance { get; private set; } = new LoadImageMgr();
    }
}