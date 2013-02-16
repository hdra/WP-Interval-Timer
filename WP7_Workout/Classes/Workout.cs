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
    public class Workout
    {
        public Workout()
        {
            RoutineID = null;
        }

        private int _workoutID;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int WorkoutID
        {
            get
            {
                return _workoutID;
            }
            set
            {
                if (_workoutID != value)
                {
                    NotifyPropertyChanging("WorkoutID");
                    _workoutID = value;
                    NotifyPropertyChanged("WorkoutID");
                }
            }

        }

        //private int _duration;
        //[Column]
        //public int Duration
        //{
        //    get
        //    {
        //        return _duration;
        //    }
        //    set
        //    {
        //        if (_duration != value)
        //        {
        //            NotifyPropertyChanging("Duration");
        //            _duration = value;
        //            NotifyPropertyChanged("Duration");
        //        }
        //    }

        //}

        private int _minutes;
        [Column]
        public int Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                if (_minutes != value)
                {
                    //experiments here
                    if (this.RoutineID != null)
                    {
                        this._routineRef.Entity.TotalMinutes -= _minutes;
                        this._routineRef.Entity.TotalMinutes += value;
                    }

                    NotifyPropertyChanging("Minutes");
                    _minutes = value;
                    NotifyPropertyChanged("Minutes");

                    
                }
            }

        }

        private int _seconds;
        [Column]
        public int Seconds
        {
            get
            {
                return _seconds;
            }
            set
            {
                if (this.RoutineID != null)
                {                    
                    this._routineRef.Entity.TotalSeconds -= _seconds;
                    this._routineRef.Entity.TotalSeconds += value;
                    if (this._routineRef.Entity.TotalSeconds<0)
                    {
                        this._routineRef.Entity.TotalMinutes--;
                        this._routineRef.Entity.TotalSeconds += 60;
                    }
                    else if (this._routineRef.Entity.TotalSeconds > 60)
                    {
                        this._routineRef.Entity.TotalMinutes++;
                        this._routineRef.Entity.TotalSeconds -= 60;
                    }
                }
                if (_seconds != value)
                {
                    NotifyPropertyChanging("Seconds");
                    _seconds = value;
                    NotifyPropertyChanged("Seconds");
                }
            }

        }


        public TimeSpan DurationFormat
        {
            get 
            {
                return new TimeSpan(0, Minutes, Seconds);
            }
            set
            {
                Minutes = value.Minutes;
                Seconds = value.Seconds;
            }
        }


        [Column]
        public int ExerciseID { get; set; }

        private EntityRef<Exercise> exerciseType;
        [Association(ThisKey = "ExerciseID", OtherKey = "ExerciseID", Storage = "exerciseType")]
        public Exercise ExerciseType
        {
            get { return exerciseType.Entity; }
            set { exerciseType.Entity = value; }
        }


        //from here, not too sure
        [Column]
        public int? RoutineID { get; set; }

        private EntityRef<Routine> _routineRef= new EntityRef<Routine>();

        [Association(ThisKey = "RoutineID", OtherKey = "RoutineID", Storage = "_routineRef")]
        public Routine OwnerRoutine
        {
            get { return _routineRef.Entity; }
            set 
            {
                Routine previousRoutine = this._routineRef.Entity;
                if ((previousRoutine != value) || (this._routineRef.HasLoadedOrAssignedValue == false))
                {
                    if (previousRoutine != null)
                    {
                        this._routineRef.Entity = null;
                        previousRoutine.WorkoutList.Remove(this);
                    }

                    this._routineRef.Entity = value;

                    if (value != null)
                    {
                        value.WorkoutList.Add(this);
                        this.RoutineID = value.RoutineID;
                    }
                    else
                    {
                        this.RoutineID = default(Nullable<int>);
                    }
                }
            }

        }



        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

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
