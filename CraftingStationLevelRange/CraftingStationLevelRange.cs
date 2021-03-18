using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace CraftingStationLevelRange
{
    [BepInPlugin("smallo.mods.craftingstationlevelrange", "Crafting Station Level Range", "1.5.0")]
    [HarmonyPatch]
    class CraftingStationLevelRangePlugin : BaseUnityPlugin
    {
        private static ManualLogSource logger;
        private static ConfigEntry<bool> enableMod;
        private static ConfigEntry<int> stationAmountIncrease;
        private static ConfigEntry<int> stationDefaultRange;
        private static ConfigEntry<double> stationSearchRange;
        private static ConfigEntry<bool> parentInheritance;
        private static ConfigEntry<double> inheritanceAmount;

        void Awake()
        {
            logger = Logger;

            enableMod = Config.Bind("1 - Global", "EnableMod", true, "Enable or disable this mod");
            stationDefaultRange = Config.Bind("2 - General", "DefaultRange", 20, "The range of a level 1 workbench (vanilla game is 20)");
            stationAmountIncrease = Config.Bind("2 - General", "IncreaseAmount", 10, "Amount to increase the station by for each level");
            stationSearchRange = Config.Bind("3 - Advanced", "SearchRange", 120.0, "The range to search for crafting stations while holding a hammer. Warning: larger numbers may potentially cause lag, only increase the number if your stations will ever get above this build range.");
            parentInheritance = Config.Bind("4 - Range Inheritance", "ParentInheritance", true, "Allow secondary Crafting Stations (Stonecutter or Artisans Table) to inherit the range of their parent stations (Workbench or Forge) if they are within range");
            inheritanceAmount = Config.Bind("4 - Range Inheritance", "InheritanceAmount", 1.0, "Amount to multiply the inheritance value by. You may want the secondary station to have a lesser value. (Example: 0.5 would be 50% the amount of the IncreaseAmount variable)");

            if (!enableMod.Value) return;

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CraftingStation), "UpdateKnownStationsInRange")]
        public static void LogStations(Player player)
        {
            foreach (CraftingStation station in CraftingStation.m_allStations)
            {
                int stationLevel = station.GetLevel();

                if (stationLevel > 1)
                {
                    int newRange = stationDefaultRange.Value + (stationAmountIncrease.Value * (stationLevel - 1));
                    ChangeStationRange(station, newRange);
                    continue;
                }
                if (parentInheritance.Value)
                {
                    if (station.m_name == "$piece_stonecutter" || station.m_name == "$piece_artisanstation") ChangeChildStationRange(station);
                }
                if (station.m_name == "$piece_workbench" || station.m_name == "$piece_forge") ChangeStationRange(station, stationDefaultRange.Value);
            }
        }

        public static (bool, CraftingStation) IsParentWorkbenchInRange(CraftingStation station, string workbenchType, float searchRange)
        {
            CraftingStation closestStation = CraftingStation.FindClosestStationInRange(workbenchType, station.transform.position, searchRange);
            if (closestStation == null) return (false, closestStation);

            Vector2 closestStationVector = new Vector2(closestStation.transform.position.x, closestStation.transform.position.z);
            Vector2 stationVector = new Vector2(station.transform.position.x, station.transform.position.z);

            if (Vector2.Distance(closestStationVector, stationVector) <= closestStation.m_rangeBuild) return (true, closestStation);
            return (false, closestStation);
        }

        public static void ChangeStationRange(CraftingStation station, float newRange)
        {
            if (station.m_rangeBuild == newRange) return;

            CraftingStation netStation = station.GetComponent<ZNetView>().GetComponent<CraftingStation>();
            netStation.m_rangeBuild = newRange;
            netStation.m_areaMarker.GetComponent<CircleProjector>().m_radius = newRange;
            netStation.m_areaMarker.GetComponent<CircleProjector>().m_nrOfSegments = (int)Math.Ceiling(Math.Max(5f, 4f * newRange));

            logger.LogInfo($"{station.m_name} ({station.GetInstanceID()}) set to {newRange} range");
        }

        public static void ChangeChildStationRange(CraftingStation station)
        {
            (bool isStationInRange, CraftingStation closestStation) = IsParentWorkbenchInRange(station, "$piece_workbench", (float) stationSearchRange.Value);
            if (!isStationInRange) ChangeStationRange(station, stationDefaultRange.Value);
            if (isStationInRange && closestStation.GetLevel() > 1)
            {
                float newRange = stationDefaultRange.Value + (((float) stationAmountIncrease.Value * (closestStation.GetLevel() - 1) * (float) inheritanceAmount.Value));
                ChangeStationRange(station, newRange);
                return;
            }
            if (isStationInRange && closestStation.GetLevel() == 1) ChangeStationRange(station, stationDefaultRange.Value);
        }
    }
}