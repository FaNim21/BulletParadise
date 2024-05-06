using UnityEngine;
using BulletParadise.World;
using BulletParadise.Visual.Drawing;
using BulletParadise.Visual;
using BulletParadise.Entities;
using BulletParadise.DataManagement;
using BulletParadise.Entities.Items;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Components")]
    public SaveManager saveManager;
    public WorldManager worldManager;
    public DrawDebug drawDebug;

    [Header("Prefabs")]
    public static Popup Popup => Instance.prefabPopup;
    public Popup prefabPopup;

    public static Projectile Projectile => Instance.prefabProjectile;
    public Projectile prefabProjectile;

    [Header("Datas")]
    public List<Item> items;
    public int currentEnteredPortalID;


    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Popup.InitializePooling();

        //Application.targetFrameRate = 60;
    }

    public void AddDrawableDebug(IDrawable drawable)
    {
        drawDebug.AddSceneDrawable(drawable);
    }
    public static void AddDrawable(IDrawable drawable)
    {
        Instance.AddDrawableDebug(drawable);
    }

    public void RemoveDrawableDebug(IDrawable drawable)
    {
        drawDebug.RemoveSceneDrawable(drawable);
    }
    public static void RemoveDrawable(IDrawable drawable)
    {
        Instance.RemoveDrawableDebug(drawable);
    }

    public void FindWorldManager()
    {
        worldManager = GameObject.Find("WorldManager").GetComponent<WorldManager>();
        Popup.InitializePooling();
    }

    public Item GetItemFromID(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            var current = items[i];
            if (current.id == id) return current;
        }
        return null;
    }

    public PortalStats GetEnteredPortalStats()
    {
        for (int i = 0; i < saveManager.GameData.portalStats.Count; i++)
        {
            var current = saveManager.GameData.portalStats[i];
            if (current.portalID == currentEnteredPortalID)
                return current;
        }
        return null;
    }
}
