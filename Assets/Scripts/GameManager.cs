using UnityEngine;
using BulletParadise.World;
using BulletParadise.Visual.Drawing;
using BulletParadise.Visual;
using BulletParadise.Entities;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public WorldManager worldManager;
    public DrawDebug drawDebug;


    public static Popup Popup => Instance.prefabPopup;
    public Popup prefabPopup;

    public static Projectile Projectile => Instance.prefabProjectile;
    public Projectile prefabProjectile;

    /*public static InventorySlot InventorySlot => Instance.inventorySlot;
    public InventorySlot inventorySlot;

    public static Bag Bag => Instance.bag;
    public Bag bag;*/


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
        drawDebug.AddDrawable(drawable);
    }
    public static void AddDrawable(IDrawable drawable)
    {
        Instance.AddDrawableDebug(drawable);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
