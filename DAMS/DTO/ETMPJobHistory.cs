using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMS.DTO
{
    public class ETMPJobHistory
    {
        #region Properties

        /// <summary>
        /// Property JobHistoryID
        /// </summary>
        public Guid JobHistoryID { get; set; }

        /// <summary>
        /// Property JobID
        /// </summary>
        public int JobID { get; set; }

        /// <summary>
        /// Property JobStartDate
        /// </summary>
        public DateTime JobStartDate { get; set; }

        /// <summary>
        /// Property JobEndDate
        /// </summary>
        public DateTime? JobEndDate { get; set; }

        /// <summary>
        /// Property StatusId
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Property Message
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Property CreatedBy
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Property CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Property UpdatedBy
        /// </summary>
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Property UpdatedDate
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Property HangfireJobID
        /// </summary>
        public long HangfireJobID { get; set; }

        /// <summary>
        /// Property JobTitle
        /// </summary>
        public string JobTitle { get; set; } = string.Empty;

        #endregion
    }

}
