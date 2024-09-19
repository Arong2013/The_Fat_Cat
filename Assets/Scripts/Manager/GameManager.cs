using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerOBJ;
    Player player;
    public TurnManager turnManager{get; private set;}
    public EntityManager entityManager{get; private set;}
    private void Awake()
    {
        turnManager = transform.GetComponent<TurnManager>();

        GameObject obj = Instantiate(playerOBJ, new Vector3(0, 0, 0), quaternion.identity);
        player = new Player(0.5f, obj.transform);
        turnManager.RegisterEntity(player);


        
        turnManager.Init();
    }
    private void Start()
    {

    }
}
