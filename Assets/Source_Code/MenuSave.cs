using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MenuSave : MonoBehaviour
{
	void Start ()
    {
        GameObject.Find("World1").GetComponent<Text>().text = "World 1 - " + Utilities.FindWorldLevel("World1.txt").ToString();
        GameObject.Find("World2").GetComponent<Text>().text = "World 2 - " + Utilities.FindWorldLevel("World2.txt").ToString();
        GameObject.Find("World3").GetComponent<Text>().text = "World 3 - " + Utilities.FindWorldLevel("World3.txt").ToString();
    }


    public void DeleteWorldFile(string world)
    {
        try
        {
            if (File.Exists(world + ".txt"))
            {
                File.Delete(world + ".txt");
                GameObject.Find(world).GetComponent<Text>().text = world + " - " + Utilities.FindWorldLevel(world + ".txt").ToString();
            }
        }

        catch (System.Exception exception)
        {
            Debug.Log(exception.ToString());
        }
    }
}
