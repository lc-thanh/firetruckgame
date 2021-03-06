﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SessionData", menuName = "Session Data")]
public class SessionData : ScriptableObject {
    public const float GAME_DURATION = 120;

    public int peopleSaved;
    public int peopleDied;
    public int unitsBurned;
    public int unitsExtinguished;
    public int waterUsed;
    public float time;

    public struct Ratings {
        public Rating peopleRating;
        public Rating unitRating;
        public Rating finalRating;
    }

    public enum Rating {
        Bad,
        Average,
        Good,
        Perfect,
    }

    public enum Title {
        None,
        GrimReaper, // > 40 kills
        Arsonist, // < 20 extinguished
        HydroHomie, // > 1100 L water
    }

    public void Reset() {
        peopleSaved = 0;
        peopleDied = 0;
        unitsBurned = 0;
        unitsExtinguished = 0;
        waterUsed = 0;
        time = GAME_DURATION;
    }

    public Ratings GetRating() {
        Ratings ratings = new Ratings();

        if (peopleDied < 6) {
            ratings.peopleRating = Rating.Perfect;
        } else if (peopleDied < 16) {
            ratings.peopleRating = Rating.Good;
        } else if (peopleDied < 25) {
            ratings.peopleRating = Rating.Average; 
        } else {
            ratings.peopleRating = Rating.Bad;
        }

        float extinguishRate = 1f * (unitsExtinguished + 1) / (unitsBurned + 1);
        if (extinguishRate > 0.97f) {
            ratings.unitRating = Rating.Perfect;
        } else if (extinguishRate > 0.92f) {
            ratings.unitRating = Rating.Good;
        } else if (extinguishRate > 0.86f) {
            ratings.unitRating = Rating.Average;
        } else {
            ratings.unitRating = Rating.Bad;
        }

        int totalRating = (int) ratings.peopleRating + (int) ratings.unitRating;
        if (totalRating == 6) {
            ratings.finalRating = Rating.Perfect;
        } else if (totalRating > 3) {
            ratings.finalRating = Rating.Good;
        } else if (totalRating > 0) {
            ratings.finalRating = Rating.Average;
        } else {
            ratings.finalRating = Rating.Bad;
        }

        return ratings;
    }

    public Title GetTitle() {
        if (peopleDied > 40) {
            return Title.GrimReaper;
        } else if (unitsExtinguished < 20) {
            return Title.Arsonist;
        } else if (waterUsed > 1100) {
            return Title.HydroHomie;
        } else {
            return Title.None;
        }
    }

    public float GetGameProgress() {
        return (GAME_DURATION - time) / GAME_DURATION;
    }
}
