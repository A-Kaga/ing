﻿namespace Sakuno.KanColle.Amatsukaze.Game.Models.MasterData
{
    partial class ExpeditionInfo
    {
        partial void UpdateCore(IRawExpeditionInfo raw)
        {
            MapArea = mapAreaInfoTable.TryGetOrDummy(raw.MapAreaId);
        }
    }
}
