namespace MyTelegramBot.View;
internal class Chart
{
    public static string DisplayBarVerticalChart(List<int> values)
    {
        string chartUrl = "https://chart.googleapis.com/chart?";
        chartUrl += "cht=bvg";            // Chart type: Bar Vertical
        chartUrl += "&chs=500x300";       // Chart size: 500x300 pixels
        chartUrl += $"&chd=t:{string.Join(',', values)}";// Chart data
        chartUrl += "&chl=";               // Chart labels
        string[] labels = { "Jan.", "Feb.", "März", "April", "Mai", "Juni", "Juli", "Aug.", "Sept.", "Okt.", "Nov.", "Dez." };
        chartUrl += string.Join("|", labels.Select(Uri.EscapeDataString));
        chartUrl += "&chdl=";              // Data labels
        string[] dataLabels = { string.Join('|', values)};
        chartUrl += string.Join("|", dataLabels);
        // Specify custom colors for bars using chco parameter
        chartUrl += "&chco=FF8000|00FF00|0000FF|FF00FF|FF0000|00FFFF|9900CC|FFFF00|FF6600|33CC33|663366|0099CC";
        chartUrl += "&chds=0,500"; // X, Y Axes

        return chartUrl;
    }
}
