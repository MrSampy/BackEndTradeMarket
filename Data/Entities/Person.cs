using System;

namespace Data.Entities
{
    public class Person:BaseEntity
    {
        public string Name { set; get; }
        public string Surname { set; get; }
        public DateTime BirthDate { set; get; }
    }
}