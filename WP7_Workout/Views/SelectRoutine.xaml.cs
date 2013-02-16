using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using WP7_Workout.Classes;

namespace WP7_Workout.Views
{
    public partial class StartRoutine : PhoneApplicationPage
    {
        //private List<Routine> routineList;
        public StartRoutine()
        {
            InitializeComponent();            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.routineListBox.ItemsSource = LoadRoutineList();
        }

        private List<Routine> LoadRoutineList()
        {
            List<Routine> allRout = null;
            IQueryable<Routine> routineList = from routine in App.CurrentApp.DB.RoutineTable select routine;
            allRout = routineList.ToList();
            return allRout;
        }

        private void routineListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (routineListBox.SelectedItem != null)
            {
                string uri = "/Views/RunningRoutine.xaml?id=" + (this.routineListBox.SelectedItem as Routine).RoutineID.ToString();
                this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            }
        }

                
    }
}