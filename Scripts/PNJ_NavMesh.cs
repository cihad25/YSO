using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PNJ_NavMesh : MonoBehaviour
{
    [SerializeField]
    private MunitionController _munitionController;
    [SerializeField]
    private NavMeshAgent _navMeshAgent;
    [SerializeField]
    private Transform[] _chemin;

    private int _prochaineDestination;


    void Update()
    {
        if(_munitionController.DesactiveNavmesh == false)
        {
            _navMeshAgent.SetDestination(_chemin[_prochaineDestination].position);
        }
        else
        {
            _navMeshAgent.speed = 0;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Chemin"))
        {
            if(_prochaineDestination < _chemin.Length - 1)
            {
                _prochaineDestination++;
            }
            else
            {
                _prochaineDestination = 0;
            }
        }
    }
}
