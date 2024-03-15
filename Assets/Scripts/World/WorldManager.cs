using BulletParadise.Entities;
using BulletParadise.Player;
using BulletParadise.Visual.Drawing;
using System.Collections.Generic;
using UnityEngine;

namespace BulletParadise.World
{
    /// <summary>+
    /// Narazie swiat bazuje na jednym 'chunku' dla nie wiadomego rozmiaru mapy i rozkladu obiektow typu sciany dla budynynkow itp itd
    /// Bazowe zaleznosci, ktore trzeba rozwiklac to:
    /// - Budynki jako czesc mapy do ktorych mozna wchodzic
    /// - NPC AI - prawdopodobnie GOAP
    /// - ENEMY AI - prawdopodobnie zwykle state machine dla zwyklych i behaviour tree dla bossow/inteligentniejszysch enemy
    /// - poziomy swiata takie jak pietra budynkow czy tez podziemia?
    /// - i wiele wiecej
    /// </summary>
    public class WorldManager : MonoBehaviour
    {
        public List<SceneObject> staticObjects;
        public IReadOnlyList<SceneObject> ObjectsReadOnly => staticObjects;

        public List<SceneObject> dynamicObjects;
        public IReadOnlyList<SceneObject> DynamicObjectsReadOnly => dynamicObjects;

        public List<Entity> entities;
        public IReadOnlyList<Entity> EntitiesReadOnly => entities;


        [SerializeField] private DrawDebug _drawDebug;

        [Header("Values")]
        public float detectBagsRadius;
        public float renderingRadius;
        public float mobsRenderingRadius;
        public bool isWorldUpdating;

        [Header("Debug")]
        [ReadOnly, SerializeField] private float _sqrDistance;


        private void Awake()
        {
            //Trzeba ztestowac czy IReadOnlyList<> czy span<> bedzie lepszy, bo w sumie finalnego efektu nie znam do tego co bedzie szybsze
            //bo niby i tak lista jest robiona raz i wartosci w niej sa zmieniane, a z drugiej strony to nie wiem xD trzeba to potestowac

            //Dochodzi fakt ewentualnego rodzielenia obiektow 'dynamicznych' czyli rzeczy jak worek, ktore znikaja i sie pojawiaja itp itd
            //zeby mialy inna wlasnosc na scenie

            //staticObjects = new List<SceneObject>();

            //baza tez jest fakt czy chce zeby wszystkie gameobjecty ktore sa rodzicem obiektu World byly inicjowane z world managera z klasa?
            //czy zeby kazdy obiekt w scenie dodawany mial juz ta wlasnosc tej klasy i byl recznie dodawany
            //obu opcji sa minusy i plusy

            if (_drawDebug == null) return;

            // \/ TO TUTAJ JEST NARAIZE DO TESTOW
            foreach (var item in staticObjects)
                _drawDebug.AddDrawable(item);

            foreach (var item in dynamicObjects)
                _drawDebug.AddDrawable(item);

            foreach (var item in entities)
                _drawDebug.AddDrawable(item);
        }
        private void Start()
        {
            //GameConsole.Log($"World manager is setup with currently at scene:\n{staticObjects.Count} static objects \n{dynamicObjects.Count} dynamic objects", "SYSTEM");
        }

        private void Update()
        {
            if (!isWorldUpdating) return;


            //TODO: 0 wiadomo trzeba bedzie po zmergowaniu inventory popracowac nad tym, te rozwiazanie jest tymczasowo
            for (int i = 0; i < ObjectsReadOnly.Count; i++)
            {
                var current = ObjectsReadOnly[i];

                _sqrDistance = (PlayerController.Instance.position - (Vector2)current.transform.position).sqrMagnitude;
                if (_sqrDistance < renderingRadius * renderingRadius && !current.gameObject.activeSelf)
                {
                    current.Show();
                }
                else if (_sqrDistance >= renderingRadius * renderingRadius && current.gameObject.activeSelf)
                {
                    current.Hide();
                }
            }

            for (int i = 0; i < DynamicObjectsReadOnly.Count; i++)
            {
                var current = DynamicObjectsReadOnly[i];

                _sqrDistance = (PlayerController.Instance.position - (Vector2)current.transform.position).sqrMagnitude;
                if (_sqrDistance < renderingRadius * renderingRadius && !current.gameObject.activeSelf)
                {
                    current.Show();
                }
                else if (_sqrDistance >= renderingRadius * renderingRadius && current.gameObject.activeSelf)
                {
                    current.Hide();
                }
            }

            //TODO: culling dla entities
            /*for (int i = 0; i < entities.Count; i++)
            {
                var current = entities[i];

                _sqrDistance = ((Vector2)PlayerController.Instance.position - (Vector2)current.transform.position).sqrMagnitude;
                if (_sqrDistance < renderingRadius * renderingRadius && !current.gameObject.activeSelf)
                {
                    current.Show();
                }
                else if (_sqrDistance >= renderingRadius * renderingRadius && current.gameObject.activeSelf)
                {
                    current.Hide();
                }
            }*/
        }

        public void AddObjectToWorld(SceneObject sceneObject)
        {
            staticObjects.Add(sceneObject);
            _drawDebug.AddDrawable(sceneObject);
        }
        public void RemoveObjectToWorld(SceneObject sceneObject)
        {
            staticObjects.Remove(sceneObject);
            _drawDebug.RemoveDrawable(sceneObject);
        }

        public void AddDynamicObjectToWorld(SceneObject sceneObject)
        {
            dynamicObjects.Add(sceneObject);
            _drawDebug.AddDrawable(sceneObject);
        }
        public void RemoveDynamicObjectToWorld(SceneObject sceneObject)
        {
            dynamicObjects.Remove(sceneObject);
            _drawDebug.RemoveDrawable(sceneObject);
        }

        public void AddEntityToWorld(Entity entity)
        {
            entities.Add(entity);
            _drawDebug.AddDrawable(entity);
        }
        public void RemoveEntityToWorld(Entity entity)
        {
            entities.Remove(entity);
            _drawDebug.RemoveDrawable(entity);
        }
    }
}
