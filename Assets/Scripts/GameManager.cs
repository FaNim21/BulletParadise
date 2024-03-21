using UnityEngine;
using BulletParadise.World;
using BulletParadise.Visual.Drawing;
using BulletParadise.Visual;
using BulletParadise.Entities;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public WorldManager worldManager;
    public DrawDebug drawDebug;
    public static Popup Popup => Instance.prefabPopup;
    public Popup prefabPopup;

    public static Projectile Projectile => Instance.prefabProjectile;
    public Projectile prefabProjectile;


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
    }
}
