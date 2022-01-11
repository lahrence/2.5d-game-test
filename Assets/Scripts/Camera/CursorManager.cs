using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CursorManager {
    public static void ChangeCursor(Texture2D type) {
        Cursor.SetCursor(type, Vector2.zero, CursorMode.ForceSoftware);
    }
}
