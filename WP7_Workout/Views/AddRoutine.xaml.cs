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
using Microsoft.Phone.Shell;
using WP7_Workout.Classes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Coding4Fun.Phone.Controls;
using Coding4Fun.Phone.Controls.Toolkit;

namespace WP7_Workout.Views
{
    public partial class AddRoutine : PhoneApplicationPage, INotifyPropertyChanged
    {
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

        public AddRoutine()
        {
            InitializeComponent();
            workoutListBox.Loaded += new RoutedEventHandler(workoutListBox_Loaded);  
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            workoutList = GetAllWorkout();
            this.workoutListBox.ItemsSource = workoutList;
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

        private ObservableCollection<Workout> GetAllWorkout()
        {
            ObservableCollection<Workout> allex = null;
            IQueryable<Workout> exlist = from ex in App.CurrentApp.DB.WorkoutTable
                                         where ex.RoutineID == null
                                         select ex;
            allex = new ObservableCollection<Workout>(exlist.ToList());
            return allex;
        }

        private void workoutListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationBarIconButton deleteButton = (ApplicationBarIconButton)ApplicationBar.Buttons[2];
            if (workoutListBox.SelectedItem != null)
            {
                deleteButton.IsEnabled = true;
            }
            else
            {
                deleteButton.IsEnabled = false;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/AddWorkout.xaml?mode=add", UriKind.Relative));
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("are you sure?", "Remove workout", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Workout toDelete = workoutListBox.SelectedItem as Workout;

                WorkoutList.Remove(toDelete);
                App.CurrentApp.DB.WorkoutTable.DeleteOnSubmit(toDelete);

                App.CurrentApp.DB.SubmitChanges();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (WorkoutList.Count > 0)
            {
                InputPrompt input = new InputPrompt();
                input.Completed += new EventHandler<PopUpEventArgs<string, PopUpResult>>(input_Completed);
                input.Title = "Routine Name";
                input.Message = "enter a name for the routine";
                input.Show();
            }
            else
                MessageBox.Show("There are no workout in the routine");
        }

        void input_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            string routineName = ((InputPrompt)sender).Value;

            //do the saving routine logic here
            Routine newRoutine = new Routine() { RoutineName = routineName };
            App.CurrentApp.DB.RoutineTable.InsertOnSubmit(newRoutine);
            App.CurrentApp.DB.SubmitChanges();
            newRoutine.WorkoutList.AddRange(WorkoutList);            
            App.CurrentApp.DB.SubmitChanges();
            this.NavigationService.GoBack();
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