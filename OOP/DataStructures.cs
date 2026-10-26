class refandout
{
    public struct Spaceship
    {
        public int X;
        public int Y;
    }

    public class PhysicsEngine
    {
        public static void ApplyThrust(
            ref Spaceship player,
            int velocityMultiplier,
            out int totalSpeed
        )
        {
            player.X *= velocityMultiplier;
            player.Y *= velocityMultiplier;
            totalSpeed = player.X + player.Y;
        }
    }

    public static void Run()
    {
        Spaceship player = new Spaceship { X = 10, Y = 20 };
        int velocityMultiplier = 3;

        // Call your method here
        PhysicsEngine.ApplyThrust(ref player, velocityMultiplier, out int totalSpeed);
        Console.WriteLine(player.X);
        Console.WriteLine(player.Y);
    }
}
