using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnableInScene : MonoBehaviour
{
    [SerializeField] private string m_scene;

    private void OnEnable()
    {
        if (m_scene != SceneManager.GetActiveScene().name)
        {
            gameObject.SetActive(false);
        }
    }
}
