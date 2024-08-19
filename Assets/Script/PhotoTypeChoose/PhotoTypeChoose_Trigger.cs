using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotoTypeChoose_Trigger : MonoBehaviour
{
    public void fourcutphoto()
    {
        SceneManager.LoadScene("fourcut");
    }
    public void ai()
    {
        SceneManager.LoadScene("aiphoto");
    }
}
