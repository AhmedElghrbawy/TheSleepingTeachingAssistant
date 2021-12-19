using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSleepingTeachingAssistant
{
    public class School
    {
        private const int STUDENTS_NUM = 20;
        private Queue<(SemaphoreSlim, int)> StudentsOnSeatSema { get; } = new ();
        private SemaphoreSlim TeacherServiceSema { get; } = new SemaphoreSlim(0, 3);
        private object SeatLock { get; } = new object ();
        private Random Random { get; } = new Random ();

        public async Task Start()
        {
            var teacherTask = Task.Run(() => new Teacher(StudentsOnSeatSema, TeacherServiceSema, SeatLock, Random).Activate());
            var studentsTasks = Enumerable.Range(0, STUDENTS_NUM).Select(i =>
            {
                return Task.Run(() =>
                {
                    new Student(i, StudentsOnSeatSema, TeacherServiceSema, SeatLock, Random).Activate();
                });
            }).ToList();

            await Task.WhenAll(studentsTasks);
            await teacherTask;
        }


    }
}
