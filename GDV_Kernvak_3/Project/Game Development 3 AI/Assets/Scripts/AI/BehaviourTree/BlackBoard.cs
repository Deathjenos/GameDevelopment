using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard
{
    //BlackBoard Content
    private Dictionary<string, object> dictionary = new Dictionary<string, object>();

    //Constructor
    public BlackBoard(params KeyValuePair<string, object>[] variables)
    {
        foreach (KeyValuePair<string, object> kv in variables)
        {
            SetValue(kv.Key, kv.Value);
        }
    }

    //Get Value out of Content
    public T GetValue<T>(string key)
    {
        if (dictionary.ContainsKey(key))
        {
            return (T)dictionary[key];
        }
        Debug.LogError("Key does noet exist in Blackboard: " + key + " for object");
        return default(T);
    }

    //Set Value in Content
    public void SetValue<T>(string key, T value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

}//CLASS
