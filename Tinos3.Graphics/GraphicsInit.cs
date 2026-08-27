namespace Tinos3.Graphics
{
    public class GraphicsInit
    {
        public void initGraphics()
        {
            Console.WriteLine("GRAPHICS: Initializing graphics");
            Console.WriteLine("GRAPHICS: Running initial display detection");
            DisplayDetection displayDetection = new DisplayDetection();

            displayDetection.InitialDisplayDetection();

            Console.WriteLine("GRAPHICS: Graphics initialized");
        }
    }
}
