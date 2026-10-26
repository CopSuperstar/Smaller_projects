public class Footballplayer
{
    public interface IBreathe { }

    public interface ICharge { }

    protected abstract class Player
    {
        public enum Position
        {
            GoalKeeper,
            Defender,
            Midfielder,
            Forward,
        }

        public Position PlayerPosition;

        public int vision;
        public int height;
        public int speed;
        public string? name;

        protected Player(Position pos, int v, int h, int s, string n)
        {
            PlayerPosition = pos;
            vision = v;
            height = h;
            speed = s;
            name = n;
        }
    }

    protected abstract class Humanplayer : Player, IBreathe
    {
        public Humanplayer(Position position, int vision, int height, int speed, string name)
            : base(position, vision, height, speed, name) { }

        public struct baseStats
        {
            enum Foot
            {
                Left,
                Right,
            }

            int endurance;
        }

        public abstract void celebration();
    }

    abstract class Robotplayer(
        Player.Position position,
        int vision,
        int height,
        int speed,
        string name
    ) : Player(position, vision, height, speed, name), ICharge { }

    class Defender(Player.Position position, int vision, int height, int speed, string name)
        : Humanplayer(position, vision, height, speed, name)
    {
        public override void celebration()
        {
            Console.WriteLine("Defender celebrates");
        }
    }

    public static void Run()
    {
        Humanplayer Bob = new Defender(Player.Position.Defender, 10, 8, 9, "Jaxon");
        Bob.celebration();
    }
}
