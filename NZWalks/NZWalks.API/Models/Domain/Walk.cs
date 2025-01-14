﻿namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }

        //Navigation Properties
        //It provides a way to navigate an association between two entity types. Every object can have a navigation property for every relationship in which it participates

        public Region Region { get; set; }  
        public WalkDifficulty WalkDifficulty { get; set; }


    }
}
