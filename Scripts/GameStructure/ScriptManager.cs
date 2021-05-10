using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.Service;
using UnityEngine;

namespace GameFramework.GameStructure
{
    /// <summary>
    /// The class to manager addressable scripts
    /// </summary>
    public class ScriptManager
    {
        private Dictionary<string, string> Scripts = new Dictionary<string, string>();

        public async Task PreloadScripts()
        {
            var scripts = await AddressableResService.GetInstance().LoadResourcesAsync<TextAsset>("Script");
            foreach(var script in scripts)
            {
                Scripts[script.name] = script.text;
            }
        }

        public string getScript(string name)
        {
            string script;
            Scripts.TryGetValue(name, out script);
            return script;
        }

        public bool ContainsScript(string name)
        {
            return Scripts.ContainsKey(name);
        }

        public void Clear()
        {
            Scripts.Clear();
        }
    }
}