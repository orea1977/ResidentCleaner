using Game;
using Game.Common;
using Game.Creatures;
using Game.SceneFlow;
using Game.Tools;
using Unity.Collections;
using Unity.Entities;

namespace ResidentCleaner
{
    public partial class ResidentCleanerSystem : GameSystemBase
    {
        private EntityQuery m_ResidentQuery;
        private bool m_Pending;
        private bool m_DeleteAll;

        // 由選單按鈕呼叫；實際清理在下一次 OnUpdate 於主執行緒執行
        public void RequestClean(bool deleteAll)
        {
            m_DeleteAll = deleteAll;
            m_Pending = true;
            Mod.log.Info($"Clean requested (deleteAll = {deleteAll})");
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            m_ResidentQuery = GetEntityQuery(
                ComponentType.ReadOnly<Resident>(),
                ComponentType.Exclude<Deleted>(),
                ComponentType.Exclude<Temp>());
        }

        protected override void OnUpdate()
        {
            if (!m_Pending) return;
            m_Pending = false;

            if (GameManager.instance.gameMode != GameMode.Game)
            {
                Mod.log.Warn("Not in game, clean skipped. Load a save first.");
                return;
            }

            NativeArray<Entity> residents = m_ResidentQuery.ToEntityArray(Allocator.Temp);
            NativeList<Entity> toDelete = new NativeList<Entity>(residents.Length, Allocator.Temp);

            foreach (Entity e in residents)
            {
                if (m_DeleteAll || IsBroken(e))
                    toDelete.Add(e);
            }

            if (toDelete.Length > 0)
                EntityManager.AddComponent<Deleted>(toDelete.AsArray());

            Mod.log.Info($"ResidentCleaner: checked {residents.Length} residents, deleted {toDelete.Length} (deleteAll = {m_DeleteAll})");

            toDelete.Dispose();
            residents.Dispose();
        }

        private bool IsBroken(Entity e)
        {
            EntityManager em = EntityManager;

            if (em.HasComponent<Resident>(e) &&
                !IsValid(em.GetComponentData<Resident>(e).m_Citizen))
                return true;

            if (em.HasComponent<CurrentVehicle>(e) &&
                !IsValid(em.GetComponentData<CurrentVehicle>(e).m_Vehicle))
                return true;

            if (em.HasComponent<Target>(e) &&
                IsDangling(em.GetComponentData<Target>(e).m_Target))
                return true;

            // 需要 csproj 引用 Colossal.Mathematics；若之前是刪掉這段才編譯成功，這裡也一併刪掉
            if (em.HasComponent<HumanCurrentLane>(e) &&
                IsDangling(em.GetComponentData<HumanCurrentLane>(e).m_Lane))
                return true;

            return false;
        }

        private bool IsValid(Entity x) => x != Entity.Null && EntityManager.Exists(x);

        private bool IsDangling(Entity x) => x != Entity.Null && !EntityManager.Exists(x);
    }
}
