using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Networking;

namespace GameFramework.GameStructure.Util
{
    interface IFileUpDownloadUtil
    {
        /// <summary>
        /// Upload file to the remote path
        /// </summary>
        /// <param name="localPath">full path of local resource</param>
        /// <param name="uploadPath">without the upload server's domain, just the relative resource uri</param>
        /// <param name="processor"></param>
        /// <returns></returns>
        IEnumerator UploadFile(string localPath, string uploadPath, HttpUtil.RequestProcess processor);

        /// <summary>
        /// Download the remote file
        /// </summary>
        /// <param name="remotePath">The full URL for file to download</param>
        /// <param name="localPath">Local path for the file to save</param>
        /// <param name="processor">Callback when the download is done</param>
        /// <returns></returns>
        IEnumerator DownloadFile(string remotePath, string localPath, HttpUtil.RequestProcess processor);

        /// <summary>
        /// Download the remote file
        /// </summary>
        /// <param name="remotePath">The full URL for file to download</param>
        /// <param name="localPath">Local path for the file to save</param>
        /// <param name="downloadhandler">Callback then then download is proecssing</param>
        /// <param name="processor">Callback when the download is done</param>
        /// <returns></returns>
        IEnumerator DownloadFile(string remotePath, string localPath, DownloadHandler downloadhandler, HttpUtil.RequestProcess processor);

        string GetUploadPrefix();
        string GetDownloadPrefix();
    }
}