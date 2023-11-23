using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Лр6_пр2.Program.Sequruty;
using static Лр6_пр2.Program.Sequruty.Police;
namespace Лр6_пр2
{
    internal class Program
    {

        public class Car
        {
            string nomer;
            public string Nomer
            {
                get { return nomer; }
            }
            public Car(string nomer)
            {
                this.nomer = nomer;
            }
        }
        public class Sequruty
        {
            string name;//поле ім'я
            //властивість
            public string Name
            {
                get { return name; }
            }
            //конструктор
            public Sequruty(string name)
            {
                this.name = name;
            }
            //обобник події NotParking
            public void CloseParking()
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Немає мiсць. Охоронець {0} закрив стоянку", name);
                //відписуємось від статичної події
                Parking.NotPlaces -= this.CloseParking;
            }
            public class Police
            {
                string name;
                public string Name
                {
                    get { return name; }
                }
                public Police(string name)
                {
                    this.name = name;
                }
                public void VideoSwitch0n()
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Полiцейський {0} включив вiдеоспостереження ", name);
                    //відписуємось від події
                    Parking.NotPlaces -= this.VideoSwitch0n;
                }
                public void DroveOutAddress(int t)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Спрацювала сигналiзацiя {0} раз", t);
                    Console.WriteLine("Полiцейський {0} приїхав на стоянку", name);
                }
                public class Parking
                {
                    //делегат,відповідаючий події SignalTriggered
                    public delegate void SignelTriggeredHandler(int k);
                    //подія SignalTriggered
                    public static event SignelTriggeredHandler SignalTriggered;
                    //делегат,відповідаючий події NotPlaces
                    public delegate void NotPlacesEventHandler();
                    // подія NotPlaces
                    public static event NotPlacesEventHandler NotPlaces;
                    //поля класа
                    bool therePlaces;//мають місця
                    string adr;//адреса
                    int AllPlaces;//кількість місць
                    List<Car> cars;//колекція машин
                    int t;//лічильник включених сигналізацй
                    //властивість
                    public bool TherePlaces
                    {
                        get { return therePlaces; }
                    }
                    //конструктор
                    public Parking(string adr, int AllPlaces)
                    {
                        this.adr = adr; this.AllPlaces = AllPlaces;
                        cars = new List<Car>(0);
                        this.therePlaces = true; t = 0;
                    }
                    public void PushCar(Car car)
                    {
                        Random o = new Random();
                        if ((NotPlaces != null) && cars.Count > AllPlaces - 1)
                        {
                            NotPlaces();//подія відбулася
                            therePlaces = false;//змінили поле місць немає
                        }
                        else
                        {
                            //добавляємо машину
                            cars.Add(car);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("На стоянку прибула " + car.Nomer);
                            int x = o.Next(1, 8);
                            if (x == 1)//випадково
                            {
                                t++;
                                //подія відбулася: спрацювала сигналізація
                                SignalTriggered(t);
                            }
                        }
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            //створюємо об'єкти 
            Parking parking = new Parking("вул. Краснова", 10);
            Sequruty sequrityMan = new Sequruty("Нiколай");
            Police polisMan = new Police("Александр");
            //підписка на статичні події
            //клас в якому проходить подія. Подія
            //+= об'єкт класа, який обробляє подію. Метод обробок
            Parking.NotPlaces += sequrityMan.CloseParking;
            Parking.NotPlaces += polisMan.VideoSwitch0n;
            Parking.SignalTriggered += polisMan.DroveOutAddress;
            int i = 1;
            while (parking.TherePlaces)
            {
                Car c = new Car("машина " + i);
                parking.PushCar(c);
                i++;
            }
            Console.ReadKey();
        }
    }
}
