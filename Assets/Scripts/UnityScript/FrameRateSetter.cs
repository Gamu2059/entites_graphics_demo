using UnityEngine;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public class FrameRateSetter : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}