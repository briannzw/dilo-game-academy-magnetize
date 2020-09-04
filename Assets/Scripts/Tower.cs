using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private PlayerController player;

    private void Start()
    {
        player = PlayerController.instance;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            player.SelectTower(gameObject);
        }
    }

}
