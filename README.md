# TheSleepingTeachingAssistant


A solution for a simple synchronization problem mentioned in Operating System Concepts, by Abraham Silberschatz.


**Description & Rules:**
- A university computer science department has a teaching assistant (TA) who helps undergraduate students with their programming assignments during regular office hours.
- There are three chairs in the hallway outside the office where students can sit and wait if the TA is currently helping another student.
- When there are no students who need help during office hours, the TA sits at the desk and takes a nap.
- If a student arrives during office hours and finds the TA sleeping, the student must awaken the TA to ask for help.
- If the TA is available, they will obtain help. Otherwise, they will either sit in a chair in the hallway or, if no chairs are available, will resume programming and will seek help at a later time.
- TA must help each of these students in turn.


**Synchronization primitives used:**
- semaphore: A semaphore having a maximum number of concurrent entries = 3. Used to represent the three chairs in the hallway.
- Queue<semaphore>: a queue of semaphores (and students' ID's). Used to implement a FIFO order when serving students. 
- Monitor (lock statement): Used to control access to the seat queue.
