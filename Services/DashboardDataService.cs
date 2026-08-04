using HseDashboard.Models;

namespace HseDashboard.Services;

/// <summary>
/// Owns the current dashboard snapshot and simulates the "real-time sync"
/// feature by nudging a few numbers every few seconds and raising OnChange.
/// In production, replace the timer tick with a SignalR hub subscription
/// or a periodic HttpClient poll against your HSE backend/IoT gateway.
/// </summary>
public class DashboardDataService : IDisposable
{
    private readonly Timer _timer;
    private readonly Random _rng = new();

    public DashboardSnapshot Snapshot { get; private set; }
    public event Action? OnChange;

    public DashboardDataService()
    {
        Snapshot = BuildInitialSnapshot();
        _timer = new Timer(_ => Tick(), null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
    }

    private void Tick()
    {
        // Simulate small live fluctuations so the "Real-time Sync" badge feels alive.
        Snapshot.LastUpdated = DateTime.Now;

        var obs = Snapshot.OverviewStats.FirstOrDefault(s => s.Label == "Safety Observations");
        if (obs is not null)
        {
            var idx = Snapshot.OverviewStats.IndexOf(obs);
            var newVal = int.Parse(obs.Value.Replace(",", "")) + _rng.Next(0, 3);
            Snapshot.OverviewStats[idx] = obs with { Value = newVal.ToString("N0") };
        }

        OnChange?.Invoke();
    }

    private static DashboardSnapshot BuildInitialSnapshot() => new()
    {
        LastUpdated = DateTime.Now,

        Topics = new()
        {
            new("🛡️", "HSE General Requirements", "Compliance • Legal • Reporting"),
            new("⚠️", "Hazard Identification & Risk Assessment", "Assessments • Controls • Records"),
            new("🎓", "Training & Competence", "Training • Evaluation • Certification"),
            new("🧪", "Chemical & Substance", "Risk Assessment • SDS • Labeling"),
            new("⚙️", "Machine & Equipment Safety", "Pre-Req • DAP • Evaluation • Release"),
            new("🌿", "Environmental Protection", "Waste • Emissions • Disposal • Legal"),
            new("☣️", "Dangerous Goods", "Transport • Storage • Handling"),
            new("👷", "Contractor Management", "Pre-qualification • Work Control"),
            new("🧯", "Fire Protection & Emergency", "Prevention • Preparedness • Response"),
            new("🏢", "Business Continuity", "Safety • Security • IT • Continuity"),
            new("📦", "Material Compliance", "Product • Substance • Legal"),
            new("🌍", "ESG & Sustainability", "CO2 • Water • Waste • DEI • Governance"),
        },

        TwinMarkers = new()
        {
            new(28, 22, RiskStatus.Safe),
            new(50, 20, RiskStatus.Safe),
            new(60, 30, RiskStatus.Critical),
            new(38, 38, RiskStatus.AtRisk),
            new(65, 42, RiskStatus.AtRisk),
            new(46, 50, RiskStatus.Info),
            new(30, 58, RiskStatus.Safe),
            new(55, 62, RiskStatus.Safe),
            new(48, 70, RiskStatus.AtRisk),
        },

        OverviewStats = new()
        {
            new("Safety Observations", "2,453", "", 18, true),
            new("Near Misses", "1,987", "", 21, true),
            new("Recordable Incidents", "32", "", -27, true),
            new("Medical Treatment", "15", "", -12, true),
            new("First Aid Cases", "46", "", -8, true),
            new("Environmental Incidents", "4", "", -20, true),
            new("Days Since Last", "124", "Recordable Incident", 0, true),
            new("Permit Compliance", "98%", "", 5, true),
            new("Training Compliance", "92%", "", 6, true),
        },

        LiveAlerts = new()
        {
            new(AlertSeverity.Critical, "Gas Leak Detected", "Utility Area", "2 min ago"),
            new(AlertSeverity.High, "Machine Guard Open", "Press Line 3", "5 min ago"),
            new(AlertSeverity.Medium, "Chemical Storage Temp High", "Storehouse 2", "12 min ago"),
            new(AlertSeverity.Medium, "PPE Non-Compliance", "Assembly Line 1", "18 min ago"),
            new(AlertSeverity.Info, "Permit Expiring Soon", "Confined Space #12", "25 min ago"),
        },

        KpiTiles = new()
        {
            new("📈", "Safety Culture Index", "87%", 7, "Target ≥ 85%"),
            new("🦺", "PPE Compliance", "94%", 4),
            new("✅", "Observation Closure Rate", "91%", 6),
            new("📋", "Permit-to-Work Compliance", "98%", 5),
            new("⚠️", "Risk Assessment Closure", "89%", 3),
            new("🚨", "Emergency Drill Compliance", "93%", 6),
            new("👷", "Contractor Safety Score", "90%", 5),
            new("📊", "Audit Score", "94%", 6),
        },

        IncidentTrend = new()
        {
            new("Jan", 3, 12, 5, 2), new("Feb", 2, 15, 6, 1), new("Mar", 4, 18, 4, 2),
            new("Apr", 3, 14, 7, 1), new("May", 2, 20, 5, 2), new("Jun", 3, 22, 6, 1),
            new("Jul", 2, 19, 5, 1), new("Aug", 3, 21, 6, 2), new("Sep", 2, 17, 4, 1),
            new("Oct", 3, 23, 6, 2), new("Nov", 2, 20, 5, 1), new("Dec", 3, 18, 4, 1),
        },

        IncidentCategoryTotal = 32,
        IncidentCategories = new()
        {
            new("Slips, Trips & Falls", 31, "#e53935"),
            new("Machine / Equipment", 28, "#fb8c00"),
            new("Material Handling", 16, "#fdd835"),
            new("Chemical Exposure", 9, "#43a047"),
            new("Electrical", 8, "#1e88e5"),
            new("Others", 8, "#8e24aa"),
        },

        EnvironmentalStats = new()
        {
            new("💨", "CO2 Emissions (t)", "1,245", -8, true),
            new("💧", "Water Usage (m³)", "8,450", -6, true),
            new("🗑️", "Waste Generated (t)", "245", -7, true),
            new("♻️", "Waste Recycled (%)", "76%", 6, true),
            new("⚡", "Energy Use (MWh)", "5,420", -5, true),
            new("☣️", "Spill Incidents", "1", -50, true),
        },

        RiskMatrix = new()
        {
            new(1, "Machine Guard Open", 2, 2),
            new(2, "Chemical Exposure", 1, 2),
            new(3, "Forklift Operation", 2, 1),
            new(4, "Working at Height", 1, 1),
            new(5, "Electrical Hazard", 2, 2),
        },

        PermitTotal = 128,
        PermitStatus = new DonutSegment[]
        {
            new("Approved", 86, 67, "#43a047"),
            new("Active", 28, 22, "#fdd835"),
            new("Expiring Soon", 10, 8, "#fb8c00"),
            new("Expired", 4, 3, "#e53935"),
        },

        TrainingTotal = 1235,
        TrainingStatus = new DonutSegment[]
        {
            new("Completed", 1154, 92, "#43a047"),
            new("Pending", 81, 6, "#fdd835"),
            new("Overdue", 20, 2, "#e53935"),
        },

        EmergencySystems = new()
        {
            new("🧯", "Fire Systems", "OK"),
            new("🚿", "Eyewash Units", "OK"),
            new("🚪", "Emergency Exits", "OK"),
            new("🩹", "First Aid Kits", "OK"),
            new("👥", "Assembly Points", "OK"),
        },

        ComplianceCalendar = new()
        {
            new("10 JUN", "ISO 14001 Internal Audit"),
            new("18 JUN", "Fire Drill"),
            new("25 JUN", "Chemical Handling Training"),
            new("30 JUN", "Legal Compliance Review"),
        },

        Documents = new()
        {
            new("HSE Dashboard Report", "May 2026"),
            new("Incident Summary Report", "May 2026"),
            new("Permit Register", "May 2026"),
            new("Waste Manifest", "May 2026"),
            new("Audit Report", "May 2026"),
        },

        SystemIntegrations = new()
        {
            new("🖥️", "SAP EHS", true),
            new("📦", "MES", true),
            new("🏭", "WMS", true),
            new("📡", "IoT Sensors", true),
            new("☀️", "Weather API", true),
        },

        ConnectedSystemsFooter = new()
        {
            new("🖥️", "EHS Platform", true),
            new("📦", "MES", true),
            new("🏭", "WMS", true),
            new("📡", "IoT Sensors", true),
            new("🔐", "Access Control", true),
            new("☀️", "Weather API", true),
            new("🛰️", "SCADA", true),
        },
    };

    public void Dispose() => _timer.Dispose();
}
