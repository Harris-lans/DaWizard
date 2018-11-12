using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
  
    [SerializeField]
    private float Cooldown = 8;

    [SerializeField]
    private GameObject PickupPrefab;

    private void Start()
    {
        SpawnPrefab();
    }

    private void SpawnPrefab()
    {

        GameObject NewPickup = Instantiate(PickupPrefab, transform.position, transform.rotation);
        NewPickup.GetComponent<PickUp>().SetSpawner(gameObject);
    }

    internal void PickupTaken()
    {
        Invoke("SpawnPrefab", Cooldown);
    }


}
