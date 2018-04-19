using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Utilities : MonoBehaviour
{
    private string fileDirection = "fileName.txt";

    public void PrepareWorldLaunch(string fileName)
    {
        try
        {
            if (File.Exists(fileDirection))
                File.Delete(fileDirection);

            using (StreamWriter writer = new StreamWriter(fileDirection))
            {
                writer.WriteLine(fileName);
            }

            this.ChangeScene("World");
        }

        catch (System.Exception exception)
        {
            Debug.Log(exception.ToString());
        }
    }


    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
