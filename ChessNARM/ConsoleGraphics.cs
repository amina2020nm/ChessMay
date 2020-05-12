using System;
namespace Шахматы
{
    public struct CChar : IEquatable<CChar>
    {
        public ConsoleColor Foreground;
        public ConsoleColor Background;
        public char C;
        public CChar(char c = ' ', ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
        {
            Foreground = foreground;
            Background = background;
            C = c;
        }
        public bool Equals(CChar other) { return other.Foreground == Foreground && other.Background == Background && other.C == C; }
        public static bool operator ==(CChar lhs, CChar rhs) { return lhs.Equals(rhs); }
        public static bool operator !=(CChar lhs, CChar rhs) { return !lhs.Equals(rhs); }
    }
    public class ConsoleGraphics
    {
        private CChar[,] backBuffer;
        private CChar[,] frontBuffer;
        public ConsoleGraphics()
        {
            backBuffer = new CChar[Console.BufferWidth, Console.BufferHeight];
            frontBuffer = new CChar[Console.BufferWidth, Console.BufferHeight];
        }
        public void Clear()//Очищаем буфер
        {
            for (int i = 0; i < backBuffer.GetLength(0); i++)
            {
                for (int j = 0; j < backBuffer.GetLength(1); j++)
                {
                    backBuffer[i, j] = new CChar();
                }
            }
        }
        public void Draw(CChar cchar, int x, int y)//Рисует цветной символ в буфере
        {
            backBuffer[x, y] = cchar;
        }
        public void DrawTransparent(char c, ConsoleColor foreground, int x, int y)//Рисует цветной символ в буфере, он не меняет цвет фона
        {
            backBuffer[x, y].C = c;
            backBuffer[x, y].Foreground = foreground;
        }
        public void DrawArea(CChar[,] cchars, int x, int y)//Рисует область цветных символов. Длина массивов используется как ширина и высота области
        {
            for (int i = 0; i < cchars.GetLength(0); i++)
            {
                for (int j = 0; j < cchars.GetLength(1); j++)
                {
                    backBuffer[x + i, y + j] = cchars[i, j];
                }
            }
        }
        public void DrawText(string text, ConsoleColor foreground, ConsoleColor background, int x, int y)// Рисует текст на экране.
        {
            CChar[,] area = new CChar[text.Length, 1];
            for (int i = 0; i < text.Length; i++)
            {
                area[i, 0] = new CChar(text[i], foreground, background);
            }
            DrawArea(area, x, y);
        }
        public void DrawTextTrasparent(string text, ConsoleColor foreground, int x, int y)// Рисует текст на экране с прозрачным фоном
        {
            CChar[,] area = new CChar[text.Length, 1];
            for (int i = 0; i < text.Length; i++)
            {
                area[i, 0] = new CChar(text[i], foreground, backBuffer[x + i, y].Background);
            }
            DrawArea(area, x, y);
        }
        public void FillArea(CChar cchar, int x, int y, int width, int height)// Заполняет область буфера одним цветным символом
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    backBuffer[x + i, y + j] = cchar;
                }
            }
        }
        public void ClearArea(int x, int y, int width, int height)// Очищает область на экране
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    backBuffer[x + i, y + j] = new CChar();
                }
            }
        }
        public void DarkenBackground(int x, int y)//Если фоновый цвет уже темный или нет темной версии, он остается неизменным.
        {
            switch (backBuffer[x, y].Background)
            {
                case ConsoleColor.Blue:
                    backBuffer[x, y].Background = ConsoleColor.DarkBlue;
                    break;
                case ConsoleColor.Green:
                    backBuffer[x, y].Background = ConsoleColor.DarkGreen;
                    break;
                case ConsoleColor.Yellow:
                    backBuffer[x, y].Background = ConsoleColor.DarkYellow;
                    break;
                case ConsoleColor.Magenta:
                    backBuffer[x, y].Background = ConsoleColor.DarkMagenta;
                    break;
                case ConsoleColor.Gray:
                    backBuffer[x, y].Background = ConsoleColor.DarkGray;
                    break;
                case ConsoleColor.Cyan:
                    backBuffer[x, y].Background = ConsoleColor.DarkCyan;
                    break;
                case ConsoleColor.Red:
                    backBuffer[x, y].Background = ConsoleColor.DarkRed;
                    break;
            }
        }
        public void DarkenForeground(int x, int y)
        {
            switch (backBuffer[x, y].Foreground)
            {
                case ConsoleColor.Blue:
                    backBuffer[x, y].Foreground = ConsoleColor.DarkBlue;
                    break;
                case ConsoleColor.Green:
                    backBuffer[x, y].Foreground = ConsoleColor.DarkGreen;
                    break;
                case ConsoleColor.Yellow:
                    backBuffer[x, y].Foreground = ConsoleColor.DarkYellow;
                    break;
                case ConsoleColor.Magenta:
                    backBuffer[x, y].Foreground = ConsoleColor.DarkMagenta;
                    break;
                case ConsoleColor.Gray:
                    backBuffer[x, y].Foreground = ConsoleColor.DarkGray;
                    break;
                case ConsoleColor.Cyan:
                    backBuffer[x, y].Foreground = ConsoleColor.DarkCyan;
                    break;
                case ConsoleColor.Red:
                    backBuffer[x, y].Foreground = ConsoleColor.DarkRed;
                    break;
            }
        }
        // Осветляет цвет фона цветного символа в буфере.Если фоновый цвет уже светлый или нет светлой версии, он остается неизменным
        public void LightenBackground(int x, int y)
        {
            switch (backBuffer[x, y].Background)
            {
                case ConsoleColor.DarkBlue:
                    backBuffer[x, y].Background = ConsoleColor.Blue;
                    break;
                case ConsoleColor.DarkGreen:
                    backBuffer[x, y].Background = ConsoleColor.Green;
                    break;
                case ConsoleColor.DarkYellow:
                    backBuffer[x, y].Background = ConsoleColor.Yellow;
                    break;
                case ConsoleColor.DarkMagenta:
                    backBuffer[x, y].Background = ConsoleColor.Magenta;
                    break;
                case ConsoleColor.DarkGray:
                    backBuffer[x, y].Background = ConsoleColor.Gray;
                    break;
                case ConsoleColor.DarkCyan:
                    backBuffer[x, y].Background = ConsoleColor.Cyan;
                    break;
                case ConsoleColor.DarkRed:
                    backBuffer[x, y].Background = ConsoleColor.Red;
                    break;
            }
        }
        public void LightenForeground(int x, int y)
        {
            switch (backBuffer[x, y].Foreground)
            {
                case ConsoleColor.DarkBlue:
                    backBuffer[x, y].Foreground = ConsoleColor.Blue;
                    break;
                case ConsoleColor.DarkGreen:
                    backBuffer[x, y].Foreground = ConsoleColor.Green;
                    break;
                case ConsoleColor.DarkYellow:
                    backBuffer[x, y].Foreground = ConsoleColor.Yellow;
                    break;
                case ConsoleColor.DarkMagenta:
                    backBuffer[x, y].Foreground = ConsoleColor.Magenta;
                    break;
                case ConsoleColor.DarkGray:
                    backBuffer[x, y].Foreground = ConsoleColor.Gray;
                    break;
                case ConsoleColor.DarkCyan:
                    backBuffer[x, y].Foreground = ConsoleColor.Cyan;
                    break;
                case ConsoleColor.DarkRed:
                    backBuffer[x, y].Foreground = ConsoleColor.Red;
                    break;
            }
        }
        public void SetBackground(ConsoleColor color, int x, int y)//Устанавливает цвет фона буфера в(x, y)
        {
            backBuffer[x, y].Background = color;
        }
        public ConsoleColor GetBackground(int x, int y)//Получает цвет фона буфера в(x, y)
        {
            return backBuffer[x, y].Background;
        }
        public void SetForeground(ConsoleColor color, int x, int y)
        {
            backBuffer[x, y].Foreground = color;
        }
        public ConsoleColor GetForeground(int x, int y)
        {
            return backBuffer[x, y].Foreground;
        }
        /// Перезаписывает фронт и перерисовывает символ, если он отличается от бэка
        public void SwapBuffers()
        {
            for (int i = 0; i < backBuffer.GetLength(0); i++)
            {
                for (int j = 0; j < backBuffer.GetLength(1); j++)
                {
                    if (frontBuffer[i, j] != backBuffer[i, j])
                    {
                        Console.SetCursorPosition(i, j);
                        Console.ForegroundColor = backBuffer[i, j].Foreground;
                        Console.BackgroundColor = backBuffer[i, j].Background;
                        Console.Write(backBuffer[i, j].C);
                        frontBuffer[i, j] = backBuffer[i, j];
                    }
                }
            }
        }
    }
}
