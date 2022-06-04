using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json.Schema;

namespace spawn_respawn_script
{
    public class client : BaseScript
    {
        public client()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(InitialSpawn);
            Tick += RespawnPlayerNearby;

        }

        public async Task Resurrect()
        {
            await BaseScript.Delay(3000);
            if (Game.PlayerPed.IsDead)
            {
                API.ResurrectPed(Game.PlayerPed.Handle);
            };
        }

        private async Task RespawnPlayerNearby()
        {
            if (Game.PlayerPed.IsDead)
            {
                JToken config = JToken.Parse(API.LoadResourceFile(API.GetCurrentResourceName(), "config.json"));



                int delay = ((int)config["respawnDelay"]);


                await BaseScript.Delay(delay);




                Vector3 playerLoc = Game.PlayerPed.Position;

                Debug.WriteLine("\n Player Location: " + playerLoc);

                List<int> numbers = new List<int>() { 1, -1 };

                Random rnd = new Random();
                int randIndex = rnd.Next(numbers.Count);
                int random = numbers[randIndex];


                int configOffset = ((int)config["respawnDistance"]);

    

                Debug.WriteLine("\n Config Offset: " + configOffset);

                int officialOffsetX = configOffset * random;
                int officialOffsetY = configOffset * random;
                int officialOffsetZ = configOffset * random;

                Debug.WriteLine("\n officialOffsetX: " + officialOffsetX);
                Debug.WriteLine("\n officialOffsetY: " + officialOffsetY);
                Debug.WriteLine("\n officialOffsetZ: " + officialOffsetZ);
                API.DoScreenFadeOut(2000);
                
                Vector3 sidewalk = World.GetNextPositionOnSidewalk(new Vector2(playerLoc.X + officialOffsetX, playerLoc.Y + officialOffsetY));
                Game.PlayerPed.Position = new Vector3(sidewalk.X, sidewalk.Y, sidewalk.Z + 5f);
                API.ResurrectPed(Game.PlayerPed.Handle);
                API.DoScreenFadeOut(2000);


            }

        }

        private async void InitialSpawn(string resourceName)
        {
            if (API.GetCurrentResourceName() == resourceName)
            {
                JToken config = JToken.Parse(API.LoadResourceFile(API.GetCurrentResourceName(), "./config.json"));

                //coords:
                JToken initialCoords = config["initialSpawnLoc"];
                int initialX = ((int)initialCoords["X"]);
                int initialY = ((int)initialCoords["Y"]);
                int initialZ = ((int)initialCoords["Z"]);

                Game.PlayerPed.Position = new Vector3(initialX, initialY, initialZ);

            }

        }
    }
}
