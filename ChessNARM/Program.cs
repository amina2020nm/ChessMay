using System;
namespace Шахматы
{
    public class Program
    {
        static Chess game;
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.CursorVisible = false;
            ConsoleGraphics graphics = new ConsoleGraphics();
            game = new Chess();
            do
            {
                game.Draw(graphics);
                graphics.SwapBuffers();
                game.Update();
            } while (game.Running);
            Console.Read();
        }
    }
}
