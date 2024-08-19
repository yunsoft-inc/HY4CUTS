using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class takeashot : MonoBehaviour
{
    public RawImage display;
    WebCamTexture camTexture;
    public int currentIndex = 2;
    private int photoIndex = 0;
    public float default_time = 10f;
    private float time;
    public Text timer;

    public RawImage prev1;
    public RawImage prev2;
    public RawImage prev3;
    public RawImage prev4;

    public GameObject loading;

    private string savePath = "C:/";

    private void Start()
    {
        time = default_time;

        Debug.Log(Application.persistentDataPath + "/photo" + photoIndex.ToString() + ".png");
        savePath = PlayerPrefs.GetString("savePath", "C:/");

        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
        {
            Debug.Log(devices[i].name);
        }
        if (camTexture != null)
        {
            display.texture = null;
            camTexture.Stop();
            camTexture = null;
        }
        WebCamDevice device = WebCamTexture.devices[currentIndex];
        camTexture = new WebCamTexture(device.name, 640,480,30);
        display.texture = camTexture;
        camTexture.Play();
    }

    void Update()
    {
        if(photoIndex <= 3)
        {
            if (time > 0)
                time -= Time.deltaTime;
            else
            {
                time = default_time;
                StartCoroutine(TakePhoto());
            }
            timer.text = Mathf.Ceil(time).ToString();
        }
    }

    public void takephoto()
    {
        time = default_time;
        StartCoroutine(TakePhoto());
    }
    IEnumerator TakePhoto()  // Start this Coroutine on some button click
    {

        // NOTE - you almost certainly have to do this here:

        yield return new WaitForEndOfFrame();

        // it's a rare case where the Unity doco is pretty clear,
        // http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
        // be sure to scroll down to the SECOND long example on that doco page 

        Texture2D photo = new Texture2D(camTexture.width, camTexture.height);
        photo.SetPixels(camTexture.GetPixels());
        photo.Apply();

        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        if (photoIndex <= 3)
        {
            File.WriteAllBytes(savePath + "/photo" + photoIndex.ToString() + ".png", bytes);
        }
        else
            photoIndex = 0;

        //apply to Prev
        if (photoIndex == 0)
        {
            prev1.texture = photo;
        }
        else if (photoIndex == 1)
        {
            prev2.texture = photo;
        }
        else if (photoIndex == 2)
        {
            prev3.texture = photo;
        }
        else if (photoIndex == 3)
        {
            prev4.texture = photo;
            loading.SetActive(true);
            Process.Start(savePath + "/background.vbs");
            StartCoroutine(loadingPage());
        }
        photoIndex++;
    }
    IEnumerator loadingPage()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("fourcutwait");
    }
    public void filepath()
    {
        string path = EditorUtility.SaveFolderPanel("Path to Store Image", "", "");
        Debug.Log(path);
        savePath = path;
        PlayerPrefs.SetString("savePath", path);
    }
}
