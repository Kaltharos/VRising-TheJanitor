using ProjectM;
using ProjectM.Network;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace AutoCloseDoors.Utils
{
    public static class Helper
    {
        public static PrefabGUID GetPrefabGUID(Entity entity)
        {
            var entityManager = Plugin.Server.EntityManager;
            PrefabGUID guid;
            try
            {
                guid = entityManager.GetComponentData<PrefabGUID>(entity);
            }
            catch
            {
                guid.GuidHash = 0;
            }
            return guid;
        }

        public static string GetPrefabName(PrefabGUID hashCode)
        {
            var s = Plugin.Server.GetExistingSystem<PrefabCollectionSystem>();
            string name = "Nonexistent";
            if (hashCode.GuidHash == 0)
            {
                return name;
            }
            try
            {
                name = s.PrefabNameLookupMap[hashCode].ToString();
            }
            catch
            {
                name = "NoPrefabName";
            }
            return name;
        }
    }
}