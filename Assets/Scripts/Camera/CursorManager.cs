using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CursorManager {
    static KeyValuePair<int, Texture2D> currentCursor = new KeyValuePair<int, Texture2D>(999, null);
    static public Dictionary<int, Texture2D> cursors = new Dictionary<int, Texture2D>();
    public static void ChangeCursor(KeyValuePair<int, Texture2D> type) {
        if (type.Key != currentCursor.Key) {
            Cursor.SetCursor(type.Value, Vector2.zero, CursorMode.ForceSoftware);
            currentCursor = type;
        }
    }

    public static void Default() {
        ChangeCursor(new KeyValuePair<int, Texture2D>(0, cursors[0]));
    }

    public static void Pointer() {
        ChangeCursor(new KeyValuePair<int, Texture2D>(1, cursors[1]));
    }
}
