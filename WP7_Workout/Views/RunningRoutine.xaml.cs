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

using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace WP7_Workout.Views
{
    public partial class RunningRoutine : PhoneApplicationPage
    {
        Routine selectedRoutine;
        List<Workout> workoutList;
        Workout currentWorkout;
        int currentIndex=0;
        bool? paused = null;
        //stuffs of the dispathertimer
        Stopwatch countdownstopwatch = new Stopwatch();
        DispatcherTimer countdowntimer = new DispatcherTimer();
        long totaltickremaining = 0;

        public RunningRoutine()
        {
            InitializeComponent();
            countdowntimer.Tick += new EventHandler(countdowntimer_Tick);
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);
            if ((e.Orientation & PageOrientation.Landscape) == PageOrientation.Landscape)
            {
                this.rowGrid1.Height = new GridLength(0);
                //this.placeRec.Height = 0;
            }
            else
            {
                //this.placeRec.Height = 150;
                //TODO: do the portrait
                this.rowGrid1.Height = new GridLength(1, GridUnitType.Star);
            }
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            //TODO: Do some clean up when the page is no longer active
            totaltickremaining = -1;
            paused = null;
        }

        private void InitializeExercise()
        {
            //if the index is still less than the workoutList, then reset the timer,
            //load up the exercise, and start the timer again
            //TODO: put something to play a sound here
            
            if (currentIndex < workoutList.Count)
            {

                //load the exercise
                currentWorkout = workoutList[currentIndex];
                this.txtExerciseName.Text = currentWorkout.ExerciseType.ExerciseName;
                this.txtExerciseDescription.Text = currentWorkout.ExerciseType.ExerciseInstruction;
                this.countdownTextBlock.Text = currentWorkout.Minutes.ToString("00") + ":" + currentWorkout.Seconds.ToString("00");
                startTimer();
            }
            else
            {
                countdowntimer.Stop();
                countdownstopwatch.Stop();
                if (MessageBox.Show("routine completed") == MessageBoxResult.OK)
                {
                    //need to clear the backstack once, or it will go back to the messagbeox
                    NavigationService.RemoveBackEntry();
                    this.NavigationService.GoBack();
                }
            }

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            currentIndex = 0;

            string selectedID = this.NavigationContext.QueryString["id"];

            selectedRoutine = (Routine)(from x in App.CurrentApp.DB.RoutineTable
                                        where x.RoutineID.ToString() == selectedID
                                        select x).SingleOrDefault();

            workoutList = (from x in App.CurrentApp.DB.WorkoutTable
                                         where x.RoutineID == selectedRoutine.RoutineID
                                         select x).ToList<Workout>();

            //set it up for the 1st exercise
            currentWorkout = workoutList[currentIndex];
            this.txtExerciseName.Text = currentWorkout.ExerciseType.ExerciseName;
            this.txtExerciseDescription.Text = currentWorkout.ExerciseType.ExerciseInstruction;
            this.countdownTextBlock.Text = currentWorkout.Minutes.ToString("00") + ":" + currentWorkout.Seconds.ToString("00");
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (paused == true || paused == null)
            {
                startTimer();
                this.startButton.Content = "Pause";
                this.paused = false;
            }
            else if (paused != true)
            {
                countdownstopwatch.Stop();
                countdowntimer.Stop();
                this.startButton.Content = "Start";
                this.paused = true;
            }
        }

        void startTimer()
        {
            //if the timer has been reset
            if (paused == null)
            {
                //get the total ticks of the current exercise
                totaltickremaining = countDownTicks();
                countdownstopwatch.Reset();
            }
            countdowntimer.Interval = new TimeSpan(0, 0, 0, 1);
            //countdowntimer.Tick += new EventHandler(countdowntimer_Tick);
            countdowntimer.Start();
            
            countdownstopwatch.Start();
        }
        
        void countdowntimer_Tick(object sender, EventArgs e)
        {
            long ticksremaining = totaltickremaining - countdownstopwatch.GetElapsedDateTimeTicks();
            if (ticksremaining < 0)
            {
                //play the sound
                var stream = TitleContainer.OpenStream("Audio/beep2.wav");
                var effect = SoundEffect.FromStream(stream);
                FrameworkDispatcher.Update();
                effect.Play();
                //point to next exercise
                currentIndex++;
                paused = null;
                //reset the timer
                countdowntimer.Stop();
                countdownstopwatch.Stop();
                countdownTextBlock.Text = "00:00";
                InitializeExercise();                
            }
            else
            {
                TimeSpan newtimespan = new TimeSpan(ticksremaining);
                int min = newtimespan.Minutes;
                int sec = newtimespan.Seconds;
                countdownTextBlock.Text = min.ToString("00") + ":" + sec.ToString("00");
            }
        }

        long countDownTicks()
        {
            return (new TimeSpan(0, currentWorkout.Minutes, currentWorkout.Seconds)).Ticks;
        }
    }
}