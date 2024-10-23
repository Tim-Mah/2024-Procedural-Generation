using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "RoomParam", menuName = "Dungeon/Room Parameters")]

public class BaseRoomParam : ScriptableObject
{
    [SerializeField] public RoomStyles style;

    [SerializeField] public int variation = 0, iterations = 15, walkLength = 7, width = 10, height = 10, radius = 10;
}

public enum RoomStyles
{Random, Rectangle, Circle}

[CustomEditor(typeof(BaseRoomParam), true), CanEditMultipleObjects]
class MyClassEditor : Editor
{
    BaseRoomParam brp;
    private void Awake()
    {
        brp = (BaseRoomParam)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (brp.style == RoomStyles.Random)
        {
            DrawPropertiesExcluding(serializedObject, "width", "height", "variation", "radius");
        }else if (brp.style == RoomStyles.Rectangle)
        {
            DrawPropertiesExcluding(serializedObject, "iterations", "walkLength", "radius");
        }
        else if (brp.style == RoomStyles.Circle)
        {
            DrawPropertiesExcluding(serializedObject, "iterations", "walkLength", "width", "height");
        }
        else
        {
            DrawDefaultInspector();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
