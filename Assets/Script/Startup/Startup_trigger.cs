using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup_trigger : MonoBehaviour
{
    public void startbtn()
    {
        SceneManager.LoadScene("PhotoTypeChoose");
    }
    public void filepath()
    {
        string path = EditorUtility.SaveFolderPanel("Path to Store Image", "", "");
        Debug.Log(path);
        PlayerPrefs.SetString("savePath", path);
    }
}
