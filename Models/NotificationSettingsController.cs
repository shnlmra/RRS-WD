using Microsoft.AspNetCore.Mvc;

namespace RRS.Models
{
	public class NotificationSettings
	{
		public bool ReservationsEnabled { get; set; }
		public bool CancellationsEnabled { get; set; }
		public bool SystemUpdatesEnabled { get; set; }
	}
}
