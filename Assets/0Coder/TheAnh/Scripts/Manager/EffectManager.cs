
using System;
using System.Collections.Generic;
using DesignPattern;
using DesignPattern.ObjectPool;
using UnityEditor;
using UnityEngine;

public enum EffectID
{
    HorizontalCollect,
    VerticalCollect
}

public class EffectManager : Singleton<EffectManager>
{ 
    [SerializeField] private List<EffectSource> m_EffectSources;
    private const string m_AddressEffect = "Assets/0Coder/TheAnh/Prefabs/Effects";
    private float posX = -4;
    public static void PlayEffect(EffectID effectID, Vector3 position)
    {
        GameObject effectPrefab = Instance.m_EffectSources[(int)effectID].EffectPrefab;
        if (effectPrefab == null)
        {
            Debug.LogWarning("Effect " + effectID.ToString() + " is null");
            return;
        }

        PoolingManager.Spawn(effectPrefab, position, Quaternion.identity);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayEffect(EffectID.HorizontalCollect,new Vector3(posX,0,0));
            posX++;
        }
    }

    private void LoadData()
    {
        string[] names = Enum.GetNames(typeof(EffectID));
        if (m_EffectSources == null) m_EffectSources = new List<EffectSource>();
       
        while(m_EffectSources.Count > names.Length) m_EffectSources.RemoveAt(m_EffectSources.Count - 1);
        for (int i = 0; i < names.Length; ++i)
        {
            if (i < m_EffectSources.Count) m_EffectSources[i].Name = names[i];
            else m_EffectSources.Add(new EffectSource() { Name = names[i] });

            m_EffectSources[i].LoadEffectPrefab(m_AddressEffect);
        }
    }

    private void Reset()
    {
        LoadData();
    }

    private void OnValidate()
    {
        LoadData();
    }
}

[Serializable]
public class EffectSource
{
    [HideInInspector] public string Name;
    [SerializeField] private GameObject m_EffectPrefab;
    public GameObject EffectPrefab
    {
        get => m_EffectPrefab;
    }

    public void LoadEffectPrefab(string folderPath)
    {
        string prefabPath = $"{folderPath}/{Name}.prefab";
        m_EffectPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (m_EffectPrefab == null)
        {
            Debug.LogWarning($"Can't find prefab: {prefabPath}");
        }
    }
}
