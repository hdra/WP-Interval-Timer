using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WP7_Workout.Classes
{
    [Table]
    public class Routine : INotifyPropertyChanged, INotifyPropertyChanging
    {
        //To increase update performance
        [Column(IsVersion = true)]
        private Binary _version;


        private int _routineID;
        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int RoutineID 
        {
            get { return _routineID; }
            set
            {
                if (_routineID != value)
                {
                    NotifyPropertyChanging("RoutineID");
                    _routineID = value;
                    NotifyPropertyChanged("RoutineID");
                }
            }
        }
        
        private string _routineName;
        [Column]
        public string RoutineName 
        {
            get { return _routineName; }
            set
            {
                if (_routineName != value)
                {
                    NotifyPropertyChanging("RoutineName");
                    _routineName = value;
                    NotifyPropertyChanged("RoutineName");
                }
            }

        }

        //private int _totalDuration;

        private int _totalMinutes;
        [Column]
        public int TotalMinutes
        {
            get { return _totalMinutes; }
            //set { TotalMinutes = value; }
            set
            {
                if (_totalMinutes != value)
                {
                    NotifyPropertyChanging("TotalMinutes");
                    _totalMinutes = value;
                    NotifyPropertyChanged("TotalMinutes");
                }
            }
        }

        private int _totalSeconds;
        [Column]
        public int TotalSeconds
        {
            get { return _totalSeconds; }
            //set { TotalSeconds = value; }
            set
            {
                if (_totalSeconds != value)
                {
                    NotifyPropertyChanging("TotalSeconds");
                    _totalSeconds = value;
                    NotifyPropertyChanged("TotalSeconds");
                }
            }
        }

        
        //from here, not too sure

        public Routine()
        {
            this.TotalMinutes = 0;
            this.TotalSeconds = 0;
            this._workoutList = new EntitySet<Workout>(this.OnWorkoutAdded, this.OnWorkoutRemoved);
        }

        public void OnWorkoutAdded(Workout workout)
        {
            workout.OwnerRoutine = this;
            this.TotalMinutes += workout.Minutes;
            this.TotalSeconds += workout.Seconds;

            if (this.TotalSeconds > 59)
            {
                this.TotalMinutes++;
                this.TotalSeconds -= 60;
            }
        }

        public void OnWorkoutRemoved(Workout workout)
        {
            workout.OwnerRoutine = null;
            this.TotalMinutes -= (workout.Minutes);
            this.TotalSeconds -= workout.Seconds;
            if (this.TotalSeconds < 0)
            {
                this.TotalMinutes--;
                this.TotalSeconds += 60;
            }
        }

        private EntitySet<Workout> _workoutList;
        [Association(ThisKey = "RoutineID", OtherKey = "RoutineID", Storage = "_workoutList")]
        public EntitySet<Workout> WorkoutList
        {
            get { return this._workoutList; }
            set { this._workoutList.Assign(value); }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}
