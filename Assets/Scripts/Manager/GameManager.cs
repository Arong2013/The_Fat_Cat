using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject playerOBJ, enemyOBJ;
    Player player;
    public TurnManager turnManager { get; private set; }
    public EntityManager entityManager { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        turnManager = transform.GetComponent<TurnManager>();
        entityManager = transform.GetComponent<EntityManager>();

        GameObject obj = Instantiate(playerOBJ, new Vector3(0, 0, 0), quaternion.identity);
        player = new Player(0.5f, obj.transform);
        turnManager.RegisterEntity(player);
        entityManager.RegisterEntity(player);

        GameObject enemy = Instantiate(enemyOBJ, new Vector3(0, 0, 2), quaternion.identity);
        var Enemy  = new Enemy(0.5f, enemy.transform);
        turnManager.RegisterEntity(Enemy);
        entityManager.RegisterEntity(Enemy);


        turnManager.Init();
    }
    private void Start()
    {

    }
}
