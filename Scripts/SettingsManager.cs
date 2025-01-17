using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance {get; set; }

    public Button backButton;
    public Slider masterSlider;
    public GameObject masterValue;
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
    }
    private void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            SaveManager.Instance.SaveVolumeSettings(masterSlider.value);
        });
        StartCoroutine(LoadAndApplySettings());
    }
    private IEnumerator LoadAndApplySettings()
    {
        LoadAndSetVolume();
        // Load GraphichsSettings
        // Load KeyBindings eklenebilir.
        yield return new WaitForSeconds(0.1f);
    }

    private void LoadAndSetVolume()
    {
        VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();
        masterSlider.value = volumeSettings.master;
        //music effects falan arttırılabilir yine
    }

    void Update()
    {
        masterValue.GetComponent<TextMeshProUGUI>().text = "%" + ((int)masterSlider.value).ToString() + "";
    }
}
