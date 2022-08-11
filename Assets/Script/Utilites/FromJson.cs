using System;
using UnityEngine;

[Serializable]
public class FromJson
{
    public string MAGIC_ATTACK;
    public string MAGIC_DEFENSE;

    public static FromJson LoadJsonString(string jsonString)
    {
        if (jsonString == null)
            return null;

        return JsonUtility.FromJson<FromJson>(jsonString);
    }
}

