using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;

    public void NiveauSuivant()
    {
        PlayerPrefs.SetInt("Niveau", levelManager.NiveauEnCours + 1);

        SceneManager.LoadScene(0);
    }

    public void Recommencer()
    {
        SceneManager.LoadScene(0);
    }

    // Acc�s rapide depuis l'UI (Si vous souhaitez jeter un oeil � tous les niveaux)
    public void NiveauUI(int choixNiveau)
    {
        PlayerPrefs.SetInt("Niveau", choixNiveau);

        SceneManager.LoadScene(0);
    }
}
