public class MyQueue<T>
{
    private T[] items = new T[4];

    int head = 0;
    int tail = 0;
    int count = 0;

    public void Dump() =>
        Console.WriteLine(
            $"[{string.Join(",", items)}] head = {head}, tail = {tail}, count = {count}"
        );

    public void Enqueue(T item)
    {
        if (count == items.Length)
        {
            var bigger = new T[items.Length * 2];
            for (int i = 0; i < items.Length; i++)
            {
                bigger[i] = items[(head + i) % items.Length];
            }
            items = bigger;
            head = 0;
            tail = count;
        }

        items[tail] = item;

        tail = (tail + 1) % items.Length;
        count++;
    }

    public T Dequeue()
    {
        if (count == 0)
            throw new InvalidOperationException("Queue is empty");
        T r = items[head];
        items[head] = default!;
        head = (head + 1) % items.Length;
        count--;
        return r;
    }

    public int Count => count;
    public int PhysicalSize => items.Length;
}

class Program
{
    static void Main()
    {
        MyQueue<int> q = new();
        q.Dump();
        q.Enqueue(1);
        q.Dump();
        q.Enqueue(2);
        q.Dump();
        q.Enqueue(3);
        q.Dump();
        q.Enqueue(4);
        q.Dump();
        q.Dequeue();
        q.Dump();
        q.Enqueue(5);
        q.Dump();
        q.Enqueue(6);
        q.Dump();
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine(q.Dequeue());
            q.Dump();
        }
    }
}
