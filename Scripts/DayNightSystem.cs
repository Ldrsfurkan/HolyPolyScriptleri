using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    public Light directionalLight;

    public float dayDurationInSeconds = 24.0f;
    public int currentHour;
    float currenTimeOfDay = 0.35f; // sabah sekiz
    public List<SkyboxTimeMapping> timeMappings;
    private float blendedValue = 0.0f;
    private bool lockNextDayTrigger = false;
    private bool lockSpawn = false;
    public TextMeshProUGUI timeUI;
    

    // Update is called once per frame
    void Update()
    {
        currenTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currenTimeOfDay %= 1;

        currentHour = Mathf.FloorToInt(currenTimeOfDay * 24);

        timeUI.text = $"{currentHour}:00";

        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currenTimeOfDay * 360) - 90, 170, 0));

        UpdateSkybox();
    }

    private void UpdateSkybox()
    {
        Material currentSkybox = null;
        foreach(SkyboxTimeMapping mapping in timeMappings)
        {
            if(currentHour == mapping.hour)
            {
                currentSkybox = mapping.skyboxMaterial;
                if (currentSkybox.shader != null)
                {
                    if(currentSkybox.shader.name == "Custom/SkyboxTransition")
                    {
                        blendedValue += Time.deltaTime;
                        blendedValue = Mathf.Clamp01(blendedValue);

                        currentSkybox.SetFloat("_TransitionFactor",blendedValue);
                    }
                    else
                    {
                        blendedValue = 0;
                    }
                }
            }
        }

        if(currentHour == 0 && lockNextDayTrigger == false)
        {
            TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }

        if(currentHour != 0 )
        {
            lockNextDayTrigger = false;
        }

        if(currentSkybox != null)
        {
            RenderSettings.skybox = currentSkybox;
        }
        
        if(currentHour >= 22 || currentHour <=5)
        {   
            if(lockSpawn == false)
            {
                SkeletonSpawner.Instance.SpawnSkeletons();
                lockSpawn = true;
            }     
        }
        else if(currentHour >5)
        {
            lockSpawn = false;
            SkeletonSpawner.Instance.RemoveAllSkeletons();
        }
    }

}
[System.Serializable]
public class SkyboxTimeMapping
{
    public string phaseName;
    public int hour;
    public Material skyboxMaterial;
}
