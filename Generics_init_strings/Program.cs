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

    public class Repository<T>
        where T : class, IEntity, new()
    {
        Dictionary<int, T> repo = new();

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
    }

    static void Main()
    {
        Repository<Product> myRepo = new();
        var newProduct = myRepo.CreateNew();
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
        var nums = new List<int> { 1, 2, 3 };
        var strings = new List<string> { "qwerty", "qwert", "qwer" };
        var products = new List<Product> { cup, cap };
        Console.WriteLine(FindMax(strings));
        Console.WriteLine(FindMax(nums));
        Console.WriteLine(FindMax(products).Id);
    }
}
