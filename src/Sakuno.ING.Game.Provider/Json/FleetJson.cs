﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sakuno.ING.Game.Models;
using Sakuno.ING.Game.Models.MasterData;

namespace Sakuno.ING.Game.Json
{
    internal class FleetJson : IRawFleet
    {
        [JsonProperty("api_id")]
        public FleetId Id { get; set; }
        [JsonProperty("api_name")]
        public string Name { get; set; }

        public long[] api_mission;
        public FleetExpeditionState ExpeditionState => (FleetExpeditionState)api_mission.ElementAtOrDefault(0);
        public ExpeditionId ExpeditionId => (ExpeditionId)api_mission.ElementAtOrDefault(1);

        public DateTimeOffset ExpeditionCompletionTime => DateTimeOffset.FromUnixTimeMilliseconds(api_mission.ElementAtOrDefault(2));

        public ShipId[] api_ship;
        public IReadOnlyList<ShipId> ShipIds => api_ship.Where(x => x > 0).ToArray();
    }
}
