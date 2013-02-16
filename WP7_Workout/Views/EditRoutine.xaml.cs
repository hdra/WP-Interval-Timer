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
using WP7_Workout.Classes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Coding4Fun.Phone.Controls;
using Coding4Fun.Phone.Controls.Toolkit;
using Microsoft.Phone.Shell;

namespace WP7_Workout.Views
{
    public partial class EditRoutine : PhoneApplicationPage, INotifyPropertyChanged
    {
        Routine selectedRoutine;
        private ObservableCollection<Workout> workoutList;
        public ObservableCollection<Workout> WorkoutList
        {
            get
            {
                return workoutList;
            }
            set
            {
                if (workoutList != value)
                {
                    workoutList = value;
                    NotifyPropertyChanged("WorkoutList");
                }
            }
        }


        public EditRoutine()
        {
            InitializeComponent();
            workoutListBox.Loaded += new RoutedEventHandler(workoutListBox_Loaded);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //Get the Routine selected from the previous page
            selectedRoutine = (Routine)(from rout in App.CurrentApp.DB.RoutineTable
                                        where rout.RoutineID.ToString() == this.NavigationContext.QueryString["id"]
                                        select rout).Single();

            //Populate the workoutlistbox
            workoutListBox.Loaded += new RoutedEventHandler(workoutListBox_Loaded);
            WorkoutList = GetAssociatedWorkout();
            workoutListBox.ItemsSource = WorkoutList;
        }

        void workoutListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.backStackNav)
            {
                App.backStackNav = false;
                ListBoxItem item = this.workoutListBox.ItemContainerGenerator.ContainerFromIndex(workoutListBox.Items.Count - 1) as ListBoxItem;
                TimeSpanPicker timespan = FindFirstElementInVisualTree<TimeSpanPicker>(item);
                timespan.OpenPicker();
            }
        }
            

        private ObservableCollection<Workout> GetAssociatedWorkout()
        {
            ObservableCollection<Workout> allex = null;
            IQueryable<Workout> exlist = from ex in App.CurrentApp.DB.WorkoutTable
                                         where ex.RoutineID == selectedRoutine.RoutineID
                                         select ex;
            allex = new ObservableCollection<Workout>(exlist.ToList());
            return allex;
        }

        

        private void addButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/AddWorkout.xaml?mode=edit&routine="+selectedRoutine.RoutineID.ToString(), UriKind.Relative));
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("are you sure?", "Remove workout", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Workout toDelete = workoutListBox.SelectedItem as Workout;

                WorkoutList.Remove(toDelete);
                selectedRoutine.WorkoutList.Remove(toDelete);

                App.CurrentApp.DB.WorkoutTable.DeleteOnSubmit(toDelete);

                //App.CurrentApp.DB.SubmitChanges();
            }
        }

        private void workoutListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationBarIconButton deleteButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            if (workoutListBox.SelectedItem != null)
            {
                deleteButton.IsEnabled = true;
            }
            else
            {
                deleteButton.IsEnabled = false;
            }
        }

        private T FindFirstElementInVisualTree<T>(DependencyObject parentElement) where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(parentElement);
            if (count == 0)
                return null;

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parentElement, i);

                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    var result = FindFirstElementInVisualTree<T>(child);
                    if (result != null)
                        return result;
                }
            }
            return null;
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