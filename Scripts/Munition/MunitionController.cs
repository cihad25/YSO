using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MunitionController : MonoBehaviour
{
    [Header("DEBUG")]
    public Text _vitesseTxt;
    public Text _vitesseRotationTxt;
    public void Plus()
    {
        _speed++;
        //_speedRotation += 2;
    }
    public void Moins()
    {
        _speed--;
        //_speedRotation-= 2;

    }
    public void PlusRot()
    {
        _speedRotation ++;
    }
    public void MoinsRot()
    {
        _speedRotation -- ;

    }


    // INSPECTEUR
    [Header("Parametres Globaux")]
    [SerializeField]
    private LevelManager _levelManager;
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private float _speedRotation = 10f;
    [Header("Parametres Joystick")] 
    [SerializeField]
    private FloatingJoystickJ1 joystick;
    [SerializeField]
    private float _sensibilitéJoystickHorizontal = 0.2f; 
    [SerializeField]
    private float _sensibilitéJoystickVertical = 0.2f;

    // VARIABLES/COMPOSANT PRIVEES
    private Transform _munitionTransform;
    private CharacterController _characterController;
    private Rigidbody _munitionRigidbody;
    public bool _desactiveNavMesh = false;

    public bool DesactiveNavmesh { get => _desactiveNavMesh; set => _desactiveNavMesh = value; }


    private void Awake()
    {
        _munitionTransform = GetComponent<Transform>();
        _characterController = GetComponent<CharacterController>();
        //_munitionRigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        _vitesseTxt.text = _speed.ToString();
        _vitesseRotationTxt.text = _speedRotation.ToString();



        MunitionMove();
        //MunitionMoveTactile();


    }

    private void MunitionMove()
    {
        // donner au joueur une vitesse vers l'avant
        Vector3 _moveVector = _munitionTransform.forward * _speed;

        // Pour obtenir la direction
        Vector3 _horizontal = joystick.Horizontal * _munitionTransform.right * _speedRotation * Time.deltaTime;
        Vector3 _vertical = joystick.Vertical * _munitionTransform.up * _speedRotation * Time.deltaTime;
        Vector3 _dir = _horizontal + _vertical;

        // Evite de faire une rotation sur soi
        float _maxX = Quaternion.LookRotation(_moveVector + _dir).eulerAngles.x;


        //if (joystick.Horizontal >= _sensibilitéJoystickHorizontal || joystick.Horizontal <= -_sensibilitéJoystickHorizontal &&
        //    joystick.Vertical >= _sensibilitéJoystickVertical || joystick.Horizontal <= -_sensibilitéJoystickVertical)
        //{
            // ROTATION
            if (_maxX < 90 && _maxX > 70 || _maxX > 270 && _maxX < 290)
            {
                // C'est trop ne fait rien !
            }
            else
            {
                // Ajouter la direction sur le mouvement en cours
                _moveVector += _dir;

                _munitionTransform.rotation = Quaternion.LookRotation(_moveVector);
            }
        //}

        // J'applique mon deplacement
        _characterController.Move(_moveVector * Time.deltaTime);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // DEFAITE
        if (hit.gameObject.CompareTag("Obstacles"))
        {
            // 1- On desactive la munition
            _munitionTransform.gameObject.SetActive(false);

            // 2- Activation Audio de collision
            _levelManager.AudioSource.PlayOneShot(_levelManager.AudioList._audioList[2]);

            // 3- Activation Fin de partie (Camera + Animation + UI)
            Invoke("Defaite", 1f);

        }
        else if (hit.gameObject.CompareTag("PNJ"))
        {
            // 0- Animation IA
            hit.gameObject.transform.parent.root.GetComponent<Animator>().SetTrigger("Mort");

            // 1- On desactive la munition
            _munitionTransform.gameObject.SetActive(false);

            // 2- Activation Audio de collision
            _levelManager.AudioSource.PlayOneShot(_levelManager.AudioList._audioList[2]);

            // 3- Activation Fin de partie (Camera + Animation + UI)
            Invoke("Defaite", 1f);
        }
        else if (hit.gameObject.CompareTag("Player"))
        {
            // 0- Animation IA
            hit.gameObject.GetComponent<Animator>().SetTrigger("Mort");
            hit.gameObject.GetComponent<Animator>().applyRootMotion = true;
            hit.gameObject.GetComponent<Rigidbody>().useGravity = true;


            // 1- On desactive la munition
            _munitionTransform.gameObject.SetActive(false);

            // 2- Activation Audio de collision
            _levelManager.AudioSource.PlayOneShot(_levelManager.AudioList._audioList[2]);

            // 3- Activation Fin de partie (Camera + Animation + UI)
            Invoke("Defaite", 1f);
        }
        // VICTOIRE
        else if (hit.gameObject.CompareTag("Cible"))
        {
            // 1- On desactive la munition
            _munitionTransform.gameObject.SetActive(false);

            // 2- Activation Audio de collision
            _levelManager.AudioSource.PlayOneShot(_levelManager.AudioList._audioList[2]);
            _levelManager.AudioSource.PlayOneShot(_levelManager.AudioList._audioList[3]);

            // 2.5 Activation effet conffeti
            _levelManager.ParticleSystemConfetti.SetActive(true);


            // 4- Retour vers le joueur (on réacitve l'autre camera + Animation)
            Invoke("Victoire", 1f);

        }
        else if (hit.gameObject.CompareTag("PNJ - Ennemi"))
        {
            // 0Bis- Desactivation NavMesh (si applicable)
            _desactiveNavMesh = true;

            // 0- Animation IA
            hit.gameObject.GetComponent<Animator>().SetTrigger("Mort");

            // 1- On desactive la munition
            _munitionTransform.gameObject.SetActive(false);

            // 2- Activation Audio de collision
            _levelManager.AudioSource.PlayOneShot(_levelManager.AudioList._audioList[2]);
            _levelManager.AudioSource.PlayOneShot(_levelManager.AudioList._audioList[3]);

            // 2.5 Activation effet conffeti
            _levelManager.ParticleSystemConfetti.SetActive(true);


            // 4- Retour vers le joueur (on réacitve l'autre camera + Animation)
            Invoke("Victoire", 1f);
        }

    }
    private void Defaite()
    {
        // 4- Changement de camera
        _levelManager.CameraGame.SetActive(false);
        _levelManager.CameraMenu.SetActive(true);

        // 5 - Activation UI Game Over
        _levelManager.CanvasDefaite.SetActive(true);

        // 6- Activation Animation
        _levelManager.PlayerAnimator.SetTrigger("Defaite");
    }
    private void Victoire()
    {
        // 4- Changement de camera
        _levelManager.CameraGame.SetActive(false);
        _levelManager.CameraMenu.SetActive(true);


        // 5- Activation UI de victoire
        _levelManager.CanvasVictoire.SetActive(true);

        // 6 - Activation Animation
        _levelManager.PlayerAnimator.SetTrigger("Victoire");

    }

}
