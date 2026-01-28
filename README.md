# Journal - Premium Daily Reflection App

A powerful and aesthetically pleasing .NET MAUI Blazor Hybrid application designed for daily journaling, emotional tracking, and data-driven insights.

## ✨ Features

- **Rich Text Editing**: Powered by Quill.js, featuring a clean toolbar for all your formatting needs (Bold, Italic, Lists, Blockquotes).
- **Categorical Mood System**:
  - **Primary Moods**: Positive (😊), Neutral (😐), Negative (😟) for high-level analytics.
  - **Sub-Moods**: Pick up to two specific feelings (e.g., Happy, Calm, Anxious, Stressed) per entry.
- **Advanced Analytics**:
  - **Mood Trends**: Track your emotional journey over time.
  - **Tag Usage**: Visualize your most common topics.
  - **Word Count Trends**: Monitor your writing habits with interactive area charts.
  - **Activity Heartbeats**: Visualize your writing patterns by day of the week and hour of the day.
- **Smart Filtering**: Filter your entries by date range (This Month, Last Month, All Time, or Custom) and mood.
- **Tagging System**: Organize your thoughts with a library of 30+ pre-built tags or create your own.
- **Security First**: Secure your private thoughts with a persistent PIN lock system.
- **Export & Portability**: Export your journal collections to beautifully formatted HTML/PDF files.
- **Responsive Design**: A premium, "glassmorphism" inspired UI that works seamlessly on desktop and mobile.

## 🚀 Getting Started

### Prerequisites

- **.NET 9 SDK**
- **MAUI Workload** (`dotnet workload install maui`)
- **Xcode** (for MacCatalyst builds)

### Installation & Run

1. Clone the repository:

   ```bash
   git clone git@github.com:Pradeeppp1/Journal-Maui.git
   cd Journal-Maui
   ```

2. Restore and Build:

   ```bash
   dotnet restore
   dotnet build -f net9.0-maccatalyst
   ```

3. Run the application:
   ```bash
   dotnet run -f net9.0-maccatalyst
   ```

## 🛠 Tech Stack

- **Framework**: .NET MAUI Blazor Hybrid
- **Language**: C#, Razor, JavaScript
- **Database**: Entity Framework Core (SQLite)
- **Charts**: ApexCharts via JS Interop
- **Text Editor**: Quill.js
- **Exporting**: QuestPDF / HTML-to-PDF

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

_Created by Pradeep Baral_
