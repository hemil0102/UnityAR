using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetakeImageLogic : MonoBehaviour
{
    [SerializeField] private Button RetakeImageButton;

    private void Start()
    {
        RetakeImageButton.onClick.AddListener(QuitGameToRatakeImage);
    }

    void QuitGameToRatakeImage()
    {
        SceneManager.LoadScene("LoadScene", LoadSceneMode.Single);
    }
}
