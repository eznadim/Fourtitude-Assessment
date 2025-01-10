namespace TransactionAPI.Models.Configuration;

public class PartnerConfiguration
{
    public List<Partner> Partners { get; set; } = new();
}

public class Partner
{
    public string PartnerNo { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

