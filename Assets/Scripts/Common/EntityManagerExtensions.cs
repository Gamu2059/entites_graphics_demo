using Unity.Entities;
using Unity.Transforms;

namespace Gamu2059.EntitiesGraphicsDemo
{
    public static class EntityManagerExtensions
    {
        /// <summary>
        /// Entityを再起的に削除するコマンドを作成する.
        /// </summary>
        public static void DestroyEntityRecursive(this EntityManager em, EntityCommandBuffer ecb, Entity e)
        {
            if (e == Entity.Null) return;

            if (em.HasBuffer<Child>(e))
            {
                var children = em.GetBuffer<Child>(e);
                foreach (var child in children)
                {
                    em.DestroyEntityRecursive(ecb, child.Value);
                }
            }

            ecb.DestroyEntity(e);
        }
    }
}