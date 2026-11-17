using System.Collections;
using System.Text;

public class Programm
{
    public static T? FindMax<T>(List<T> items)
        where T : IComparable<T>
    {
        T current = items[0];
        for (int i = 0; i < items.Count; i++)
        {
            if (current.CompareTo(items[i]) < 0)
            {
                current = items[i];
            }
        }
        return current;
    }

    public class Product : IEntity, IComparable<Product>
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public bool Sold { get; set; }

        public override string ToString()
        {
            return $"{Price}, {Sold}";
        }

        public int CompareTo(Product? other)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                return this.Price.CompareTo(other.Price);
            }
        }
    }

    public interface IEntity
    {
        public int Id { get; set; }
    }

    public class Repository<T> : IEnumerable<T>
        where T : class, IEntity, new()
    {
        private readonly Dictionary<int, T> repo = new();

        public IEnumerator<T> GetEnumerator()
        {
            return repo.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (!repo.TryAdd(item.Id, item))
            {
                throw new ArgumentException("this id is occupied");
            }
        }

        public T? GetById(int id)
        {
            repo.TryGetValue(id, out T? foundObject);
            return foundObject;
        }

        public T CreateNew()
        {
            T item = new T();
            item.Id = repo.Count + 1;
            Add(item);
            return item;
        }

        public void AddRange(IEnumerable<T> range)
        {
            ArgumentNullException.ThrowIfNull(range);
            foreach (T addition in range)
            {
                Add(addition);
            }
        }

        public IEnumerable<T> GetAll() => repo.Values;

        public string ExportInCsv()
        {
            var sb = new StringBuilder(repo.Count * 10);
            sb.Append("id, Price, Sold");
            sb.AppendLine();
            foreach (var obj in this)
            {
                sb.Append(obj.Id).Append(", ").Append(obj).AppendLine();
                // sb.Append($"{obj.Id}, {obj}");
                // sb.AppendLine();
            }
            sb.AppendLine();
            sb.Append($"Overall Count is {repo.Count()}");
            return sb.ToString();
        }
    }

    static void Main()
    {
        Repository<Product> myRepo = new();
        var cup = new Product
        {
            Id = 1,
            Price = 100,
            Sold = false,
        };
        var cap = new Product
        {
            Id = 2,
            Price = 200,
            Sold = false,
        };
        myRepo.Add(cup);
        myRepo.Add(cap);
        Console.WriteLine(myRepo.ExportInCsv());
    }
}
