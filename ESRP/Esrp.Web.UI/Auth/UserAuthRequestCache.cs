using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esrp.Web.Auth
{
	public class UserAuthRequestCache
	{
		private static UserAuthRequestCache _cache;
		public static UserAuthRequestCache Cache
		{
			get
			{
				if(_cache == null)
					_cache = new UserAuthRequestCache();
				return _cache;
			}
		}

		private class StoredData
		{
			public Guid Ticket { get; set; }
			public int SystemId { get; set; }

			public StoredData(Guid ticket, int systemId)
			{
                Ticket = ticket;
                SystemId = systemId;
			}
		}

        /// <summary>
        /// Храним логин пользователя и все системы в которые он был залогинен
        /// </summary>
        private readonly Dictionary<string, List<StoredData>> _userTickets = new Dictionary<string, List<StoredData>>();

        /// <summary>
        /// Регистрируем пользователя
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public Guid AddUser(string userLogin, int systemId)
        {
            lock (_userTickets)
            {
                /* Регистрируем для пользователя другой тикет */
                var ticket = Guid.NewGuid();
                if (!_userTickets.ContainsKey(userLogin))
                {
                    _userTickets[userLogin] = new List<StoredData> { new StoredData(ticket, systemId) };
                }
                else
                {
                    var data = _userTickets[userLogin].SingleOrDefault(c => c.SystemId == systemId);
                    if (data == null)
                        _userTickets[userLogin].Add(new StoredData(ticket, systemId));
                    else
                        data.Ticket = ticket;
                }

                return ticket;
            }
        }

        /// <summary>
        /// Проверяем - можно ли пользователю заходить в указанную систему с таким билетом
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="ticket"></param>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public bool CheckUserTicket(string userLogin, Guid ticket, int systemId)
		{
            lock (_userTickets)
            {
                if (!_userTickets.ContainsKey(userLogin))
                    return false;

                return _userTickets[userLogin].SingleOrDefault(c => c.SystemId == systemId && c.Ticket == ticket) != null;
            }
		}
	}
}