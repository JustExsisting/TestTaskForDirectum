using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingApp
{
    class Meeting
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set;}
        public bool NeedNotify { get; set; }
        public TimeSpan NotificationTime { get; set; }
        
        /// <summary>
        /// Конструктор для создания экземляра встречи
        /// </summary>
        /// <param name="Name">Имя встречи</param>
        /// <param name="StarDate">Дата начала встречи</param>
        /// <param name="EndDate">Дата примерного окончания встречи</param>
        /// <param name="NeedNotify">Нужно ли уведомить</param>
        /// <param name="NotificationTime">Время уведомления о встречи</param>
        public Meeting(string Name, DateTime StarDate, DateTime EndDate, bool NeedNotify, TimeSpan NotificationTime)
        {
            FillMeet(Name, StarDate, EndDate, NeedNotify, NotificationTime);
        }

        /// <summary>
        /// Функция редактирования встречи
        /// </summary>
        /// <param name="Id">Id встречи</param>
        /// <param name="Name">Имя встречи</param>
        /// <param name="StarDate">Дата начала встречи</param>
        /// <param name="EndDate">Дата примерного окончания встречи</param>
        /// <param name="NeedNotify">Нужно ли уведомить</param>
        /// <param name="NotificationTime">Время уведомления о встречи</param>
        public void UpdateMeeting(uint Id, string Name, DateTime StarDate, DateTime EndDate, bool NeedNotify, TimeSpan NotificationTime)
        {
            if (Id == this.Id)
            {
                FillMeet(Name, StarDate, EndDate, NeedNotify, NotificationTime);
            }
            else
            {
                Console.WriteLine("Такая встреча не найдена!");
            }
        }
        /// <summary>
        /// Функция для заполнения встречи
        /// </summary>
        /// <param name="Name">Имя встречи</param>
        /// <param name="StarDate">Дата начала встречи</param>
        /// <param name="EndDate">Дата примерного окончания встречи</param>
        /// <param name="NeedNotify">Нужно ли уведомить</param>
        /// <param name="NotificationTime">Время уведомления о встречи</param>
        private void FillMeet(string Name, DateTime StarDate, DateTime EndDate, bool NeedNotify, TimeSpan NotificationTime)
        {
            //поиск максимального id 
            Id = Program.MeetingsList.OrderByDescending(meeting => meeting.Id).First().Id + 1;
            this.Name = Name.Trim();
            this.StarDate = StarDate;
            this.EndDate = EndDate;
            this.NeedNotify = NeedNotify;
            if (this.NeedNotify == true)
            {
                this.NotificationTime = NotificationTime;
            }
        }
        public override string ToString()
        {
            var needNotify = NeedNotify == true ? "Да" : "Нет";
            var notificationTime = string.Empty;
            if (NeedNotify == true)
            {
                notificationTime = "за " + NotificationTime.ToString();
            }
            return $"{Id}) {Name} : {StarDate} - {EndDate} уведомление: {needNotify} {notificationTime}";
        }
    }
}
