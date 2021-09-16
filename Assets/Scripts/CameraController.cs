using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] 
    private GameObject _gamPlayCam;
    [SerializeField] 
    private GameObject _mainMenuCam;
    
    public void EnableGamePlayCamera()
    {
        _mainMenuCam.SetActive(false);
        _gamPlayCam.SetActive(true);
    }
}
