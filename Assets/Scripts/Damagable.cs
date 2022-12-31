using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    // only when the bullet hit the enemy, then it is damageable, otherwise, like hitting on the ground or on the wall. It is not damageable
    private Hp _hp;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _hp = GetComponent<Hp>();
    }

    // player hit the enemy, now enemy becomes to IT, player need to run away
    public void inflictDamage(float dmg)
    {
        _hp.enemyIsIT = true;
        _hp.playerIsIT = false;
        gameManager.GetComponent<GameManager>().enemyITCount += 1;
        gameManager.enemyItCountText.text = "Eneemy IT Count: " + gameManager.GetComponent<GameManager>().enemyITCount.ToString();
        _hp.reduceHp(dmg);
    }
}
