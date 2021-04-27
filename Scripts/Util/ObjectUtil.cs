using GameFramework.GameStructure.GameItems.ObjectModel;
using Newtonsoft.Json;

namespace GameFramework.GameStructure.Util
{
    public class ObjectUtil
    {
        public static T Clone<T>(T o)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(o));
        }

        public static string ExportGameItemMeta(GameItem gameItem)
        {
            return null;
        }
    }
}