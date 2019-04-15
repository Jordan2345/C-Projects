using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameControl : MonoBehaviour
{
    [SerializeField] public string name;
    public static GameControl control;
    private string planet;

    private void Awake()
    {
        //name = RetrieveName.save_Names.userName;
        if (CookieClick.gameData == null && control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
    }
    //KEEP THIS
    private void Update()
    {
        name = RetrieveName.save_Names.userName;
    }
    private void Start()
    {
        name = RetrieveName.save_Names.userName;
        Load();
    }
    public void Save()
    {
        //Get Scene
        if (SceneManager.GetActiveScene().name.Equals("Moon Game Scene"))
            planet = "Moon";
        else if(SceneManager.GetActiveScene().name.Equals("Earth Game Scene"))
            planet = "Earth";
        string concat = planet + name;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/GameInfo/GameInfo"+concat+".dat");
        GameData data = new GameData();

        data.numCookiesClicked = CookieClick.numCookiesClicked;
        data.pointerLevel = CookieClick.pointerLevel;
        data.autoClickerLevel = CookieClick.autoClickerLevel;
        data.pointerCost = CookieClick.pointerCost;
        data.autoClickerCost = CookieClick.autoClickerCost;
        data.multiplierCost = CookieClick.multiplierCost;
        data.pointerIncrement = CookieClick.pointerIncrement;
        data.autoClickerIncrement = CookieClick.autoClickerIncrement;
        data.multiplierCoefficient = CookieClick.multiplierCoefficient;
        data.multiplierLevel = CookieClick.multiplierLevel;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Saved [" + planet+"]["+name+"] data");
    }
    public void Load()
    {
        string planet = "";
        //Get Scene
        if (SceneManager.GetActiveScene().name.Equals("Moon Game Scene"))
            planet = "Moon";
        else if (SceneManager.GetActiveScene().name.Equals("Earth Game Scene"))
            planet = "Earth";
        string concat = planet + name;
        Debug.Log("[" + planet + "][" + name + "]");
        Debug.Log("{" + concat + "}");
        if (File.Exists(Application.persistentDataPath + "/GameInfo/GameInfo" + concat+".dat") && !planet.Equals(""))
        {
            
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/GameInfo/GameInfo" + concat + ".dat", FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            CookieClick.numCookiesClicked = data.numCookiesClicked;
            CookieClick.pointerLevel = data.pointerLevel;
            CookieClick.autoClickerLevel = data.autoClickerLevel;
            CookieClick.pointerCost = data.pointerCost;
            CookieClick.autoClickerCost = data.autoClickerCost;
            CookieClick.multiplierCost = data.multiplierCost;
            CookieClick.pointerIncrement = data.pointerIncrement;
            CookieClick.autoClickerIncrement = data.autoClickerIncrement;
            CookieClick.multiplierCoefficient = data.multiplierCoefficient;
            CookieClick.multiplierLevel = data.multiplierLevel;
            Debug.Log("Loaded [" + planet + "][" + name + "] data");
        }
    }
    public void NewGameScene()
    {
        SceneManager.LoadScene("NewGame");
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("LoadGame");
    }
    public void MenuScene()
    {
        SceneManager.LoadScene("Startup");
    }
    public void MoonScene()
    {
        //go to moon
        Save();
        SceneManager.LoadScene("Moon Game Scene");
    }
    public void EarthScene()
    {
        //go to earth
        Save();
        SceneManager.LoadScene("Earth Game Scene");
    }
    public void FromStartToEarth()
    {
        name = RetrieveName.save_Names.userName;
        Debug.Log(name);
        if(File.Exists(Application.persistentDataPath + "/GameInfo/GameInfoEarth" + name + ".dat") || File.Exists(Application.persistentDataPath + "/GameInfo/GameInfoMoon" + name + ".dat"))
        {
            if(name.Equals(""))
            {
                Debug.Log("No name entered");
            }
            //checks if there is already a file with the same given name
            SceneManager.LoadScene("Startup");
            Debug.Log("File with name [" + name + "] already exists");
        }
        else
        {
            EarthScene();
        }
    }
    public void FromStartLoadName()
    {
        //TODO-This
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game has been quit");
    }
    private void OnApplicationQuit()
    {
        //when application is closing, call save function
        Save();
        Debug.Log("Saved [" + planet + "][" + name + "]'s data on exit");
    }
}
[Serializable]
class GameData
{
    public float numCookiesClicked, pointerLevel, autoClickerLevel, pointerCost, autoClickerCost, multiplierCost,
                            pointerIncrement, autoClickerIncrement, multiplierCoefficient, multiplierLevel;
}

