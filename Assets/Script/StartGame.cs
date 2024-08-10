using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField, TabGroup("Button")] private Button startGame;

    private void Awake()
    {
        startGame.onClick.AddListener(() => ChangeScene());
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}
