using System.Collections.Generic;

namespace BulletParadise.DataManagement
{
    public class TimerData
    {
        public int hours;
        public int minutes;
        public int seconds;
        public int milliseconds;


        public TimerData()
        {
            hours = 0;
            minutes = 0;
            seconds = 0;
            milliseconds = 0;
        }
    }

    [System.Serializable]
    public class PortalStats
    {
        public int portalID;

        public int attempts;
        public int deaths;
        public int completions;

        public TimerData timer;


        public PortalStats()
        {
            attempts = 0;
            deaths = 0;
            completions = 0;

            timer = new TimerData();
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
