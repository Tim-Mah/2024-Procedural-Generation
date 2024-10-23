using UnityEditor;
using UnityEngine;

public abstract class Generator : MonoBehaviour
{
    [Tooltip("Tile map this object will be generated on")]
    [SerializeField] protected TileMapRender tileMap;

    public void Generate()
    {
        // Generate has to be public so the button shows up in editor
        CreateTilePlan();
    }

    // Keep these methods protected, generate runs the inplementation
    public abstract void CreateTilePlan();
}

//Generate button in the editor to create room
[CustomEditor(typeof(Generator), true)]

public class Generate : Editor
{
    Generator generator;
    private void Awake()
    {
        generator = (Generator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate"))
        {
            generator.Generate();
        }
    }
}
