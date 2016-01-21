﻿using System;
using System.Drawing;
using Todo.Service.Model.Interface;

namespace Todo.Service.Model.Fake
{
    public class FakeCategory : ICategory
    {
        public int Id { get; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public int Order { get; set; }

        public FakeCategory(int id)
        {
            Id = id;
        }
    }
}