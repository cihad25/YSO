using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunitionCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _lookAt;

    [SerializeField]
    private float _offset = 1.5f;
    [SerializeField]
    private float _distance = 3.5f;

    private Vector3 _newPos;




    void Update()
    {
        // Nouvelle Position
        _newPos = _lookAt.position + (-transform.forward * _distance) + (transform.up * _offset);
        transform.position = Vector3.Lerp(transform.position, _newPos, 0.05f);

        // Nouvelle Rotation
        transform.LookAt(_lookAt);
    }
}
