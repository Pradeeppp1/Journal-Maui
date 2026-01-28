using System;
using System.Collections.Generic;

namespace Journal.Models;

public class AnalyticsDto
{
    public Dictionary<string, int> MoodDistribution { get; set; } = new();
    public string MostFrequentMood { get; set; } = "None";
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public List<DateTime> MissedDays { get; set; } = new();
    public Dictionary<string, int> TagUsage { get; set; } = new();
    public List<WordCountTrendPoint> WordCountTrends { get; set; } = new();
    public Dictionary<string, int> ActivityByDayOfWeek { get; set; } = new();
    public Dictionary<int, int> ActivityByHour { get; set; } = new();
    public List<HeatMapPoint> ActivityHeatMap { get; set; } = new();
}

public class HeatMapPoint
{
    public string Name { get; set; } = string.Empty; // Weekday
    public List<HeatMapData> Data { get; set; } = new();
}

public class HeatMapData
{
    public string X { get; set; } = string.Empty; // Week of Month
    public int Y { get; set; } // Entry Count
}

public class WordCountTrendPoint
{
    public DateTime Date { get; set; }
    public double AverageWordCount { get; set; }
}
