using System.Collections.Generic;

public class StackDataComparer : IComparer<StackData>
{
    public int Compare(StackData x, StackData y)
    {
        // by domain
        int domainComparison = string.Compare(x.domain, y.domain);
        if (domainComparison != 0)
            return domainComparison;

        // by cluster
        int clusterComparison = string.Compare(x.cluster, y.cluster);
        if (clusterComparison != 0)
            return clusterComparison;

        // by standardid
        return string.Compare(x.standardid, y.standardid);
    }
}
