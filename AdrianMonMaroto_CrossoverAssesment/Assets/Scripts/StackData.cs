public class StackData
{
    public int id { get; set; }
    public string subject { get; set; }
    public string grade { get; set; }
    public int mastery { get; set; }
    public string domainid { get; set; }
    public string domain { get; set; }
    public string cluster { get; set; }
    public string standardid { get; set; }
    public string standarddescription { get; set; }

    public override string ToString()
    {
        return $"Id: {id}, Subject: {subject}, Grade: {grade}, Mastery: {mastery}, DomainId: {domainid}," +
            $"Domain: {domain}, Cluster: {cluster}, StandardId: {standardid}, StandardDescription: {standarddescription}";
    }
}
