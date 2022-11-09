using UnityEditor;
using UnityEngine;

namespace Code.Utility
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    static class GameViewMaximizer
    {
        static GameViewMaximizer()
        {
#if UNITY_EDITOR
            EditorApplication.update += Update;
#endif
        }

        static void Update()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying && ShouldToggleMaximize())
                EditorWindow.focusedWindow.maximized = !EditorWindow.focusedWindow.maximized;
#endif
        }

        private static bool ShouldToggleMaximize() => 
            Input.GetKey(KeyCode.Return) && Input.GetKey(KeyCode.LeftShift);
    }
}