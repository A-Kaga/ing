﻿using System;

namespace Sakuno.ING.Game.Models
{
    public readonly struct RepairingDockId : IEquatable<RepairingDockId>, IComparable<RepairingDockId>
    {
        private readonly int value;
        public RepairingDockId(int value) => this.value = value;

        public int CompareTo(RepairingDockId other) => value - other.value;
        public bool Equals(RepairingDockId other) => value == other.value;

        public static implicit operator int(RepairingDockId id) => id.value;
        public static explicit operator RepairingDockId(long value) => new RepairingDockId((int)value);

        public override string ToString() => value.ToString();
    }

    public interface IRawRepairingDock : IIdentifiable<RepairingDockId>
    {
        RepairingDockState State { get; }
        ShipId RepairingShipId { get; }
        DateTimeOffset CompletionTime { get; }
        Materials Consumption { get; }
    }

    public enum RepairingDockState
    {
        Locked = -1,
        Empty = 0,
        Repairing = 1,
    }
}
