using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] Tetrominos;

    public void NewTetromino()
    {
       Instantiate(Tetrominos[Random.Range(0,Tetrominos.Length)],transform.position, Quaternion.identity);
    }
}
