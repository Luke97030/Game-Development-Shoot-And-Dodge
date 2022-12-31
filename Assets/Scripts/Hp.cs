using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    //public float maxHealth = 100f; 
    //public bool isDead { get; private set; }
    //private float _currentHealth;
    private MeshRenderer[] _meshes;

    // variable for die animation 
    private Animator _animator;

    public bool playerIsIT = true;
    public bool enemyIsIT = false;
    // Start is called before the first frame update
    void Start()
    {
        // get all meshes under the enemy melee 
        _meshes = GetComponentsInChildren<MeshRenderer>();
        //_currentHealth = maxHealth;
        _animator = GetComponentInChildren<Animator>();
    }

    // enemy takes the damage
    public void reduceHp(float dmg)
    {

        //_currentHealth -= dmg;
        //// Mathf.Clamp return the value between the minimum and maximum health range 
        //_currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);

        //Debug.Log("Current Health: " + _currentHealth);
        StartCoroutine(OnDamage());
    }

    IEnumerator OnDamage()
    {
        foreach (var mesh in _meshes)
        {
            //if (isDead == false)
            //{
                mesh.material.color = Color.red;
            //}          
        }
        // let the player see/notice the color change when enemy takes the damage
        yield return new WaitForSeconds(0.2f);
        foreach (var mesh in _meshes)
        {
                mesh.material.color = Color.white;
        }
    }
}
