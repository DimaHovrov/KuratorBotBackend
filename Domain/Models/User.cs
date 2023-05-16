using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string PhoneNumber { get; set; }
        public UInt64 TelegramId { get; set; }
        public string TelegramLogin { get; set; }
        public UInt64 GroupsId { get; set; }
        public bool IsConnectionOnBot { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsStudent { get; set; }
        public bool IsTutor { get; set; }
    }
}
