using UnityEngine;

public class Crosshair : MonoBehaviour {
    private void OnGUI() {
        GUI.Box(new Rect(Screen.width / 2f, Screen.height / 2f, 10, 10), "");
    }
}