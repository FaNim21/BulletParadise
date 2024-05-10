using System.Collections.Generic;

namespace BulletParadise.DataManagement
{
    [System.Serializable]
    public class PortalStats
    {
        public int portalID;

        public int attempts;
        public int deaths;
        public int completions;

        public double timerMiliseconds;


        public PortalStats()
        {
            attempts = 0;
            deaths = 0;
            completions = 0;
            timerMiliseconds = 0d;
        }
    }

    [System.Serializable]
    public class GameData
    {
        public int[] weaponsIdsInSlot;
        public List<PortalStats> portalStats;


        public GameData()
        {
            weaponsIdsInSlot = new int[2];
            weaponsIdsInSlot[0] = -1;
            weaponsIdsInSlot[1] = -1;

            portalStats = new List<PortalStats>();
        }
    }
}
