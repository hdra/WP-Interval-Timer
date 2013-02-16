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
using System.Collections.ObjectModel;
using WP7_Workout.Classes;
using System.ComponentModel;
using Microsoft.Phone.Shell;

namespace WP7_Workout.Views
{
    public partial class ManageRoutines : PhoneApplicationPage
    {
        private ObservableCollection<Routine> allRoutines;
        public ObservableCollection<Routine> AllRoutines
        {
            get
            {
                return allRoutines;
            }
            set
            {
                if (allRoutines != value)
                {
                    allRoutines = value;
                    NotifyPropertyChanged("AllRoutines");
                }
            }
        }
        public ManageRoutines()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AllRoutines = LoadRoutineList();
            this.routineListBox.ItemsSource = AllRoutines;
            clearNullWorkout();
        }

        private ObservableCollection<Routine> LoadRoutineList()
        {
            ObservableCollection<Routine> allRout = null;
            IQueryable<Routine> routineList = from routine in App.CurrentApp.DB.RoutineTable select routine;
            allRout = new ObservableCollection<Routine>(routineList.ToList());
            return allRout;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            string uri = "/Views/EditRoutine.xaml?id=" + (routineListBox.SelectedItem as Routine).RoutineID.ToString();
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/AddRoutine.xaml", UriKind.Relative));
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("are you sure?", "Remove routine", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Routine toDelete = routineListBox.SelectedItem as Routine;
                IQueryable<Workout> associatedWorkout = from x in App.CurrentApp.DB.WorkoutTable
                                                  where x.RoutineID == toDelete.RoutineID
                                                  select x;

                AllRoutines.Remove(toDelete);
                App.CurrentApp.DB.RoutineTable.DeleteOnSubmit(toDelete);
                App.CurrentApp.DB.WorkoutTable.DeleteAllOnSubmit(associatedWorkout);

                App.CurrentApp.DB.SubmitChanges();
            }
        }

        private void clearNullWorkout()
        {
            IQueryable<Workout> associatedWorkout = from x in App.CurrentApp.DB.WorkoutTable
                                                    where x.RoutineID == null
                                                    select x;
            App.CurrentApp.DB.WorkoutTable.DeleteAllOnSubmit(associatedWorkout);

            App.CurrentApp.DB.SubmitChanges();
        }

        private void routineListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationBarIconButton editButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            ApplicationBarIconButton deleteButton = (ApplicationBarIconButton)ApplicationBar.Buttons[2];
            if (routineListBox.SelectedItem != null)
            {
                editButton.IsEnabled = true;
                deleteButton.IsEnabled = true;
            }
            else
            {
                editButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        
    }
}