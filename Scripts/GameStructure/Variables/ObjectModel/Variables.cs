//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

using System;
using System.Collections.Generic;
using GameFramework.GameStructure.Util;
using GameFramework.Localisation.ObjectModel;
using GameFramework.Preferences;
using Newtonsoft.Json;
using UnityEngine;

namespace GameFramework.GameStructure.Variables.ObjectModel
{
    /// <summary>
    /// Holds a list of customisable variables
    /// </summary>
    [Serializable]
    public class Variables
    {
        public enum VariableType
        {
            Bool,
            Float,
            Int,
            String,
            Vector2,
            Vector3,
            Color
        }

        #region Editor Parameters

        /// <summary>
        /// An array of BoolVariables
        /// </summary>
        public List<BoolVariable> BoolVariables
        {
            get
            {
                return _boolVariables;
            }
            set
            {
                _boolVariables = value;
            }
        }
        [Tooltip("An array of BoolVariables.")]
        [SerializeField]
        List<BoolVariable> _boolVariables = new List<BoolVariable>();

        /// <summary>
        /// An array of IntVariables
        /// </summary>
        public List<IntVariable> IntVariables
        {
            get
            {
                return _intVariables;
            }
            set
            {
                _intVariables = value;
            }
        }
        [Tooltip("An array of IntVariables.")]
        [SerializeField]
        List<IntVariable> _intVariables = new List<IntVariable>();

        /// <summary>
        /// An array of FloatVariables
        /// </summary>
        public List<FloatVariable> FloatVariables
        {
            get
            {
                return _floatVariables;
            }
            set
            {
                _floatVariables = value;
            }
        }
        [Tooltip("An array of FloatVariables.")]
        [SerializeField]
        List<FloatVariable> _floatVariables = new List<FloatVariable>();

        /// <summary>
        /// An array of StringVariables
        /// </summary>
        public List<StringVariable> StringVariables
        {
            get
            {
                return _stringVariables;
            }
            set
            {
                _stringVariables = value;
            }
        }
        [Tooltip("An array of StringVariables.")]
        [SerializeField]
        List<StringVariable> _stringVariables = new List<StringVariable>();

        /// <summary>
        /// An array of Vector2Variables
        /// </summary>
        public List<Vector2Variable> Vector2Variables
        {
            get
            {
                return _Vector2Variables;
            }
            set
            {
                _Vector2Variables = value;
            }
        }
        [Tooltip("An array of Vector2Variables.")]
        [SerializeField]
        List<Vector2Variable> _Vector2Variables = new List<Vector2Variable>();

        /// <summary>
        /// An array of Vector3Variables
        /// </summary>
        public List<Vector3Variable> Vector3Variables
        {
            get
            {
                return _Vector3Variables;
            }
            set
            {
                _Vector3Variables = value;
            }
        }
        [Tooltip("An array of Vector3Variables.")]
        [SerializeField]
        List<Vector3Variable> _Vector3Variables = new List<Vector3Variable>();

        /// <summary>
        /// An array of ColorVariables
        /// </summary>
        public List<ColorVariable> ColorVariables
        {
            get
            {
                return _colorVariables;
            }
            set
            {
                _colorVariables = value;
            }
        }
        [Tooltip("An array of ColorVariables.")]
        [SerializeField]
        List<ColorVariable> _colorVariables = new List<ColorVariable>();

        #endregion Editor Parameters

        #region Load / Save
        /// <summary>
        /// Load saved values from preferences or set to default if not found.
        /// This method should only be called on the GameItem metas, shouldn't be called against the PlayerGameItem
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="useSecurePrefs"></param>
        public void Load(string prefix = "", bool? useSecurePrefs = null)
        {
            foreach (var variable in BoolVariables)
            {
                if (variable.PersistChanges)
                    variable.Value = PreferencesFactory.GetBool(prefix + variable.Tag, variable.DefaultValue, useSecurePrefs);
                else
                    variable.Value = variable.DefaultValue;
            }
            foreach (var variable in FloatVariables)
            {
                if (variable.PersistChanges)
                    variable.Value = PreferencesFactory.GetFloat(prefix + variable.Tag, variable.DefaultValue, useSecurePrefs);
                else
                    variable.Value = variable.DefaultValue;
            }
            foreach (var variable in IntVariables)
            {
                if (variable.PersistChanges)
                    variable.Value = PreferencesFactory.GetInt(prefix + variable.Tag, variable.DefaultValue, useSecurePrefs);
                else
                    variable.Value = variable.DefaultValue;
            }
            foreach (var variable in StringVariables)
            {
                if (variable.PersistChanges)
                    variable.Value = PreferencesFactory.GetString(prefix + variable.Tag, variable.DefaultValue, useSecurePrefs);
                else
                    variable.Value = variable.DefaultValue;
            }
            foreach (var variable in Vector2Variables)
            {
                if (variable.PersistChanges)
                    variable.Value = PreferencesFactory.GetVector2(prefix + variable.Tag, variable.DefaultValue, useSecurePrefs) ?? Vector2.zero;
                else
                    variable.Value = variable.DefaultValue;
            }
            foreach (var variable in Vector3Variables)
            {
                if (variable.PersistChanges)
                    variable.Value = PreferencesFactory.GetVector3(prefix + variable.Tag, variable.DefaultValue, useSecurePrefs) ?? Vector3.zero;
                else
                    variable.Value = variable.DefaultValue;
            }
            foreach (var variable in ColorVariables)
            {
                if (variable.PersistChanges)
                    variable.Value = PreferencesFactory.GetColor(prefix + variable.Tag, variable.DefaultValue, useSecurePrefs) ?? Color.white;
                else
                    variable.Value = variable.DefaultValue;
            }
        }

        /// <summary>
        /// Update PlayerPrefs with values that should be saved.
        /// This method should only be called on the GameItem metas, shouldn't be called against the PlayerGameItem
        /// </summary>
        /// Note: This does not call PreferencesFactory.Save()
        /// <param name="prefix"></param>
        /// <param name="useSecurePrefs"></param>
        public void UpdatePlayerPrefs(string prefix = "", bool? useSecurePrefs = null)
        {
            foreach (var variable in BoolVariables)
            {
                if (variable.PersistChanges)
                    PreferencesFactory.SetBool(prefix + variable.Tag, variable.Value, useSecurePrefs);
            }
            foreach (var variable in FloatVariables)
            {
                if (variable.PersistChanges)
                    PreferencesFactory.SetFloat(prefix + variable.Tag, variable.Value, useSecurePrefs);
            }
            foreach (var variable in IntVariables)
            {
                if (variable.PersistChanges)
                    PreferencesFactory.SetInt(prefix + variable.Tag, variable.Value, useSecurePrefs);
            }
            foreach (var variable in StringVariables)
            {
                if (variable.PersistChanges)
                    PreferencesFactory.SetString(prefix + variable.Tag, variable.Value, useSecurePrefs);
            }
            foreach (var variable in Vector2Variables)
            {
                if (variable.PersistChanges)
                    PreferencesFactory.SetVector2(prefix + variable.Tag, variable.Value, useSecurePrefs);
            }
            foreach (var variable in Vector3Variables)
            {
                if (variable.PersistChanges)
                    PreferencesFactory.SetVector3(prefix + variable.Tag, variable.Value, useSecurePrefs);
            }
            foreach (var variable in ColorVariables)
            {
                if (variable.PersistChanges)
                    PreferencesFactory.SetColor(prefix + variable.Tag, variable.Value, useSecurePrefs);
            }
        }
        #endregion Load / Save

        #region get
        /// <summary>
        /// Return a BoolVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public BoolVariable GetBool(string tag)
        {
            foreach (var variable in BoolVariables)
                if (variable.Tag == tag)
                    return variable;
            return null;
        }

        /// <summary>
        /// Return an FloatVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public FloatVariable GetFloat(string tag)
        {
            foreach (var variable in FloatVariables)
                if (variable.Tag == tag)
                    return variable;
            return null;
        }

        /// <summary>
        /// Return an IntVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public IntVariable GetInt(string tag)
        {
            foreach (var variable in IntVariables)
                if (variable.Tag == tag)
                    return variable;
            return null;
        }

        /// <summary>
        /// Return an StringVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public StringVariable GetString(string tag)
        {
            foreach (var variable in StringVariables)
                if (variable.Tag == tag)
                    return variable;
            return null;
        }

        /// <summary>
        /// Return an Vector2Variable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Vector2Variable GetVector2(string tag)
        {
            foreach (var variable in Vector2Variables)
                if (variable.Tag == tag)
                    return variable;
            return null;
        }

        /// <summary>
        /// Return an Vector3Variable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Vector3Variable GetVector3(string tag)
        {
            foreach (var variable in Vector3Variables)
                if (variable.Tag == tag)
                    return variable;
            return null;
        }

        /// <summary>
        /// Return a ColorVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public ColorVariable GetColor(string tag)
        {
            foreach (var variable in ColorVariables)
                if (variable.Tag == tag)
                    return variable;
            return null;
        }

        #endregion get

        /// <summary>
        /// Set a BoolVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetBool(string tag, bool value)
        {
            foreach (var variable in BoolVariables)
            {
                if (variable.Tag == tag)
                {
                    variable.Value = value;
                    return;
                }
            }
            //Not found
            var newVariable = new BoolVariable();
            newVariable.Tag = tag;
            newVariable.Value = value;
            BoolVariables.Add(newVariable);
        }

        /// <summary>
        /// Set an FloatVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetFloat(string tag, float value)
        {
            foreach (var variable in FloatVariables)
            {
                if (variable.Tag == tag)
                {
                    variable.Value = value;
                    return;
                }
            }
            //Not found
            var newVariable = new FloatVariable();
            newVariable.Tag = tag;
            newVariable.Value = value;
            FloatVariables.Add(newVariable);
        }

        /// <summary>
        /// Set an IntVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetInt(string tag, int value)
        {
            foreach (var variable in IntVariables)
            {
                if (variable.Tag == tag)
                {
                    variable.Value = value;
                    return;
                }
            }
            //Not found
            var newVariable = new IntVariable();
            newVariable.Tag = tag;
            newVariable.Value = value;
            IntVariables.Add(newVariable);
        }

        /// <summary>
        /// Set an StringVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetString(string tag, string value)
        {
            foreach (var variable in StringVariables)
            {
                if (variable.Tag == tag)
                {
                    variable.Value = value;
                    return;
                }
            }
            //Not found
            var newVariable = new StringVariable();
            newVariable.Tag = tag;
            newVariable.Value = value;
            StringVariables.Add(newVariable);
        }

        /// <summary>
        /// Set an Vector2Variable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetVector2(string tag, Vector2 value)
        {
            foreach (var variable in Vector2Variables)
            {
                if (variable.Tag == tag)
                {
                    variable.Value = value;
                    return;
                }
            }
            //Not found
            var newVariable = new Vector2Variable();
            newVariable.Tag = tag;
            newVariable.Value = value;
            Vector2Variables.Add(newVariable);
        }

        /// <summary>
        /// Set an Vector3Variable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetVector3(string tag, Vector3 value)
        {
            foreach (var variable in Vector3Variables)
            {
                if (variable.Tag == tag)
                {
                    variable.Value = value;
                    return;
                }
            }
            //Not found
            var newVariable = new Vector3Variable();
            newVariable.Tag = tag;
            newVariable.Value = value;
            Vector3Variables.Add(newVariable);
        }

        /// <summary>
        /// Set a ColorVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetColor(string tag, Color value)
        {
            foreach (var variable in ColorVariables)
            {
                if (variable.Tag == tag)
                {
                    variable.Value = value;
                    return;
                }
            }
            //Not found
            var newVariable = new ColorVariable();
            newVariable.Tag = tag;
            newVariable.Value = value;
            ColorVariables.Add(newVariable);
        }


        public string ToJsonMapString()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            GenerateVariableDictionary<BoolVariable, bool>(BoolVariables, dict);
            GenerateVariableDictionary<IntVariable, int>(IntVariables, dict);
            GenerateVariableDictionary<FloatVariable, float>(FloatVariables, dict);
            GenerateVariableDictionary<StringVariable, string>(StringVariables, dict);
            GenerateVariableDictionary<Vector2Variable, Vector2>(Vector2Variables, dict);
            GenerateVariableDictionary<Vector3Variable, Vector3>(Vector3Variables, dict);
            GenerateVariableDictionary<ColorVariable, Color>(ColorVariables, dict);

            return JsonConvert.SerializeObject(dict);
        }

        private void GenerateVariableDictionary<T, K>(List<T> variables, Dictionary<string, string> dict) where T : Variable<K>
        {
            string keyPrefix = typeof(T).Name + "_";
            foreach(var variable in variables)
            {
                dict[keyPrefix + variable.Tag] = JsonUtility.ToJson(variable);
            }
        }

        public static Variables FromDict(IDictionary<string, string> dict)
        {
            Variables variables = new Variables();
            foreach (var pair in dict)
            {
                string type = pair.Key.Substring(0, pair.Key.IndexOf('_'));
                if (type == typeof(BoolVariable).Name)
                {
                    variables.BoolVariables.Add(FromString<BoolVariable>(pair.Value));
                }
                else if (type == typeof(FloatVariable).Name)
                {
                    variables.FloatVariables.Add(FromString<FloatVariable>(pair.Value));
                }
                else if (type == typeof(IntVariable).Name)
                {
                    variables.IntVariables.Add(FromString<IntVariable>(pair.Value));
                }
                else if (type == typeof(StringVariable).Name)
                {
                    variables.StringVariables.Add(FromString<StringVariable>(pair.Value));
                }
                else if (type == typeof(Vector2Variable).Name)
                {
                    variables.Vector2Variables.Add(FromString<Vector2Variable>(pair.Value));
                }
                else if (type == typeof(Vector3Variable).Name)
                {
                    variables.Vector3Variables.Add(FromString<Vector3Variable>(pair.Value));
                }
                else if (type == typeof(ColorVariable).Name)
                {
                    variables.ColorVariables.Add(FromString<ColorVariable>(pair.Value));
                }
            }
            return variables;
        }

        public static Variables FromJsonMapString(string json)
        {
            return FromDict(JsonConvert.DeserializeObject<Dictionary<string, string>>(json));
        }

        private static T FromString<T>(string str)
        {
            T t = JsonUtility.FromJson<T>(str);
            return t;
        }


        public List<string> CompareWith(Variables that)
        {
            List<string> differents = new List<string>();
            //Compare each type variable list
            if(CompareVariableLists<BoolVariable, bool>(this.BoolVariables, that.BoolVariables) != 0)
            {
                differents.Add("BoolVariable");
            }
            if (CompareVariableLists<IntVariable, int>(this.IntVariables, that.IntVariables) != 0)
            {
                differents.Add("IntVariable");
            }
            if (CompareVariableLists<FloatVariable, float>(this.FloatVariables, that.FloatVariables) != 0)
            {
                differents.Add("FloatVariable");
            }
            if (CompareVariableLists<StringVariable, string>(this.StringVariables, that.StringVariables) != 0)
            {
                differents.Add("StringVariable");
            }
            if (CompareVariableLists<Vector2Variable, Vector2>(this.Vector2Variables, that.Vector2Variables) != 0)
            {
                differents.Add("Vector2Variable");
            }
            if (CompareVariableLists<Vector3Variable, Vector3>(this.Vector3Variables, that.Vector3Variables) != 0)
            {
                differents.Add("Vector3Variable");
            }
            if (CompareVariableLists<ColorVariable, Color>(this.ColorVariables, that.ColorVariables) != 0)
            {
                differents.Add("ColorVariable");
            }
            return differents;
        }

        private int CompareVariableLists<T, M>(List<T> thisChildren, List<T> thatChildren) where T : Variable<M>
        {
            //Check whether the List is empty
            if (thisChildren == null || thisChildren.Count == 0)
            {
                if (thatChildren == null || thatChildren.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            if (thatChildren == null || thatChildren.Count == 0)
            {
                return 1;
            }
            //Check the list size
            if (thisChildren.Count > thatChildren.Count)
            {
                return 1;
            }
            else if (thisChildren.Count < thatChildren.Count)
            {
                return -1;
            }
            //Sort the list to ease the compare
            thisChildren.Sort((x, y) => x.Tag.CompareTo(y.Tag));
            thatChildren.Sort((x, y) => x.Tag.CompareTo(y.Tag));
            for (int i = 0; i < thisChildren.Count; i++)
            {
                List<string> differents = ObjectUtil.CompareObjects(thisChildren[i], thatChildren[i]);
                if(differents!=null && differents.Count > 0)
                {
                    //Found different
                    //But cannot describe the bigger or smaller in the list, always return 1 for different
                    return 1;
                }
            }
            //No difference
            return 0;

        }
    }


    [Serializable]
    public class Variable<T>
    {
        #region Editor Parameters


        /// <summary>
        /// A unique tag that represents this variable
        /// </summary>
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }
        [Tooltip("A unique tag that represents this variable")]
        [SerializeField]
        string _tag;

        /// <summary>
        /// A LocalisableText that holds a name for this variable that can be used for display purposes
        /// </summary>
        public LocalisableText Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        [Tooltip("A LocalisableText that holds a name for this variable that can be used for display purposes")]
        [SerializeField]
        LocalisableText _name;

        /// <summary>
        /// An optional category that this item belongs to
        /// </summary>
        /// Can be used for grouping items
        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
            }
        }
        [Tooltip("An optional category that this item belongs to. Can be used for grouping items")]
        [SerializeField]
        string _category;

        /// <summary>
        /// A default value for this variable
        /// </summary>
        /// This is also reflected through the Value property which can also be updated 
        public T DefaultValue
        {
            get
            {
                return _defaultValue;}
            set
            {
                _defaultValue = value;
            }
        }
        [Tooltip("A default value for this variable")]
        [SerializeField]
        T _defaultValue;

        /// <summary>
        /// Whether changes should be saved across game sessions
        /// </summary>
        public bool PersistChanges
        {
            get
            {
                return _persistChanges;
            }
            set
            {
                _persistChanges = value;
            }
        }
        [Tooltip("Whether runtime changes should be saved across game sessions.")]
        [SerializeField]
        bool _persistChanges;

        #endregion Editor Parameters

        /// <summary>
        /// The current value of this item
        /// </summary>
        [SerializeField]
        public T Value;

    }

    [Serializable]
    public class BoolVariable : Variable<bool>
    {
        #region Editor Parameters

        #endregion Editor Parameters
    }

    [Serializable]
    public class IntVariable : Variable<int>
    {
        #region Editor Parameters

        #endregion Editor Parameters
    }

    [Serializable]
    public class FloatVariable : Variable<float>
    {
        #region Editor Parameters

        #endregion Editor Parameters
    }

    [Serializable]
    public class StringVariable : Variable<string>
    {
        #region Editor Parameters

        #endregion Editor Parameters
    }

    [Serializable]
    public class Vector2Variable : Variable<Vector2>
    {
        #region Editor Parameters

        #endregion Editor Parameters
    }

    [Serializable]
    public class Vector3Variable : Variable<Vector3>
    {
        #region Editor Parameters

        #endregion Editor Parameters
    }

    [Serializable]
    public class ColorVariable : Variable<Color>
    {
        #region Editor Parameters

        #endregion Editor Parameters
    }
}