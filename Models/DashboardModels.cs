namespace HseDashboard.Models;

public enum RiskStatus { Safe, AtRisk, Critical, Info }
public enum AlertSeverity { Critical, High, Medium, Info }

public record HseTopic(string Icon, string Title, string Subtitle);

public record TwinMarker(double XPercent, double YPercent, RiskStatus Status);

public record OverviewStat(string Label, string Value, string SubLabel, double ChangePercent, bool IsGood);

public record LiveAlert(AlertSeverity Severity, string Title, string Location, string MinutesAgoLabel);

public record KpiTile(string Icon, string Label, string Value, double ChangePercent, string? Note = null);

public record TrendPoint(string Month, int Recordable, int NearMiss, int FirstAid, int MedicalTreatment);

public record CategorySlice(string Label, double Percent, string ColorHex);

public record EnvironmentalStat(string Icon, string Label, string Value, double ChangePercent, bool DownIsGood);

public record RiskMatrixItem(int Rank, string Label, int LikelihoodIndex, int ImpactIndex); // 0=Low,1=Med,2=High

public record DonutSegment(string Label, int Count, int Percent, string ColorHex);

public record EmergencySystem(string Icon, string Label, string Status);

public record CalendarItem(string Date, string Title);

public record DocumentItem(string Title, string DateLabel);

public record IntegrationItem(string Icon, string Label, bool Connected);

public class DashboardSnapshot
{
    public List<HseTopic> Topics { get; set; } = new();
    public List<TwinMarker> TwinMarkers { get; set; } = new();
    public List<OverviewStat> OverviewStats { get; set; } = new();
    public List<LiveAlert> LiveAlerts { get; set; } = new();
    public List<KpiTile> KpiTiles { get; set; } = new();
    public List<TrendPoint> IncidentTrend { get; set; } = new();
    public List<CategorySlice> IncidentCategories { get; set; } = new();
    public int IncidentCategoryTotal { get; set; }
    public List<EnvironmentalStat> EnvironmentalStats { get; set; } = new();
    public List<RiskMatrixItem> RiskMatrix { get; set; } = new();
    public DonutSegment[] PermitStatus { get; set; } = Array.Empty<DonutSegment>();
    public int PermitTotal { get; set; }
    public DonutSegment[] TrainingStatus { get; set; } = Array.Empty<DonutSegment>();
    public int TrainingTotal { get; set; }
    public List<EmergencySystem> EmergencySystems { get; set; } = new();
    public List<CalendarItem> ComplianceCalendar { get; set; } = new();
    public List<DocumentItem> Documents { get; set; } = new();
    public List<IntegrationItem> SystemIntegrations { get; set; } = new();
    public List<IntegrationItem> ConnectedSystemsFooter { get; set; } = new();
    public DateTime LastUpdated { get; set; }
}
