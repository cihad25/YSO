using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Niveau
{
    public string nom;
    public GameObject _niveau;
}

public class LevelManager : MonoBehaviour
{
    [Header("Lancement niveau")]
    [SerializeField]
    private Animator _playerAnimator;
    [SerializeField]
    private GameObject _munition;
    [SerializeField]
    private GameObject _cameraMenu;
    [SerializeField]
    private GameObject _cameraGame;

    [Header("Gestion Audio")]
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioList _audioList;

    [Header("Gestion Defaite/Victoire")]
    [SerializeField]
    private GameObject _canvasVictoire;
    [SerializeField]
    private GameObject _canvasDefaite;
    [SerializeField]
    private GameObject _particleSystemConfetti;

    [Header("Gestion Liste de Niveau")]
    [SerializeField]
    private List<Niveau> _niveauList = new List<Niveau>();

    // VARIABLES PRIVEES
    private int _niveauEnCours;

    // PROPRIETES
    public AudioSource AudioSource { get => _audioSource; }
    public AudioList AudioList { get => _audioList;  }
    public Animator PlayerAnimator { get => _playerAnimator; }
    public GameObject CameraMenu { get => _cameraMenu; }
    public GameObject CameraGame { get => _cameraGame; }
    public GameObject CanvasVictoire { get => _canvasVictoire; }
    public GameObject CanvasDefaite { get => _canvasDefaite; }
    public GameObject ParticleSystemConfetti { get => _particleSystemConfetti; }
    public int NiveauEnCours { get => _niveauEnCours; set => _niveauEnCours = value; }

    private void Awake()
    {
        _niveauEnCours = PlayerPrefs.GetInt("Niveau");
        _niveauList[_niveauEnCours]._niveau.SetActive(true);
    }
    void Start()
    {
        // Permet de mettre en pause notre animation pour avoir la position de tir
        _playerAnimator.SetBool("Shoot", true);
        _playerAnimator.speed = 0;

    }

    public void Play()
    {
        // 1- On change de camera
        _cameraMenu.SetActive(false);
        _cameraGame.SetActive(true);

        // 2- On active notre effet d'animation
        _playerAnimator.speed = 1;

        // On joue l'audio
        _audioSource.gameObject.GetComponentInChildren<AudioSource>().PlayOneShot(_audioList._audioList[0], 0.2f);
        _audioSource.PlayOneShot(_audioList._audioList[1]);

        // On active notre munition
        _munition.SetActive(true);

    }
}
