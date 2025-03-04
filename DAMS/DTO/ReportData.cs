using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMS.DTO
{
    public class ReportData
    {
        public MailReportData MailReportData { get; set; } = new MailReportData();
        public SyncReportData SyncReportData { get; set; } = new SyncReportData();
    }
    public class MailReportData
    {
        public int MailSuccessCount { get; set; }
        public int MailFailCount { get; set; }
        public int SkipCount { get; internal set; }
    }
    public class SyncReportData
    {
        public int SyncSuccessCount { get; set; }
        public int SyncFailCount { get; set; }

    }
}
