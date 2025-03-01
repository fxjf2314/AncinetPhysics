using UnityEngine;
using System.Runtime.InteropServices;
public class LockMouse : MonoBehaviour
{
    // µº»ÎWindows API
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    private Vector2 lockedMousePosition;

    void Start()
    {
        lockedMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Cursor.visible = false;
    }

    void Update()
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        int targetX = screenWidth; 
        int targetY = screenHeight; 

        SetCursorPos(targetX, targetY);
    }
    private void OnDisable()
    {
        Cursor.visible = true;
    }
}