using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace GameFramework.GameStructure.Util
{
    /// <summary>
    /// To update the percentage of the downloading with a slider
    /// </summary>
    class FileDownloadPercentageHandler : DownloadHandlerScript
    {
        private int contentLength;
        private int receivedLength = 0;
        private Slider slider;
        private List<byte> receivedData = new List<byte>();

        // Standard scripted download handler - allocates memory on each ReceiveData callback
        public FileDownloadPercentageHandler(Slider slider) : base()
        {
            this.slider = slider;
            UpdateSlider(0);
        }

        // Pre-allocated scripted download handler
        // reuses the supplied byte array to deliver data.
        // Eliminates memory allocation.

        public FileDownloadPercentageHandler(byte[] buffer, Slider slider) : base(buffer)
        {
            this.slider = slider;
            UpdateSlider(0);
        }

        // Required by DownloadHandler base class. Called when you address the 'bytes' property.
        protected override byte[] GetData() { return receivedData.ToArray(); }

        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            if (data == null || data.Length < 1)
            {
                return false;
            }
            receivedLength += dataLength;
            if (data.Length == dataLength)
            {
                receivedData.AddRange(data);
            }
            else
            {
                receivedData.AddRange(data.ToList().GetRange(0, dataLength));
            }
            UpdateSlider(GetProgress());
            return true;
        }

        // Called when all data has been received from the server and delivered via ReceiveData.

        protected override void CompleteContent()
        {
            UpdateSlider(GetProgress());
        }

        // Called when a Content-Length header is received from the server.

        protected override void ReceiveContentLength(int contentLength)
        {
            this.contentLength = contentLength;
        }

        protected override float GetProgress()
        {
            if (contentLength > 0)
            {
                return 1.0f * receivedLength / contentLength;
            }
            else if (receivedLength == 0)
            {
                return 0;
            }
            else
            {
                return 1 - 1.0f / receivedLength;
            }
        }

        private void UpdateSlider(float value)
        {
            slider.enabled = true;
            slider.value = value;
            slider.enabled = false;
        }
    }
}