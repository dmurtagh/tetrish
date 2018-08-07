using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeNewAudioTrack
{
    [MenuItem("Assets/Create/New Audio Track")]
    public static void CreateMyAsset()
    {
        AudioTrack asset = ScriptableObject.CreateInstance<AudioTrack>();

        AssetDatabase.CreateAsset(asset, "Assets/Config/NewAudioTrack.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
