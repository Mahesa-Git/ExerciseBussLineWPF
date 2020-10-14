using System;
using System.Collections.Generic;
using System.Windows;

namespace ExerciseBussLineWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BusStationGenerator();
        }
        static Random rnd = new Random();
        static List<BusStation> busStations = new List<BusStation>();
        static List<Passenger> passengerList = new List<Passenger>();
        static Bus bus = new Bus(20, 0);

        private void Drive_Button_Click(object sender, RoutedEventArgs e)
        {
            listBox_0_.Items.Clear();
            listBox_1_.Items.Clear();
            listBox_2_.Items.Clear();
            listBox_3_.Items.Clear();
            listBox_4_.Items.Clear();
            PassengerGenerator(rnd.Next(2, 6 + 1));
            for (int i = passengerList.Count - 1; i >= 0; i--)
            {
                if (Bus.currentStation == passengerList[i].EntersBusAt && passengerList[i].IsChecked == true)
                {
                    if (bus.PassengersOnBus < bus.MaxPassengers)
                    {
                        passengerList[i].IsRiding = true;
                        bus.PassengersOnBus++;
                    }
                    else
                        Bus.BusIsFullMessage++;
                }
                if (passengerList[i].IsRiding == true && passengerList[i].ExitsBusAt == Bus.currentStation)
                {
                    Bus.leavingPassengers++;
                    passengerList.RemoveAt(i);
                }
            }
            for (int i = 0; i < busStations.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        if (Bus.currentStation == i)
                        {
                            listBox_0_.Items.Add(BusAtThisStationMessage(i));
                            myImageOne.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            listBox_0_.Items.Add(BusNotAtThisStationMessage(i));
                            myImageOne.Visibility = Visibility.Hidden;
                        }
                        break;
                    case 1:
                        if (Bus.currentStation == i)
                        {
                            listBox_1_.Items.Add(BusAtThisStationMessage(i));
                            myImageTwo.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            listBox_1_.Items.Add(BusNotAtThisStationMessage(i));
                            myImageTwo.Visibility = Visibility.Hidden;
                        }
                        break;
                    case 2:
                        if (Bus.currentStation == i)
                        {
                            listBox_2_.Items.Add(BusAtThisStationMessage(i));
                            myImageThree.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            listBox_2_.Items.Add(BusNotAtThisStationMessage(i));
                            myImageThree.Visibility = Visibility.Hidden;
                        }
                        break;
                    case 3:
                        if (Bus.currentStation == i)
                        {
                            listBox_3_.Items.Add(BusAtThisStationMessage(i));
                            myImageFour.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            listBox_3_.Items.Add(BusNotAtThisStationMessage(i));
                            myImageFour.Visibility = Visibility.Hidden;
                        }
                        break;
                    case 4:
                        if (Bus.currentStation == i)
                        {
                            listBox_4_.Items.Add(BusAtThisStationMessage(i));
                            myImageFive.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            listBox_4_.Items.Add(BusNotAtThisStationMessage(i));
                            myImageFive.Visibility = Visibility.Hidden;
                        }
                        break;
                    default:
                        break;
                }
            }

            bus.PassengersOnBus -= Bus.leavingPassengers;
            nrOfPassengersLabel.Content = ($"{(Bus.BusIsFullMessage == 0 ? ($"{bus.PassengersOnBus}") : "20")}");

            Bus.BusIsFullMessage = 0;
            Bus.leavingPassengers = 0;
            for (int i = 0; i < passengerList.Count; i++)
                passengerList[i].IsChecked = true;
            if (Bus.currentStation == busStations.Count - 1)
                Bus.currentStation = -1;

            Bus.currentStation++;
        }
        static void PassengerGenerator(int thisMany)
        {
            for (int i = 0; i < thisMany; i++)
            {
                int exitsBusAt;
                int entersBusAt;
                while (true)
                {
                    exitsBusAt = rnd.Next(0, 5);
                    entersBusAt = rnd.Next(0, 5);

                    if ((entersBusAt != Bus.currentStation + 1) && (entersBusAt != Bus.currentStation - 1) && (entersBusAt != Bus.currentStation))
                        break;
                }
                Passenger passenger = new Passenger(exitsBusAt, entersBusAt, false, false);
                passengerList.Add(passenger);
            }
            AssignNewWaitingPassengers(passengerList);
        }
        static void AssignNewWaitingPassengers(List<Passenger> thisList)
        {
            for (int i = 0; i < busStations.Count; i++)
            {
                for (int j = 0; j < thisList.Count; j++)
                {
                    if (thisList[j].EntersBusAt == busStations[i].StationID && thisList[j].IsChecked == false)
                        busStations[i].WaitingPassengers++;
                }
            }
        }
        static void BusStationGenerator()
        {
            for (int i = 0; i < 5; i++)
            {
                BusStation tempStop = new BusStation($"Hållplats {i + 1}: ", 0, i);
                busStations.Add(tempStop);
            }
        }
        static string BusAtThisStationMessage(int i)
        {
            string returnValue = ($"Bussen är här,släpper\nav {Bus.leavingPassengers} passagerare." +
                            $"{(Bus.BusIsFullMessage != 0 ? ($"\noch hämtar upp {(busStations[i].WaitingPassengers - Bus.BusIsFullMessage)}.\n{Bus.BusIsFullMessage} kunde inte åka med\ndå bussen var full") : ($"\noch hämtar upp\n{busStations[i].WaitingPassengers} passagerare."))}");
            busStations[i].WaitingPassengers = 0;
            busStations[i].WaitingPassengers += Bus.BusIsFullMessage;
            return returnValue;
        }
        static string BusNotAtThisStationMessage(int i)
        {
            string returnValue = ($"{busStations[i].WaitingPassengers} väntande\npassagerare");
            return returnValue;
        }
    }
}
