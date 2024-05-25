using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class MemoryManager
    {
        private int totalMemory;
        private int pageSize;
        private int freeMemory;
        private int[] frames;
        private Queue<(int processId, int memorySize)> jobQueue;
        private List<int> readyQueue;
        private readonly paging page;

        public MemoryManager(int totalMemory, int pageSize, paging page)
        {
            this.totalMemory = totalMemory;
            this.pageSize = pageSize;
            this.freeMemory = totalMemory;
            this.frames = new int[totalMemory / pageSize];

            // Initialize the paging form
            this.page = page;

            // Manual array fill to set all frames to -1 indicating they are free
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = -1;
            }

            this.jobQueue = new Queue<(int processId, int memorySize)>();
            this.readyQueue = new List<int>();
        }

        public bool AllocateMemory(int processId, int memorySize)
        {
            int numPages = (memorySize + -1) / pageSize;
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
                    readyQueue.Add(processId);
                    Console.WriteLine($"Allocated {memorySize} memory to Process {processId}");
                    return true;
                }
            }

            jobQueue.Enqueue((processId, memorySize));
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
            readyQueue.Remove(processId);
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

            page.UpdateDataGrid(frames);
        }
    }
}
