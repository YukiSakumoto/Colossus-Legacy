using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float MaxSoundVolume = 0.5f;
    [SerializeField] private float MaxBGMVolume = 0.3f;

    public float soundVolume;
    public float BGMVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        soundVolume = MaxSoundVolume;
        BGMVolume = MaxBGMVolume;
    }


    public float GetMaxSoundVol() { return MaxSoundVolume; }
    public float GetMaxBGMVol() { return MaxBGMVolume; }

}