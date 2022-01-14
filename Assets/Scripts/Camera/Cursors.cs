using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursors : MonoBehaviour {
    public List<Texture2D> cursorTextures;
    void Start() {
        for (int i = 0; i < cursorTextures.Count; i++) {
            CursorManager.cursors.Add(i, cursorTextures[i]);
        }
    }
}
