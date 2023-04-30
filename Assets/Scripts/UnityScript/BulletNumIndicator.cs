using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public class BulletNumIndicator : UIBehaviour
    {
        [SerializeField]
        private Text bulletNumText;

        [SerializeField]
        private Text particleNumText;

        [SerializeField]
        private float updateInterval;

        private float _currentTime;
        private List<int> _bulletNumRecords = new();
        private List<int> _particleNumRecords = new();

        protected override void OnEnable()
        {
            BulletCountSystem.OnCountedBulletNum += RecordBulletNum;
            ParticleCountSystem.OnCountedParticleNum += RecordParticleNum;
        }

        protected override void OnDisable()
        {
            ParticleCountSystem.OnCountedParticleNum -= RecordParticleNum;
            BulletCountSystem.OnCountedBulletNum -= RecordBulletNum;
        }

        private void Update()
        {
            if (bulletNumText == null) return;

            if (_currentTime > 0f)
            {
                _currentTime -= Time.deltaTime;
                return;
            }

            _currentTime = updateInterval;
            UpdateBulletNumText();
            UpdateParticleNumText();
        }

        private void RecordBulletNum(int bulletNum)
        {
            _bulletNumRecords?.Add(bulletNum);
        }

        private void RecordParticleNum(int particleNum)
        {
            _particleNumRecords?.Add(particleNum);
        }

        private void UpdateBulletNumText()
        {
            if (_bulletNumRecords == null) return;

            if (_bulletNumRecords.Count <= 0)
            {
                bulletNumText.text = "0";
                return;
            }

            var numAvg = (float)_bulletNumRecords.Sum(r => r) / _bulletNumRecords.Count;
            _bulletNumRecords.Clear();
            bulletNumText.text = numAvg.ToString("F0");
        }

        private void UpdateParticleNumText()
        {
            if (_particleNumRecords == null) return;

            if (_particleNumRecords.Count <= 0)
            {
                particleNumText.text = "0";
                return;
            }

            var numAvg = (float)_particleNumRecords.Sum(r => r) / _particleNumRecords.Count;
            _particleNumRecords.Clear();
            particleNumText.text = numAvg.ToString("F0");
        }
    }
}