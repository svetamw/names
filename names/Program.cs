using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace names
{
    class Program
    {
        static void Main(string[] args)
        {
            const string src_file_name = "Names.txt";
            var first_names = new List<string>();
            var last_names = new List<string>();
            var patronymics = new List<string>();


            using (var file = new StreamReader(src_file_name))
            {
                while (!file.EndOfStream)
                {
                    var str = file.ReadLine();
                    var values = str.Split(' ');
                    first_names.Add(values[1]);
                    last_names.Add(values[0]);
                    patronymics.Add(values[2]);
                    //Console.WriteLine(str);
                }
            }
            var students = CreateStudents(first_names.ToArray(), last_names.ToArray(), patronymics.ToArray(), 100);
            double min = double.PositiveInfinity;
            double max = double.NegativeInfinity;
            for (int i = 0; i < students.Length; i++)
            {
                var avg = students[i].AverageRating;
                if (avg < min) min = avg;
                if (avg > max) max = avg;
            }
            var delta = max - min;
            var min_treshold = min + delta * 0.1;
            var max_treshold = max - delta * 0.1;
            var best = new List<Student>();
            var last = new List<Student>();
            foreach (var stud in students)
            {
                var average_rating = stud.AverageRating;
                {
                    if (average_rating > max_treshold)
                    {
                        best.Add(stud);
                            }
                    else if (average_rating < min_treshold)
                    {
                        last.Add(stud);
                            }
                }
                
               
            }
            Student.WriteToCSV("All_students.csv", students);
            Student.WriteToCSV("Best_students.csv", best.ToArray());
            Student.WriteToCSV("Last_students.csv", last.ToArray());


            // stud.Visualize();
            //Console.ReadLine();
        }
        static Student[] CreateStudents(string[] Names, string[] LastNames, string[] Patronymics, int Count)
        {
            var result = new Student[Count];
            var rnd = new Random();
            for (var i = 0; i < Count; i++)
            {
                var first_name = Names[rnd.Next(0, Names.Length)];
                var last_name = LastNames[rnd.Next(0, LastNames.Length)];
                var patronimyc = Patronymics[rnd.Next(0, Patronymics.Length)];
                var student = new Student();
                student.FirstName = first_name;
                student.LastName = last_name;
                student.Patronymic = patronimyc;
                student.Ratings = GetRandomRatings(rnd, 10, 2, 3, 4, 5);
                result[i] = student;

            }
            return result;
        }

        static int GetRandom(Random rnd, params int[] Variants)
        {
            int index = rnd.Next(0, Variants.Length);
            return Variants[index];
        }

        //массив оценок
        static int[] GetRandomRatings(Random rnd, int Count, params int[] Variants)
        {
            var result = new int[Count];
            for (int i = 0; i < Count; i++)
                result[i] = GetRandom(rnd, Variants);
            return result;
        }

    }

    class Student
    {
        /* public String FirstName;
         public String LastName;
         public String Patronymic;
         public int[] Ratings;*/

        public string FirstName, LastName, Patronymic;
        public int[] Ratings;

        public Student(string LastName, string FirstName, string Patronymic)
        {
            this.LastName = LastName;
            this.FirstName = FirstName;
            this.Patronymic = Patronymic;
        }

        public Student()
        {
        }

        public static void WriteToCSV(string FileName, Student[] Student)
        {
            using (var file = new StreamWriter(FileName, false, Encoding.UTF8))
            {
                file.WriteLine("LastName;FirstName;Patronymic;Ratings");
                for (var i = 0; i < Student.Length; i++)
                {
                    var ratings = string.Join(";", Student[i].Ratings);
                    file.WriteLine("{0};{1};{2};{3}", Student[i].LastName, Student[i].FirstName, Student[i].Patronymic, ratings);
                }
            }
        }

        public static Student[] ReadCSV(String FileName)
        {
            var result = new List<Student>(1000);
            using (var file = new StreamReader(FileName))
            {
                file.ReadLine();
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();
                    var values = line.Split(';');
                    var student = new Student(values[0], values[1], values[2]);
                    var ratings = new int[values.Length - 3];
                    for (var i = 0; i < ratings.Length; i++)
                        ratings[i] = int.Parse(values[i + 3]);
                    student.Ratings = ratings;
                    result.Add(student);

                }
            }
            return result.ToArray();
        }


        public double AverageRating
        {
            get
            {
                /*if (Ratings == null && Ratings.Length == 0)
                    return double.NaN;
                double sum = 0;
                for (int i = 0; i < Ratings.Length; i++)
                sum += Ratings[i];
                return sum / Ratings.Length;*/
                return Ratings.Average();
            }


        }




        public void Visualize()
        {
            Console.Write("{0}, {1}, {2}:", LastName, FirstName, Patronymic);
            if (Ratings != null)
            {
                for (int i = 0; i < Ratings.Length; i++)
                    Console.Write("{0},", Ratings[i]);
            }
            Console.WriteLine();
        }

    }


}
