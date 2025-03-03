
namespace DAMS.DatabaseModel.ETMP.INT.DB.Models
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public bool IsSent { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
