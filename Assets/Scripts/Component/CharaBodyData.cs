using System;
using Unity.Entities;

namespace Gamu2059.EntitiesGraphicsDemo
{
    /// <summary>
    /// BodyのEntityを保持するためのデータ.
    /// </summary>
    public struct CharaBodyData : IComponentData
    {
        public Entity BodyEntity;
    }
}