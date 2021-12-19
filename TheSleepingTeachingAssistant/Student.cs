using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSleepingTeachingAssistant
{
    public class Student
    {
        public int Id { get; }
        private Queue<(SemaphoreSlim, int)> StudentsOnSeatSema { get; }
        private SemaphoreSlim TeacherServiceSema { get; }
        private object SeatLock { get; }
        private Random Random { get; }

        public Student(int id, Queue<(SemaphoreSlim, int)> studentsOnSeatSema, SemaphoreSlim teacherServiceSema, Object seatLock, Random random)
        {
            Id = id;
            StudentsOnSeatSema = studentsOnSeatSema;
            TeacherServiceSema = teacherServiceSema;
            SeatLock = seatLock;
            Random = random;
        }


        /// <summary>
        /// Simulates a student continuously transfering between "programming" and "beingServed" states.
        /// </summary>
        /// <returns></returns>
        public async Task Activate()
        {
            while (true)
            {
                // Simulate "programming" for a random period of time
                var ms = Random.Next(10, 40) * 1000;

                Console.WriteLine($"Student {Id} is going to programm for {ms/1000}sec");
                await Task.Delay(ms);


                Console.WriteLine($"Student {Id} finished programming, asking for teacher service");
                SemaphoreSlim studentSima;
                try
                {
                    studentSima = AskForTeacherService();
                }
                catch (SemaphoreFullException ex)
                {
                    // if seat is full, go back to "programming again"
                    Console.WriteLine($"Student {Id} found the seat full");
                    continue;
                }

                // if seat is not full, students waits untill he/she gets served by the teacher
                Console.WriteLine($"Student {Id} is on the seat now");

                studentSima.Wait();

            }
        }

        /// <summary>
        /// Simulates a student checking if the students' seat is available. If the seat is available, the student is queued on the
        /// seat and semaphoreSlim representing this student is returned. else, an exception is thorwn
        /// </summary>
        /// <returns>SemaphoreSlim representing the student waiting for the service</returns>
        /// <exception cref="SemaphoreFullException">Seat is full</exception>
        private SemaphoreSlim AskForTeacherService()
        {
            SemaphoreSlim studentSima;

            lock (SeatLock)
            {
                // thorws an exception if the seat is full
                // the lock is released even if an exception is thrown within the body of a lock statement
                TeacherServiceSema.Release();
                studentSima = new SemaphoreSlim(0, 1);

                StudentsOnSeatSema.Enqueue((studentSima, Id));
                Console.WriteLine(StudentsOnSeatSema.Count);
            }

            return studentSima;
        }

    }
}
