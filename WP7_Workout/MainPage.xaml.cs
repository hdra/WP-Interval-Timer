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
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Serialization;
using WP7_Workout.Classes;
using System.Data.Linq;

namespace WP7_Workout
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            //CreateInitialData();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            CreateInitialData();
        }

        private void CreateInitialData()
        {
            if (!App.CurrentApp.DB.DatabaseExists())
            {
                App.CurrentApp.DB.CreateDatabase();

                var exer1 = new Exercise()
                {
                    ExerciseName = "Warm Up",
                    ExerciseInstruction = "Warm up at an easy-moderate pace"
                };
                App.CurrentApp.DB.ExerciseTable.InsertOnSubmit(exer1);
                var exer2 = new Exercise()
                {
                    ExerciseName = "Box Jumps",
                    ExerciseInstruction = "Stand facing a 12-to-18-inch-high box or step. Squat slightly, then swing your arms for momentum as you jump up onto the box, landing with your knees soft. Step down and continue at a fast pace "                                           
                };
                App.CurrentApp.DB.ExerciseTable.InsertOnSubmit(exer2);
                var exer3 = new Exercise()
                {
                    ExerciseName = "Side Shuffles",
                    ExerciseInstruction = "Place two cones about 6 feet apart. Side-shuffle between them, traveling back and forth as quickly as possible"
                };
                App.CurrentApp.DB.ExerciseTable.InsertOnSubmit(exer3);
                var exer4 = new Exercise()
                {
                    ExerciseName = "Traveling High Skips",
                    ExerciseInstruction = "Find an open area and skip, using your momentum to get big air and moving forward as quickly as possible"
                };
                App.CurrentApp.DB.ExerciseTable.InsertOnSubmit(exer4);
                var exer5 = new Exercise()
                {
                    ExerciseName = "Rest",
                    ExerciseInstruction = "Take a rest and recover your energy"
                };
                App.CurrentApp.DB.ExerciseTable.InsertOnSubmit(exer5);

                var exer6 = new Exercise()
                {
                    ExerciseName = "Treadmill Run I",
                    ExerciseInstruction = "Speed up your speed on the treadmill, but keep it at a moderate pace"
                };
                App.CurrentApp.DB.ExerciseTable.InsertOnSubmit(exer6);
                var exer7 = new Exercise()
                {
                    ExerciseName = "Treadmill Run II",
                    ExerciseInstruction = "Run on the treadmill even faster, raising the intensity"
                };
                App.CurrentApp.DB.ExerciseTable.InsertOnSubmit(exer7);
                var exer8 = new Exercise()
                {
                    ExerciseName = "Treadmill Run III",
                    ExerciseInstruction = "Slow down your speed, but keep running"
                };
                App.CurrentApp.DB.ExerciseTable.InsertOnSubmit(exer8);
                App.CurrentApp.DB.SubmitChanges();


                var work1 = new Workout() { Minutes = 5, Seconds = 0 };
                work1.ExerciseID = exer1.ExerciseID;
                var work2 = new Workout() { Minutes = 5, Seconds = 0 };
                work2.ExerciseID = exer2.ExerciseID;
                var work3 = new Workout() { Minutes = 5, Seconds = 0 };
                work3.ExerciseID = exer3.ExerciseID;
                var work4 = new Workout() { Minutes = 5, Seconds = 0 };
                work4.ExerciseID = exer4.ExerciseID;
                var work5 = new Workout() { Minutes = 5, Seconds = 0 };
                work5.ExerciseID = exer5.ExerciseID;

                var work6 = new Workout() { Minutes = 3, Seconds = 30 };
                work6.ExerciseID = exer1.ExerciseID;
                var work7 = new Workout() { Minutes = 5, Seconds = 0 };
                work7.ExerciseID = exer6.ExerciseID;
                var work8 = new Workout() { Minutes = 5, Seconds = 0 };
                work8.ExerciseID = exer7.ExerciseID;
                var work9 = new Workout() { Minutes = 3, Seconds = 0 };
                work9.ExerciseID = exer8.ExerciseID;
                var work10 = new Workout() { Minutes = 3, Seconds = 30 };
                work10.ExerciseID = exer7.ExerciseID;
                var work11 = new Workout() { Minutes = 3, Seconds = 30 };
                work11.ExerciseID = exer8.ExerciseID;
                var work12 = new Workout() { Minutes = 5, Seconds = 0 };
                work12.ExerciseID = exer5.ExerciseID;


                App.CurrentApp.DB.WorkoutTable.InsertAllOnSubmit(new[] { work1, work2, work3, work4, work5, work6 });
                App.CurrentApp.DB.SubmitChanges();

                var rout1 = new Routine() { RoutineName = "Power Interval 1" };
                var rout2 = new Routine() { RoutineName = "Treadmill Cardio Interval 1" };
                App.CurrentApp.DB.RoutineTable.InsertOnSubmit(rout1);
                App.CurrentApp.DB.RoutineTable.InsertOnSubmit(rout2);
                App.CurrentApp.DB.SubmitChanges();

                rout1.WorkoutList.AddRange(new[] { work1, work2, work3, work4, work5 });
                rout2.WorkoutList.AddRange(new[] { work6, work7, work8, work9, work10, work11, work12 });
                App.CurrentApp.DB.SubmitChanges();

                
            }
            //else
            //{
            //    DataLoadOptions options = new DataLoadOptions();
            //    options.LoadWith<Person>(c => c.PersonTitle);
            //    App.CurrentApp.DB.LoadOptions = options;
            //    var people = App.CurrentApp.DB.People.ToList();
            //}
        }

    }
}