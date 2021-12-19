using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSleepingTeachingAssistant
{
    public class Teacher
    {
        private Queue<(SemaphoreSlim, int)> StudentsOnSeatSema { get; }
        private SemaphoreSlim TeacherServiceSema { get; }
        private object SeatLock { get; }

        private Random Random { get; }
        public Teacher(Queue<(SemaphoreSlim, int)> studentsOnSeatSema, SemaphoreSlim teacherServiceSema, Object seatLock, Random random)
        {
            StudentsOnSeatSema = studentsOnSeatSema;
            TeacherServiceSema = teacherServiceSema;
            SeatLock = seatLock;
            Random = random;
        }


        public async Task Activate()
        {
            while (true)
            {
                await ServeStudents();
            }
        }

        
        private async Task ServeStudents()
        {
            // wait for a student to be available on seat
            TeacherServiceSema.Wait();


            SemaphoreSlim currentStudentSema;
            int studentId = -1;
            lock (SeatLock)
            {
                (currentStudentSema, studentId) = StudentsOnSeatSema.Dequeue();
            }
            Console.WriteLine($"Teacher saw that student {studentId} need service");

            // simulate serving a student for a random period of time

            await Task.Delay(Random.Next(3, 10) * 1000);

            currentStudentSema.Release(); // the student can go back to "programming" again
            Console.WriteLine($"Teacher finished Serving student {studentId}");

        }




        
    }
}
