﻿using BulletParadise.DataManagement;
using BulletParadise.Datas;
using BulletParadise.Player;
using BulletParadise.World.Components;
using TMPro;
using UnityEngine;

namespace BulletParadise.Entities
{
    public class Portal : MonoBehaviour, IInteractable, ISavable
    {
        public bool IsFocused { get; set; }

        private SpriteRenderer _spriteRenderer;
        private PortalsController _controller;
        private TextMeshPro _textMeshPro;

        public PortalData data;
        public PortalStats stats;
        public bool canEnter = true;


        private void Awake()
        {
            _controller = GetComponentInParent<PortalsController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _textMeshPro = GetComponentInChildren<TextMeshPro>();

            _spriteRenderer.sprite = data.sprite;
            _textMeshPro.SetText(data.name);
        }

        public void Focus()
        {
            IsFocused = true;
            _controller.ShowPortalWindow(this);
        }
        public void LostFocus()
        {
            IsFocused = false;
            _controller.HidePortalWindow();
        }

        public void Interact(PlayerController player)
        {
            if (!canEnter) return;
            _controller.EnterPortal();
        }

        public void Save(GameData gameData)
        {
            for (int i = 0; i < gameData.portalStats.Count; i++)
            {
                if (gameData.portalStats[i].portalID == data.id)
                {
                    gameData.portalStats[i] = stats;
                    return;
                }
            }

            stats.portalID = data.id;
            gameData.portalStats.Add(stats);
        }
        public void Load(GameData gameData)
        {
            for (int i = 0; i < gameData.portalStats.Count; i++)
            {
                if (gameData.portalStats[i].portalID == data.id)
                {
                    stats = gameData.portalStats[i];
                    return;
                }
            }
        }
    }
}
