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
using System.ComponentModel;
using WP7_Workout.Classes;
using System.Collections.ObjectModel;
using Microsoft.Phone.Shell;

namespace WP7_Workout.Views
{
    public partial class ManageExercises : PhoneApplicationPage, INotifyPropertyChanged
    {
        private ObservableCollection<Exercise> allExercise;
        public ObservableCollection<Exercise> AllExercise
        {
            get 
            {
                return allExercise;
            }
            set
            {
                if (allExercise != value)
                {
                    allExercise = value;
                    NotifyPropertyChanged("AllExercise");
                }
            }
        }

        public ManageExercises()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AllExercise = GetAllExercises();
            this.exerciseListBox.ItemsSource = AllExercise;
        }

        private ObservableCollection<Exercise> GetAllExercises()
        {
            ObservableCollection<Exercise> allex = null;
            IQueryable<Exercise> exlist = from ex in App.CurrentApp.DB.ExerciseTable select ex;
            allex = new ObservableCollection<Exercise>(exlist.ToList());
            return allex;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/AddExercise.xaml", UriKind.Relative));
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            string uri = "/Views/EditExercise.xaml?id="+ (exerciseListBox.SelectedItem as Exercise).ExerciseID.ToString() ;
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Remove exercise", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Exercise toDelete = exerciseListBox.SelectedItem as Exercise;
                AllExercise.Remove(toDelete);
                App.CurrentApp.DB.ExerciseTable.DeleteOnSubmit(toDelete);
                App.CurrentApp.DB.SubmitChanges();
            }
        }

        private void exerciseListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationBarIconButton editButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            ApplicationBarIconButton deleteButton = (ApplicationBarIconButton)ApplicationBar.Buttons[2];
            if (exerciseListBox.SelectedItem != null)
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