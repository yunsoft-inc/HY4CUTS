using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.Experimental.RestService;
using UnityEngine.UI;

public class loading : MonoBehaviour
{
    string path;
    public RawImage ri;
    // Start is called before the first frame update
    void Start()
    {
        path = PlayerPrefs.GetString("savePath", "C:/");
        path = path + "/output.png";

        byte[] bytes = File.ReadAllBytes(path);

        Texture2D tex = new Texture2D(2, 2);

        tex.LoadImage(bytes);
        ri.texture = tex;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
