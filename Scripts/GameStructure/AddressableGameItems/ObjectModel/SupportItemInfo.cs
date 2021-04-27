using System;
using System.Collections.Generic;
using static GameFramework.GameStructure.Model.AddressableGameItemMeta;

namespace GameFramework.GameStructure.AddressableGameItems.ObjectModel
{
    [Serializable]
    public class SupportItemInfo
    {
        public string GiType;
        public ContentType ContentType;
        public List<string> GiIds;
    }
}