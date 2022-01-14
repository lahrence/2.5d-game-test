using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CursorDictionary: Dictionary<int, Texture2D>, ISerializationCallbackReceiver {
    [HideInInspector][SerializeField] private List<int> _keys = new List<int>();
    [HideInInspector][SerializeField] private List<Texture2D> _values = new List<Texture2D>();

    public void OnBeforeSerialize() {
        _keys.Clear();
        _values.Clear();
    
        foreach (var kvp in this) {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }
    
    public void OnAfterDeserialize() {
        Clear();
    
        for (var i = 0; i != Math.Min(_keys.Count, _values.Count); i++) {
            Add(_keys[i], _values[i]);
        }
    }
}