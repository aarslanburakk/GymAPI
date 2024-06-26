﻿using System.Text.Json.Serialization;

namespace RuxGymAPI.Models
{
    public class PlayerPremium
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsPremium { get; set; } = false;
        public string? EndPremiumDay { get; set; } = null;
        [JsonIgnore]
        public Guid PlayerId { get; set; }
        [JsonIgnore]
        public Player Player { get; set; }
    }
}
