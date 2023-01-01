using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace BeeHiveManagementSystem
{
    static class HoneyVault
    {
        public const float NECTAR_CONVERSION_RATIO = .19f;
        public const float LOW_LEVEL_WARNING = 10f;
        private static float honey = 25f;
        private static float nectar = 100f;

        public static string StatusReport
        {
            get 
            {
                string returnString;
                returnString =  $"HONEY: {honey:0.0}\nNECTAR: {nectar:0.0}";
                if (honey < LOW_LEVEL_WARNING)
                    returnString += "\nLOW HONEY - ADD A HONEY MANUFACTURER";
                if (nectar < LOW_LEVEL_WARNING)
                    returnString += "\nLOW NECTAR - ADD A NECTAR COLLECTOR";

                return returnString;
            }
        }


        public static void CollectNectar(float amount) 
        {
            if (amount > 0)
            {
                nectar += amount;
            }
        }

        public static void ConverNectarToHoney(float amount) 
        {
            float amountToTrans = amount;

            if (amount > nectar)
            {
                amountToTrans = nectar;
            }

            nectar -= amountToTrans;
            honey += amountToTrans * NECTAR_CONVERSION_RATIO;
        }

        public static bool ConsumeHoney(float amount) 
        {
            if (amount <= honey)
            {
                honey -= amount;
                return true;
            }
            return false;
                
        }
    }
}
