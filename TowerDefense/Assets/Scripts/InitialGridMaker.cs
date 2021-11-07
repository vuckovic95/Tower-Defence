using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ova skripta mi sluzi da kreiram grid od kog cu posle napraviti prefab i posle je necu koristiti. Lakse mi je preko koda da kreiram, nego rucno
public class InitialGridMaker : MonoBehaviour
{
    private Transform _gridHolder;
    private int _heightAndWidth = 15;
    private float _offset = 1.1f;
    private List<GameObject> _platforms = new List<GameObject>();
    private void Start()
    {
        _gridHolder = this.transform;

        int helper = -1;
        for (int i = 0; i < Mathf.Pow(_heightAndWidth, 2); i++)
        {
            GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
            platform.transform.parent = _gridHolder.transform;
            _platforms.Add(platform);
            helper++;

            if (i == 0)
            {
                _platforms[i].transform.position = Vector3.zero;
            }
            else
            {
                if (helper <= _heightAndWidth - 1)
                {
                    _platforms[i].transform.position = new Vector3(_platforms[i - 1].transform.position.x + _offset,
                                                                   _platforms[i - 1].transform.position.y,
                                                                   _platforms[i - 1].transform.position.z);
                }
                else
                {
                    _platforms[i].transform.position = new Vector3(0,
                                                                  _platforms[i - 1].transform.position.y,
                                                                  _platforms[i - 1].transform.position.z + _offset);
                    helper = 0;
                }
            }
        }
    }
}
