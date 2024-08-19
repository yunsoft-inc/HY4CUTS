using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class fourcut : MonoBehaviour
{
    public Text label;
    int frameType = 0;
    int pageCount = 1;
    public int MaxCount = 3;
    public GameObject view1;
    public GameObject view2;
    private string savePath;

    public RawImage ri1;
    public RawImage ri2;
    public RawImage ri3;
    public RawImage ri4;

    public GameObject loading;


    void Start()
    {
        savePath = PlayerPrefs.GetString("savePath", "C:/");
        
        string path = savePath + "/frame1_layout.png";
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex1 = new Texture2D(2, 2);
        tex1.LoadImage(bytes);
        ri1.texture = tex1;
        
        path = savePath + "/frame2_layout.png";
        bytes = File.ReadAllBytes(path);
        Texture2D tex2 = new Texture2D(2, 2);
        tex2.LoadImage(bytes);
        ri2.texture = tex2;

        path = savePath + "/frame3_layout.png";
        bytes = File.ReadAllBytes(path);
        Texture2D tex3 = new Texture2D(2, 2);
        tex3.LoadImage(bytes);
        ri3.texture = tex3;

        path = savePath + "/frame4_layout.png";
        bytes = File.ReadAllBytes(path);
        Texture2D tex4 = new Texture2D(2, 2);
        tex4.LoadImage(bytes);
        ri4.texture = tex4;
    }

    public void frame1()
    {
        frameType = 1;
        ViewChange(1);
    }
    public void frame2()
    {
        frameType = 2;
        ViewChange(1);
    }
    public void frame3()
    {
        frameType = 3;
        ViewChange(1);
    }
    public void frame4()
    {
        frameType = 4;
        ViewChange(1);
    }

    public void Back()
    {
        ViewChange(0);
    }
    
    private void ViewChange(int num)
    {
        if (num == 1)
        {
            view1.SetActive(false);
            view2.SetActive(true);
            PlayerPrefs.SetInt("frameType", frameType);
        }
        else if (num == 2)
        {
            
            PlayerPrefs.SetInt("pageCount", pageCount);
        }
        else if(num == 0)
        {
            view1.SetActive(true);
            view2.SetActive(false);
        }
    }

    public void plus()
    {
        if(pageCount >= 1 && pageCount < MaxCount)
        {
            pageCount++;
            label.text = pageCount.ToString();
        }
    }
    public void minus()
    {
        if (pageCount > 1 && pageCount <= MaxCount)
        {
            pageCount--;
            label.text = pageCount.ToString();
        }
    }

    public void Confirm()
    {
        ViewChange(2);
        loading.SetActive(true);
        StartCoroutine(load());
    }
    IEnumerator load()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("takeashot");
    }
    public void filepath()
    {
        string path = EditorUtility.SaveFolderPanel("Path to Store Image", "", "");
        Debug.Log(path);
        savePath = path;
        PlayerPrefs.SetString("savePath", path);
    }
}
