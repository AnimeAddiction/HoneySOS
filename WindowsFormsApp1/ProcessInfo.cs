public class ProcessInfo
{
    public string ProcessId { get; set; }
    public int BurstTime { get; set; }
    public int MemorySize { get; set; }
    public int ArrivalTime { get; set; }
    public int Priority { get; set; }
    public string Status { get; set; }

    public ProcessInfo()
    {
    }

    public ProcessInfo(string processId, int burstTime, int memorySize, int arrivalTime, int priority, string status)
    {
        ProcessId = processId;
        BurstTime = burstTime;
        MemorySize = memorySize;
        ArrivalTime = arrivalTime;
        Priority = priority;
        Status = status;
    }

    // Optional: Override ToString() method for easier debugging and logging
    public override string ToString()
    {
        return $"ProcessID: {ProcessId}, BurstTime: {BurstTime}, MemorySize: {MemorySize}, ArrivalTime: {ArrivalTime}, Priority: {Priority}, Status: {Status}";
    }
}