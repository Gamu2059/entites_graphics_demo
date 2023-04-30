using Unity.Entities;

namespace Gamu2059.EntitiesGraphicsDemo
{
    /// <summary>
    /// キャラ制御システムのグループ.
    /// </summary>
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class SimulationCharaSystemGroup : ComponentSystemGroup
    {
    }
}