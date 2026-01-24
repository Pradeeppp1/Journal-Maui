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
}

public class WordCountTrendPoint
{
    public DateTime Date { get; set; }
    public double AverageWordCount { get; set; }
}
