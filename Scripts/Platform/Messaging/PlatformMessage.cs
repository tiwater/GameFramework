using System;
using System.Collections.Generic;
using GameFramework.Messaging;

namespace GameFramework.GameStructure.Platform.Messaging
{
    public class PlatformMessage : BaseMessage
    {

        public const string DEFAULT_MESSAGE_HEADER = "com.tiwater.karu.GAME_UPDATED";
        public const string TYPE = "MsgType";

        public const string TYPE_CREATION_UPDATED = "CreationUpdated";
        public const string TYPE_EQUIPMENT_UPDATED = "EquipmentUpdated";
        public const string TYPE_OPERATION = "Operation";

        public const string CONTENT_PGI = "PGI";

        public const string CONTENT_EQUIPMENT = "EQUIPMENT";

        public const string CONTENT_PGI_ID = "PGI_Id";
        public const string CONTENT_OPERATION = "Operation";

        public string Header;
        public Dictionary<string, object> Content = new Dictionary<string, object>();


        public PlatformMessage()
        {
        }

        public PlatformMessage(string header, Dictionary<string, object> content)
        {
            Header = header;
            Content = content;
        }


        public PlatformMessage(string header)
        {
            Header = header;
        }

        /// <summary>
        /// Put the given value into this message
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void PutContent(string key, object value)
        {
            if (Content == null)
            {
                Content = new Dictionary<string, object>();
            }
            Content[key] = value;
        }

        /// <summary>
        /// Get the specified content
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetContent(string key)
        {
            if(Content != null)
            {
                if (Content.ContainsKey(key))
                {
                    return Content[key];
                }
            }
            return null;
        }
    }
}