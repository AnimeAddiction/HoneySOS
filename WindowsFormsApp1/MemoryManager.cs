using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class MemoryManager
    {
        private int totalMemory;
        private int pageSize;
        private int freeMemory;
        private int[] frames;
        public Queue<(int processId, int memorySize)> jobQueue;  // Job queue
        public Dictionary<int, int> readyMemoryMap;  // Dictionary to store memory size for processes in the ready queue
        private paging page;

        public MemoryManager(int totalMemory, int pageSize, paging page)
        {
            this.totalMemory = totalMemory;
            this.pageSize = pageSize;
            this.freeMemory = totalMemory;
            this.frames = new int[totalMemory / pageSize];
            this.readyMemoryMap = new Dictionary<int, int>();  // Initialize memory map for processes in the ready queue

            // Initialize all frames to -1 indicating they are free
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = -1;
            }

            this.jobQueue = new Queue<(int processId, int memorySize)>();  // Initialize job queue
            this.page = page;
        }

        public bool AllocateMemory(int processId, int memorySize)
        {
            int numPages = (memorySize + pageSize - 1) / pageSize;
            if (freeMemory >= memorySize)
            {
                List<int> allocatedPages = new List<int>();
                for (int i = 0; i < frames.Length && allocatedPages.Count < numPages; i++)
                {
                    if (frames[i] == -1)
                    {
                        frames[i] = processId;
                        allocatedPages.Add(i);
                    }
                }

                if (allocatedPages.Count == numPages)
                {
                    freeMemory -= memorySize;
                    readyMemoryMap.Add(processId, memorySize);  // Add memory size to readyMemoryMap
                    UpdateQueues(); // Update the display of queues
                    Console.WriteLine($"Allocated {memorySize} memory to Process {processId}");
                    return true;
                }
            }

            jobQueue.Enqueue((processId, memorySize));  // Add to job queue
            UpdateQueues(); // Update the display of queues
            Console.WriteLine($"Process {processId} added to job queue due to insufficient memory.");
            return false;
        }

        public void DeallocateMemory(int processId)
        {
            int freedMemory = 0;
            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] == processId)
                {
                    frames[i] = -1;
                    freedMemory += pageSize;
                }
            }

            freeMemory += freedMemory;
            readyMemoryMap.Remove(processId);  // Remove memory size from readyMemoryMap

            Console.WriteLine($"Deallocated memory from Process {processId}, freed {freedMemory} memory.");

            if (jobQueue.Count > 0)
            {
                var nextJob = jobQueue.Peek();
                if (freeMemory >= nextJob.memorySize)
                {
                    jobQueue.Dequeue();
                    AllocateMemory(nextJob.processId, nextJob.memorySize);
                }
            }
            UpdateQueues(); // Update the display of queues
        }

        public void ClearFrames()
        {
            // Reset all frames to -1 indicating they are free
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = -1;
            }

            // Reset freeMemory to the total memory
            freeMemory = totalMemory;

            // Clear job queue
            jobQueue.Clear();

            // Clear readyMemoryMap
            readyMemoryMap.Clear();

            Console.WriteLine("Cleared all frames and reset memory.");

            // Update the DataGridView on the UI thread
            page.Invoke((MethodInvoker)delegate
            {
                UpdateQueues();
                VisualizeMemory();
            });
        }

        public void VisualizeMemory()
        {
            Console.WriteLine("Memory Frames:");
            for (int i = 0; i < frames.Length; i++)
            {
                Console.Write(frames[i] == -1 ? "[Free]" : $"[P{frames[i]}]");
                if ((i + 1) % 8 == 0)
                    Console.WriteLine();
                else
                    Console.Write(" ");
            }
            Console.WriteLine("\n");

            // Update the DataGridView on the UI thread
            page.Invoke((MethodInvoker)delegate
            {
                page.UpdateDataGrid(frames);
            });
        }

        private void UpdateQueues()
        {
            if (page.InvokeRequired)
            {
                // If we're not on the UI thread, invoke this method on the UI thread
                page.Invoke((MethodInvoker)delegate
                {
                    page.UpdateQueueGrid(jobQueue, readyMemoryMap);
                });
            }
            else
            {
                // If we're already on the UI thread, update the queues directly
                page.UpdateQueueGrid(jobQueue, readyMemoryMap);
            }
        }

        public void FCFS()
        {
            if (jobQueue.Count > 0)
            {
                int memorySize = jobQueue.Peek().memorySize;
                if (freeMemory >= memorySize)
                {
                    var nextJob = jobQueue.Dequeue();
                    if (AllocateMemory(nextJob.processId, nextJob.memorySize))
                    {
                        Console.WriteLine($"Allocated memory to Process {nextJob.processId} using FCFS.");
                    }
                    else
                    {
                        Console.WriteLine($"Insufficient memory for Process {nextJob.processId}.");
                    }
                }
                else
                {
                    UpdateQueues();
                }
                
            }
        }

        public void SJF(List<ProcessInfo> processes)
        {
            // Create a temporary list to hold jobs from the job queue
            List<ProcessInfo> tempJobs = new List<ProcessInfo>(processes);

            // Sort the temporary list based on burst time (memory size)
            tempJobs.Sort((job1, job2) => job1.BurstTime.CompareTo(job2.BurstTime));

            ClearFrames();  
            readyMemoryMap.Clear();
            freeMemory = totalMemory;

            // Iterate through the sorted list and try to allocate memory to each job
            foreach (var job in tempJobs)
            {
                
                    UpdateQueues();
                    // If there is enough free memory, allocate memory to the job
                    if (AllocateMemory(int.Parse(job.ProcessId), job.MemorySize))
                    {

                        Console.WriteLine($"Allocated memory to Process {job.ProcessId} using SJF.");
                        UpdateQueues();
                       // Exit the method after allocating memory to the first suitable job
                    }
              
            }

            // If no suitable job is found, update the queues without allocating memory
            UpdateQueues();
        }

        public void Priority(List<ProcessInfo> processes)
        {
            // Create a temporary list to hold jobs from the job queue
            List<ProcessInfo> tempJobs = new List<ProcessInfo>(processes);

            // Sort the temporary list based on priority
            tempJobs.Sort((job1, job2) => job1.Priority.CompareTo(job2.Priority));

            ClearFrames();
            readyMemoryMap.Clear();
            freeMemory = totalMemory;

            // Iterate through the sorted list and try to allocate memory to each job
            foreach (var job in tempJobs)
            {
                UpdateQueues();
                // If there is enough free memory, allocate memory to the job
                if (AllocateMemory(int.Parse(job.ProcessId), job.MemorySize))
                {
                    Console.WriteLine($"Allocated memory to Process {job.ProcessId} using Priority.");
                    UpdateQueues();
                    // Exit the method after allocating memory to the first suitable job
                }
            }

            // If no suitable job is found, update the queues without allocating memory
            UpdateQueues();
        }



    }
}
