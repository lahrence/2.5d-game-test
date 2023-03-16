using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CursorManager {
    static KeyValuePair<int, Texture2D> currentCursor = new KeyValuePair<int, Texture2D>(999, null);
    static public Dictionary<int, Texture2D> cursors = new Dictionary<int, Texture2D>();
    public static void ChangeCursor(KeyValuePair<int, Texture2D> type, Vector2 hotspot) {
        if (type.Key != currentCursor.Key) {
            Cursor.SetCursor(type.Value, hotspot, CursorMode.ForceSoftware);
            currentCursor = type;
        }
    }

    public static void Default() {
        ChangeCursor(new KeyValuePair<int, Texture2D>(0, cursors[0]), Vector2.zero);
    }

    public static void Pointer() {
        ChangeCursor(new KeyValuePair<int, Texture2D>(1, cursors[1]), new Vector2(6,0));
    }
}
