using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DungeonGen generator;
    [SerializeField] private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player.position *= 0;
        generator.CreateTilePlan();
        player.position = new Vector3 (generator.GetSpawnPoint().x, generator.GetSpawnPoint().y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
