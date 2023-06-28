using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace SLIDDES.StateMachines
{
    /* Create an inspector reference to a MonoScript (A regular C# script) 
     * <example>
     * 
     * [MonoScript]
     * public string scriptName;
     * 
     * 'Script' script = (Script)Activator.CreateInstance(Type.GetType(scriptName));
     * 
     * </example>
     * <credit>
     * Bunny83
     * </credit>
     */

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class MonoScriptAttribute : PropertyAttribute
    {
        /// <summary>
        /// The type of the monoscript
        /// </summary>
        public System.Type type;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(MonoScriptAttribute), false)]
    public class MonoScriptPropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Contains a reference to all Monoscripts
        /// </summary>
        private static Dictionary<string, MonoScript> scriptsCache;

        private bool viewMonoScriptString;
        private Color previousColor;

        static MonoScriptPropertyDrawer()
        {
            scriptsCache = new Dictionary<string, MonoScript>();
            // Add scripts to cache
            MonoScript[] monoScripts = Resources.FindObjectsOfTypeAll<MonoScript>();
            for(int i = 0; i < monoScripts.Length; i++)
            {
                Type type = monoScripts[i].GetClass();
                if(type != null && !scriptsCache.ContainsKey(type.FullName))
                {
                    scriptsCache.Add(type.FullName, monoScripts[i]);
                }
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(property.propertyType == SerializedPropertyType.String)
            {
                Rect rect = EditorGUI.PrefixLabel(position, label);
                Rect labelRect = position;
                labelRect.xMax = rect.xMin;
                position = rect;

                viewMonoScriptString = GUI.Toggle(labelRect, viewMonoScriptString, "", "label");
                if(viewMonoScriptString)
                {
                    property.stringValue = EditorGUI.TextField(position, property.stringValue);
                    return;
                }

                MonoScript monoScript = null;
                string typeName = property.stringValue;
                if(!string.IsNullOrEmpty(typeName))
                {
                    scriptsCache.TryGetValue(typeName, out monoScript);
                    if(monoScript == null)
                    {
                        previousColor = GUI.color;
                        GUI.color = Color.red;
                    }
                }

                monoScript = (MonoScript)EditorGUI.ObjectField(position, monoScript, typeof(MonoScript), false);
                if(monoScript == null && !string.IsNullOrEmpty(typeName)) GUI.color = previousColor;

                if(GUI.changed)
                {
                    if(monoScript != null)
                    {
                        Type type = monoScript.GetClass();
                        MonoScriptAttribute monoScriptAttribute = (MonoScriptAttribute)attribute;
                        if(monoScriptAttribute.type != null && !monoScriptAttribute.type.IsAssignableFrom(type))
                        {
                            type = null;
                        }

                        if(type != null)
                        {
                            property.stringValue = type.FullName;
                        }
                        else
                        {
                            Debug.LogWarning($"[MonoScriptAttribute] The script {monoScript.name} does not contain an assignable class");
                        }
                    }
                    else
                    {
                        //property.stringValue = ""; dont erase
                    }
                }
            }
            else
            {
                GUI.Label(position, "[MonoScriptAttribute] The MonoScript attribute can only be used on string variables");
            }
        }
    }
#endif
}
