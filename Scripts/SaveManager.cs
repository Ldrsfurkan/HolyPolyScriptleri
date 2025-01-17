using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance {get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
     
    public bool isSavingToJson;
    //Json Save Path
    string jsonPathProject;
    //Json Gerçek Save Path
    string jsonPathPersistant;
    //Binary Save Path
    string binaryPath;
    string fileName = "SaveGame";
    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar ;
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

    #region || ------- General Section ------- ||     
    
    #region || ------- Saving ------- ||
    
   
    public void SaveGame(int slotNumber)
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();
        SavingTypeSwitch(data, slotNumber);
    }

    private PlayerData GetPlayerData()
    {
        float[] playerStats = new float[3];
        playerStats[0] = PlayerStatus.Instance.currentHealth;
        playerStats[1] = PlayerStatus.Instance.currentFood;
        playerStats[2] = PlayerStatus.Instance.currentWater;

        float[] playerPosAndRot = new float[6];
        playerPosAndRot[0] = PlayerStatus.Instance.playerBody.transform.position.x;
        playerPosAndRot[1] = PlayerStatus.Instance.playerBody.transform.position.y;
        playerPosAndRot[2] = PlayerStatus.Instance.playerBody.transform.position.z;

        playerPosAndRot[3] = PlayerStatus.Instance.playerBody.transform.rotation.x;
        playerPosAndRot[4] = PlayerStatus.Instance.playerBody.transform.rotation.y;
        playerPosAndRot[5] = PlayerStatus.Instance.playerBody.transform.rotation.z;

        return new PlayerData(playerStats, playerPosAndRot);
    }

    public void SavingTypeSwitch(AllGameData gameData, int slotNumber)
    {
        if(isSavingToJson)
        {
            SaveGameDataToJsonFile(gameData, slotNumber);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData, slotNumber);
        }
    }
    #endregion

    #region ||------- Loading----- ||
    
    public AllGameData LoadingTypeSwitch(int slotNumber)
    {
        if(isSavingToJson)
        {
            // json için ayrı daha sonra
            AllGameData gameData = LoadGameDataFromJsonFile(slotNumber);
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataFromBinaryFile(slotNumber);
            return gameData;
        } 
    }
    public void LoadGame(int slotNumber)
    {
        //Player Data
        SetPlayerData(LoadingTypeSwitch(slotNumber).playerData);
        //Environment Data ve diğerleri daha sonra
    }
    
    private void SetPlayerData(PlayerData playerData)
    {
        //Player Stats
        PlayerStatus.Instance.currentHealth = playerData.playerStats[0];
        PlayerStatus.Instance.currentFood = playerData.playerStats[1];
        PlayerStatus.Instance.currentWater = playerData.playerStats[2];

        //Player Position
        Vector3 loadedPosition;
        loadedPosition.x = playerData.playerPositionAndRotation[0];
        loadedPosition.y = playerData.playerPositionAndRotation[1];
        loadedPosition.z = playerData.playerPositionAndRotation[2];

        

        //Player Rotation
        Vector3 loadedRotation;
        loadedRotation.x =playerData.playerPositionAndRotation[3];
        loadedRotation.y =playerData.playerPositionAndRotation[4];
        loadedRotation.z =playerData.playerPositionAndRotation[5];

        

        PlayerStatus.Instance.playerBody.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        PlayerStatus.Instance.playerBody.GetComponent<CharacterController>().enabled = false;

        PlayerStatus.Instance.playerBody.transform.position = loadedPosition;
        PlayerStatus.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);

        StartCoroutine(DelayForSetPlayerData());

        PlayerStatus.Instance.playerBody.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        PlayerStatus.Instance.playerBody.GetComponent<CharacterController>().enabled = true;

        Debug.Log($"Position Loaded: {loadedPosition}, Rotation Loaded: {loadedRotation}");
    }

    private IEnumerator DelayForSetPlayerData()
    {
        yield return new WaitForSeconds(0.2f);
    }

    public void StartLoadedGame(int slotNumber)
    {
        SceneManager.LoadScene("GameScene");

        StartCoroutine(DelayedLoading(slotNumber));
    }

    public IEnumerator DelayedLoading(int slotNumber)
    {
        yield return new WaitForSeconds(1f);

        LoadGame(slotNumber);
    }
    #endregion
    #endregion

     #region || ------- Binary Section ------- ||

    public void SaveGameDataToBinaryFile(AllGameData gameData, int slotNumber)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        
        FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Data Saved to" + binaryPath + fileName + slotNumber + ".bin");


    }
    public AllGameData LoadGameDataFromBinaryFile(int slotNumber)
    {
      
       if(File.Exists(binaryPath + fileName + slotNumber + ".bin"))
       {

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Open);

        AllGameData data = formatter.Deserialize(stream) as AllGameData;
        stream.Close();
        
        print("Data Loaded from" + binaryPath + fileName + slotNumber + ".bin");

        return data;

       }
       else
       {
        return null;
       }
    }


    #endregion

    #region || ------- Json Section ------- ||
    public void SaveGameDataToJsonFile(AllGameData gameData, int slotNumber)
    {
        string json = JsonUtility.ToJson(gameData);

        using (StreamWriter writer = new StreamWriter(jsonPathPersistant + fileName + slotNumber + ".json")) // çıkarırken jsonPathPresistent kullanılcak.
        {
            writer.Write(json);
            print(jsonPathPersistant + fileName + slotNumber + ".json");
        };
    }
    public AllGameData LoadGameDataFromJsonFile(int slotNumber)
    {
        using (StreamReader reader = new StreamReader(jsonPathPersistant + fileName + slotNumber + ".json")) // çıkarırken jsonPathPresistent kullanılcak. test için
        {
            string json = reader.ReadToEnd();

            AllGameData data = JsonUtility.FromJson<AllGameData>(json);
            return data;
        };
    }
    #endregion

    #region || ------- Settings Section ------- ||
    
    #region || ------- Volume Settings  ------- ||
    [System.Serializable]
    public class VolumeSettings
    {
        public float master;
        // daha fazla müzik/effekt sesleri vs eklenirse buraya eklenebilir. SaveMasterVolume ve LoadMasterVolume i tekrarlarsın.
    }

    public void SaveVolumeSettings(float _master)
    {   
        VolumeSettings volumeSettings = new VolumeSettings()
        {
            master = _master
        };

        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings)); // tüm classı JsonUtility Sayesinde string olarak setliyoruz.
        PlayerPrefs.Save();
    }
    public VolumeSettings LoadVolumeSettings()
    {
        return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
    }
    #endregion
    
    #endregion
    
    #region || ------- Utility ------- ||

    public bool DoesFileExists(int slotNumber)
    {
        if(isSavingToJson)
        {
            if(File.Exists(jsonPathPersistant + fileName + slotNumber + ".json")) // /SaveGame2.json
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if(File.Exists(binaryPath + fileName + slotNumber + ".bin")) // /SaveGame3.bin
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool IsSlotEmpty(int slotNumber)
    {
        if(DoesFileExists(slotNumber))
        {
            return false; //tam tersi
        }
        else
        {
            return true;
        }
    }
    public void DeselectButton()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }


    #endregion
    }


    

