using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelLocalManager : MonoBehaviour
{
    [SerializeField]
    private bool _activationNavMesh = false;
    [SerializeField]
    private NavMeshSurface _navMeshSurface;

    [Header("Gestion Animator Vehicules")]
    [SerializeField]
    private bool _activationAnimation = false;
    [SerializeField]
    private Animator[] _animator;
    [SerializeField]
    private Animator[] _animatorInverse;

    [Header("Gestion Light : Nuit")]
    [SerializeField]
    private bool _activationLight = false;
    [SerializeField]
    private Material _skybox;
    [SerializeField]
    private Light _light;


    void Start()
    {
        if(_activationNavMesh == true)
        {
            _navMeshSurface.BuildNavMesh();
        }
        if (_activationAnimation == true)
        {
            for (int i = 0; i < _animator.Length; i++)
            {
                _animator[i].Play("PNJ Vehicule Circulation", 0, Random.Range(0, 1f));
            }
            for (int i = 0; i < _animatorInverse.Length; i++)
            {
                _animatorInverse[i].Play("PNJ Vehicule Circulation Inverse", 0, Random.Range(0, 1f));
            }

        }
        if (_activationLight == true)
        {
            Nuit();
        }
    }

    private void Nuit()
    {
        // 1- Je modifie la skybox et je réduis l'intensité de l'ambiance
        RenderSettings.skybox = _skybox;
        RenderSettings.ambientIntensity = 0.5f;

        // 2- J'assombris la lumière de la scène
        _light.color = Color.black;

    }
}
