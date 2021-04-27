using System;
using System.Collections.Generic;

public class StringUtil
{


    /// <summary>
    /// Check whether the List has valid data
    /// </summary>
    /// <param name="strList"></param>
    /// <returns></returns>
    public static bool IsListEmpty(List<string> strList)
    {

        bool isEmpty = true;
        if (strList != null)
        {
            foreach (var name in strList)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    return false;
                }
            }
        }
        return isEmpty;
    }
}
