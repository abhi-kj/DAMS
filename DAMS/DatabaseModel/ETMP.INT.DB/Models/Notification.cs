
using System;

namespace DAMS.DatabaseModel.ETMP.INT.DB.Models
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public bool IsSent { get; set; }
        public DateTime CreatedDate { get; set; }
        public int NotificationTemplateId { get; set; }

    }
}

public class UserNotificationExclusion
{
    public Guid UserNotificationExclusionId { get; set; }

    public string? UserWwid { get; set; }

    public int NotificationTemplateId { get; set; }

    public bool? IsActive { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }


}