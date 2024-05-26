using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class MemoryManager
    {
        public int totalMemory;
        public int pageSize;
        public int freeMemory;
        public int[] frames;
        public Queue<(int processId, int memorySize)> jobQueue;  // Job queue
        public List<int> readyQueue;  // Ready queue
        //public List<int> jobQueuee;
        public List<(int processId, int memorySize)> jobQueuee;
        private paging page;

        public MemoryManager(int totalMemory, int pageSize, paging page)
        {
            this.totalMemory = totalMemory;
            this.pageSize = pageSize;
            this.freeMemory = totalMemory;
            this.frames = new int[totalMemory / pageSize];

            // Initialize all frames to -1 indicating they are free
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = -1;
            }

            this.jobQueue = new Queue<(int processId, int memorySize)>();  // Initialize job queue
            this.readyQueue = new List<int>();  // Initialize ready queue
            this.jobQueuee = new List<(int processId, int memorySize)>();  // Initialize job queue
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
                    readyQueue.Add(processId);  // Add to ready queue
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
            readyQueue.Clear();

            Console.WriteLine("Cleared all frames and reset memory.");

            // Update the DataGridView on the UI thread
            page.Invoke((MethodInvoker)delegate
            {
                UpdateQueues();
                VisualizeMemory();
            });
        }

        public bool CheckAvailableMemory(int memorySize)
        {
            return (freeMemory) >= memorySize;
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
            readyQueue.Remove(processId);  // Remove from ready queue
            UpdateQueues(); // Update the display of queues
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
        public int CalculateTotalMemoryUsed()
        {
            int usedMemory = 0;
            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != -1)
                {
                    usedMemory += pageSize;
                }
            }
            return usedMemory;
        }
        private void UpdateQueues()
        {
            if (page.InvokeRequired)
            {
                // If we're not on the UI thread, invoke this method on the UI thread
                page.Invoke((MethodInvoker)delegate
                {
                    page.UpdateQueueGrid(jobQueue, readyQueue);
                });
            }
            else
            {
                // If we're already on the UI thread, update the queues directly
                page.UpdateQueueGrid(jobQueue, readyQueue);
            }
        }
    }
}
