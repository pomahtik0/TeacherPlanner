﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherPlanner.Classes
{
    public class Theme
    {
        public string Name { get; set; } = "";
        public List<Lesson> Lessons { get; private set; } = [];
        public int NumberOfLessons
        {
            get => Lessons.Count;
            set
            {
                Lessons = new List<Lesson>(value);
            }
        }
    }
}
