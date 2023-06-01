using MeetingApp;
using System.Globalization;
using System.Runtime;
using System.Threading;
using System.Xml.Linq;

class Program
{
    static public Thread thread { get; set; }
    public static List<Meeting> MeetingsList = new List<Meeting>();

    public static void Notifier()
    {
        while (true)
        {
            var tmpList = MeetingsList.Where(meeting => meeting.NeedNotify == true).ToList();
            //проверка есть ли встречи о которых нужно напомнить
            if (tmpList.Count() > 0)
            {
                foreach (var item in tmpList)
                {
                    var notifyTime = item.StarDate - item.NotificationTime;
                    //из начала вычеть за сколько нужно напомнить и если равно сейчас, то отправить уведомление
                    if (notifyTime.AddSeconds(5) >= DateTime.Now && notifyTime < DateTime.Now)
                    {
                        Console.WriteLine($"!!!УВЕДОМЛЕНИЕ!!!\n{item}\n\"!!!УВЕДОМЛЕНИЕ!!!");
                    }
                }
            }
        }
    }

    static void Main(string[] args)
    {
        thread = new Thread(Notifier);
        thread.IsBackground = true;
        thread.Start();
        var isOpen = true;
        while (isOpen == true)
        {
            Console.Clear();
            Console.WriteLine(
                "Меню:\n" +
                "1 - Управление встречами\n" +
                "2 - Просмотр встреч\n" +
                "ESC - Выход");
            ConsoleKeyInfo pressedKeyMainMenu = Console.ReadKey();
            //управление программой
            switch (pressedKeyMainMenu.Key)
            {
                case ConsoleKey.D1: //Управление встречами
                    Console.Clear();
                    Console.WriteLine(
                        "Вы вошли в управление встречами:\n" +
                        "1 - Добавить новую встречу\n" +
                        "2 - Изменить уже имеющуюся встречу\n" +
                        "3 - Удалить встречу\n" +
                        "ESC - выход");
                var pressedKeyManage = Console.ReadKey();
                switch (pressedKeyManage.Key)
                {
                    case ConsoleKey.D1://Добавить новую встречу
                    {
                        var tmpMeeting = CreateMeeting();
                        if(tmpMeeting != null)
                        {
                            //не должно быть пересекающихся встреч
                            if (MeetingsList.Where(oldMeeting => CheckingCorrectnessTime(oldMeeting, tmpMeeting)).ToList().Count == 0)
                            {
                                MeetingsList.Add(tmpMeeting);
                            }
                            else
                            {
                                Console.WriteLine("Встречи пересекаются!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Дата конца встречи не может быть раньше начала!");
                            Console.ReadKey();
                        }
                        break;

                    }
                    case ConsoleKey.D2://Изменить уже имеющуюся встречу
                    {
                        List<Meeting> tmpList;
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("Укажите дату встречи, которую необходимо изменить в формате ГГГГ.ММ.ДД ЧЧ:ММ:");
                            var tmp = Console.ReadLine();
                            if (tmp != null && DateTime.TryParse(tmp.Trim(), out DateTime needDate) == true)
                            {
                                tmpList = MeetingsList.Where(meeting => meeting.StarDate.Date == needDate.Date).ToList();
                                Console.Clear();
                                Console.WriteLine("Укажите Id встречи которую нужно изменить");
                                foreach (var meeting in tmpList)
                                {
                                    Console.Clear();
                                    Console.WriteLine(meeting.ToString());
                                }
                                var idStr = Console.ReadLine();
                                if (idStr != null && uint.TryParse(idStr, out uint id) && MeetingsList.Count > 0 && MeetingsList.Contains(MeetingsList.Where(meeting => meeting.Id == id).First()))
                                {
                                    MeetingsList.Remove(MeetingsList.Where(meeting => meeting.Id == id).First());
                                    var meeting = CreateMeeting();
                                    if(meeting != null) 
                                    { 
                                        //не должно быть пересекающихся встреч
                                        if (MeetingsList.Where(oldMeeting => CheckingCorrectnessTime(oldMeeting, meeting)).ToList().Count == 0)
                                        {
                                            MeetingsList.Add(meeting);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Встречи пересекаются!");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Дата конца встречи не может быть раньше начала!");
                                        Console.ReadKey();
                                    }
                                    Console.WriteLine($"Запись с Id: {id} была изменина\nПрисвоен новый Id: {meeting.Id}");
                                }
                            }
                            break;
                        }
                        break;
                    }
                    case ConsoleKey.D3://Удалить встречу
                    {
                        Console.Clear();
                        Console.WriteLine("Укажите Id встречи которую нужно удалить");
                        var tmp = Console.ReadLine();
                        if(tmp != null)
                        {
                            if (tmp != null && uint.TryParse(tmp, out uint id) == true && MeetingsList.Count > 0 && MeetingsList.Contains(MeetingsList.Where(meeting => meeting.Id == id).First()) == true)
                            {
                                if (MeetingsList.Remove(MeetingsList.Where(meeting => meeting.Id == id).First()) == true)
                                {
                                    Console.WriteLine($"Запись с Id: {id} была удалена");
                                }
                            }
                        }
                        break; ;
                    }
                }
                break;
                case ConsoleKey.D2://Просмотр встреч
                {
                    Console.Clear();
                    Console.WriteLine(
                        "Вы вошли в просмотр встреч:\n" +
                        "1 - Посмотреть все встречи\n" +
                        "2 - Посмотреть встечи назначенные на конкретный день\n" +
                        "3 - Сформировать файл встреч за конкретный день\n" +
                        "ESC - выход");
                    var pressedKeyWatch = Console.ReadKey();
                    switch (pressedKeyWatch.Key)
                    {
                        case ConsoleKey.D1://Посмотреть все встречи
                        {
                            Console.Clear();
                            foreach (var meeting in MeetingsList)
                            {
                                Console.WriteLine(meeting.ToString());
                            }
                            Console.ReadKey();
                            break;
                        }
                        case ConsoleKey.D2://Посмотреть встечи назначенные на конкретный день
                        {
                            Console.Clear();
                            Console.WriteLine("Укажите дату назныченных встреч в формате ГГГГ.ММ.ДД");
                            var tmp = Console.ReadLine();
                            if (tmp != null && DateTime.TryParse(tmp, out DateTime startDate) == true)
                            {
                                var tmpList = MeetingsList.Where(meeting => meeting.StarDate.Date == startDate.Date).ToList();
                                if (tmpList.Count > 0)
                                {
                                    foreach(var meeting in tmpList)
                                    {
                                        Console.WriteLine(meeting.ToString());
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Нет встреч на эту дату\nНажмите любую клавишу");
                                    Console.ReadKey();
                                }
                            }
                            break;
                        }
                        case ConsoleKey.D3://Сформировать файл встреч за конкретный день
                        {
                            Console.Clear();
                            Console.WriteLine("Укажите дату назныченных встреч в формате ГГГГ.ММ.ДД");
                            var tmp = Console.ReadLine();
                            if (tmp != null && DateTime.TryParse(tmp, out DateTime startDate) == true && MeetingsList.Count > 0 && MeetingsList.Contains(MeetingsList.Where(meeting => meeting.StarDate.Date == startDate.Date).First()) == true)
                            {
                                var tmpList = MeetingsList.Where(meeting => meeting.StarDate.Date == startDate.Date).ToList();
                                if (tmpList.Count > 0)
                                {
                                    string filePath = "Встречи_за_" + startDate.ToString("yyyy.MM.dd") + ".txt";
                                    if (File.Exists(filePath))
                                        File.Delete(filePath);
                                    using (StreamWriter streamWriter = new StreamWriter(filePath))
                                    {
                                        foreach (var meeting in tmpList)
                                        {
                                            streamWriter.WriteLine(meeting.ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Нет встреч на эту дату\nНажмите любую клавишу");
                                    Console.ReadKey();
                                }
                            }
                           break;
                        }
                    }
                    break;
                }
                    case ConsoleKey.Escape:
                    isOpen = false;
                    break;
            }
        }
    }
    static Meeting CreateMeeting()
    {
        InputMeeting(out string name, out DateTime starDate, out DateTime endDate, out bool needNotify, out TimeSpan notificationTime);
        if (starDate < endDate)
        {
            return new Meeting(name, starDate, endDate, needNotify, notificationTime);
        }
        return null;
    }
    static void InputMeeting(out string name, out DateTime starDate, out DateTime endDate, out bool needNotify, out TimeSpan notificationTime)
    {
        needNotify = false;
        notificationTime = TimeSpan.MinValue;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Введите название встречи:");
            var tmp = Console.ReadLine();
            if (tmp != null)
            {
                tmp = tmp.Trim();
                if (tmp != string.Empty)
                {
                    name = tmp;
                    break;
                }
            }
        }
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Введите начало встречи в формате ГГГГ.ММ.ДД ЧЧ:ММ:");
            var tmp = Console.ReadLine();
            if (tmp != null)
                tmp = tmp.Trim();
            else
                continue;
            if (DateTime.TryParse(tmp, out starDate) == true)
                break;
        }
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Введите примерный конец встречи в формате ГГГГ.ММ.ДД ЧЧ:ММ:");
            var tmp = Console.ReadLine();
            if (tmp != null)
                tmp = tmp.Trim();
            else
                continue;
            if (DateTime.TryParse(tmp, out endDate) == true)
                break;
        }
        var needClose = false;
        while (needClose == false)
        {
            Console.Clear();
            Console.WriteLine("Введите нужно ли уведомления для этой встречи\nД - да, Н - нет");
            ConsoleKeyInfo pressedkey = Console.ReadKey();
            switch (pressedkey.Key)
            {
                //русская буква Д
                case ConsoleKey.L:
                {

                    needNotify = true;
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Введите время, за которое нужно будет напомнить о встрече в формате ЧЧ:MM");
                        var tmp = Console.ReadLine();
                        if (tmp != null)
                            tmp = tmp.Trim();
                        else
                            continue;
                        if (TimeSpan.TryParse(tmp, out notificationTime) == true)
                        {
                            needClose = true;
                            break;
                        }
                    }
                    break;
                }
                //русская буква Н
                case ConsoleKey.Y:
                {
                    needNotify = false;
                    needClose = true;
                    break;
                }
            }
        }
    }
    static bool CheckingCorrectnessTime(Meeting oldMeeting, Meeting newMeeting)
    {
        //начало старой встречи больше, чем начало новой
        if (oldMeeting.StarDate.CompareTo(newMeeting.StarDate) > 0)
        {
            //начало старой встречи больше или равно концу новой
            if (oldMeeting.StarDate.CompareTo(newMeeting.EndDate) >= 0)
            {
                return true;
            }
        }
        //конец старой встречи меньше или равен началу новой
        else if (oldMeeting.EndDate.CompareTo(newMeeting.StarDate) <= 0)
        {
            if (oldMeeting.EndDate.CompareTo(newMeeting.EndDate) < 0)
            {
                return true;
            }
        }
        return false;
    }
}