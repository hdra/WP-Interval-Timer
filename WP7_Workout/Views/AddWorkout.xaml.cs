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

namespace WP7_Workout.Views
{
    public partial class AddWorkout : PhoneApplicationPage
    {
        public AddWorkout()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.exerciseListBox.ItemsSource = GetAllExercises();
        }

        private ObservableCollection<Exercise> GetAllExercises()
        {
            ObservableCollection<Exercise> allex = null;
            IQueryable<Exercise> exlist = from ex in App.CurrentApp.DB.ExerciseTable select ex;
            allex = new ObservableCollection<Exercise>(exlist.ToList());
            return allex;
        }

        private void exerciseListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //TODO:need to check if there is mode is add or edit
            if (this.NavigationContext.QueryString["mode"] == "add")
            {
                App.CurrentApp.DB.WorkoutTable.InsertOnSubmit(new Workout() { ExerciseID = (exerciseListBox.SelectedItem as Exercise).ExerciseID, Minutes = 2, Seconds = 0 });
                App.CurrentApp.DB.SubmitChanges();
            }
            else if (this.NavigationContext.QueryString["mode"] == "edit")
            {
                //this is wrong should add this to the routine
                //maybe try to query the routine data here, and add it directly
                int routineId = int.Parse(this.NavigationContext.QueryString["routine"].ToString());
                Routine editedRoutine = (from x in App.CurrentApp.DB.RoutineTable 
                                         where x.RoutineID == routineId
                                         select x).SingleOrDefault();                               
                Workout createdWorkout = new Workout() { ExerciseID = (exerciseListBox.SelectedItem as Exercise).ExerciseID, Minutes = 2, Seconds = 0 };
                App.CurrentApp.DB.WorkoutTable.InsertOnSubmit(createdWorkout);
                App.CurrentApp.DB.SubmitChanges();
                editedRoutine.WorkoutList.Add(createdWorkout);
                App.CurrentApp.DB.SubmitChanges();
            }
            //AddRoutine.workoutList.Add(new Workout() { ExerciseID = (exerciseListBox.SelectedItem as Exercise).ExerciseID, Minutes = 0, Seconds = 0 });
            App.backStackNav = true;
            this.NavigationService.GoBack();
        }
            
        
    }
}