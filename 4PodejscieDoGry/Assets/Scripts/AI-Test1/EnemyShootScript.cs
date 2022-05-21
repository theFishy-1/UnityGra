using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootScript : MonoBehaviour
{
    public float speed;

    public Transform player;
    private Transform playerlast;
    private Vector3 target;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector3(player.position.x, player.position.y, player.position.z);
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(2);

        playerlast.position = player.position;
    }

    private void Update() {
        Waiting();
        transform.position = Vector3.MoveTowards(transform.position, playerlast.position, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y) {
            DestroyProjctile();
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            DestroyProjctile();
        }
    }

    void DestroyProjctile() {
        Destroy(gameObject);
    }
}
