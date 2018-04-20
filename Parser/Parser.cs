using System;
using System.Linq;

namespace parser
{
    public class Parser
    {
        // Функция в стринговом виде
        string func;
        // Функция разбитая пробелами
        string[] s_Function;
        // Аргумент функции
        double x;

        // Конструктор
        public Parser(string input)
        {
            func = input;
            s_Function = input.Split(' ');
        }

        Parser(string[] input, int indexOpenBold, int indexCloseBold)
        {
            s_Function = new string[indexCloseBold - indexOpenBold - 1];
            for (int i = 1; i <= s_Function.Length; i++) s_Function[i - 1] = input[indexOpenBold + i];
        }

        // Функция замены x
        double X() { return x; }
        // Сложение a и b
        double Add(double a, double b) { return a + b; }
        // Вычитание b из a
        double Sub(double a, double b) { return a - b; }
        // Умножение a и b
        double Mul(double a, double b) { return a * b; }
        // Деление a на b
        double Div(double a, double b) { if (b == 0) { throw new DivideByZeroException(); } else return a / b; }

        // Возведение a в степень b
        double Pow(double a, double b) { return Math.Pow(a, b); }

        // Pi
        double Pi() { return Math.PI; }
        // Экспонента
        double Exp() { return Math.E; }

        // Синус от x
        double Sin(double x) { return Math.Sin(Math.PI * x / 180); }
        // Косинус от x
        double Cos(double x) { return Math.Cos(Math.PI * x / 180); }
        // Тангенс от x
        double Tan(double x) { return Math.Tan(Math.PI * x / 180); }
        // Котангенс от x
        double Cot(double x) { return 1 / Math.Tan(Math.PI * x / 180); }

        // Выполнение действия типа operation, согласно функции func
        void DoMath(string operation, Func<double, double, double> func)
        {
            int i = 0;
            do
            {
                if (s_Function[i] == operation)
                {
                    // Вычисляет значение func и записывает результат в ячейку перед действием
                    s_Function[i - 1] = func(Convert.ToDouble(s_Function[i - 1]), Convert.ToDouble(s_Function[i + 1])).ToString();
                    // Сдвигает массив, чтобы закрыть ячейку действия
                    for (int j = i; j < s_Function.Length - 1; j++)
                    {
                        s_Function[j] = s_Function[j + 1];
                    }
                    // Сдвигает массив, чтобы закрыть ячейку второго параметра
                    for (int j = i; j < s_Function.Length - 1; j++)
                    {
                        s_Function[j] = s_Function[j + 1];
                    }
                    s_Function[s_Function.Length - 1] = "";
                    s_Function[s_Function.Length - 2] = "";
                    i = 0;
                }
                else i++;
            } while (i < s_Function.Length);
        }
        // Вычисление
        public double calcFun(double arg)
        {
            s_Function = func.Split(' ');
            x = arg;

            // Кол-во открывающихся и закрывающихся скобок
            int oB = 0, cB = 0;

            // Замена констант
            for (int i = 0; i < s_Function.Length; i++)
            {
                if (s_Function[i] == "x") s_Function[i] = X().ToString();
                if (s_Function[i].ToLower() == "pi") s_Function[i] = Pi().ToString();
                if (s_Function[i].ToLower() == "e") s_Function[i] = Exp().ToString();
                // Подсчёт открывающих и закрывающих скобок
                if (s_Function[i].Contains("(")) { oB++; }
                if (s_Function[i].Contains(")")) { cB++; }
            }
            if (oB != cB) throw new Exception("Неверно расставлены скобки.");

            if (oB * cB != 0)
            {
                // Баланс скобок, индекс открывающей скобки, индекс закрывающей
                int bB = 0, ioB = s_Function.Length, icB = -1;
                int i = 0;
                do
                {
                    // Ищет индекс самой первой скобки
                    if (s_Function[i].Contains("("))
                    {
                        bB++;
                        if (i < ioB) ioB = i;
                    }
                    // Ищет индекс самой последней скобки
                    if (s_Function[i].Contains(")"))
                    {
                        bB--;
                        icB = i;
                    }
                    // Если есть не парные закрывающие скобки
                    if (bB < 0) throw new Exception("Неверно расставлены скобки.");

                    // Когда находится самая большая пара скобок - внутри вычисляется значение
                    if (bB == 0 && ioB < icB)
                    {
                        // Вызывается рекурсивно функция подсчёта для скобки
                        Parser p = new Parser(s_Function, ioB, icB);
                        // Подсчёт sin, cos, и т.д.
                        switch (s_Function[ioB].ToLower())
                        {
                            case "sin(":
                                {
                                    s_Function[ioB] = Sin(p.calcFun(x)).ToString();
                                    break;
                                }
                            case "cos(":
                                {
                                    s_Function[ioB] = Cos(p.calcFun(x)).ToString();
                                    break;
                                }
                            case "tan(":
                                {
                                    s_Function[ioB] = Tan(p.calcFun(x)).ToString();
                                    break;
                                }
                            case "cot(":
                                {
                                    s_Function[ioB] = Cot(p.calcFun(x)).ToString();
                                    break;
                                }
                            case "(":
                                {
                                    s_Function[ioB] = p.calcFun(x).ToString();
                                    break;
                                }
                            default:
                                {
                                    throw new Exception("Не найдена функция " + s_Function[ioB]);
                                }
                        }
                        // Замещение ячеек массива
                        for (int j = ioB + 1; j <= icB; j++)
                        {
                            s_Function[j] = "";
                        }
                        for (int j = 0; j < s_Function.Length; j++)
                        {
                            if (s_Function[j] == "")
                            {
                                int k = 1;
                                do
                                {
                                    if (j + k >= s_Function.Length) break;
                                    s_Function[j] = s_Function[j + k];
                                    s_Function[j + k] = "";
                                    k++;
                                } while (s_Function[j] == "");
                            }
                        }
                        ioB = s_Function.Length;
                        icB = -1;
                        i = 0;
                    }
                    else i++;

                } while (i < s_Function.Length);
            }

            // Выполнение всех действий
            DoMath("^", Pow);
            DoMath("*", Mul);
            DoMath("/", Div);
            DoMath("+", Add);
            DoMath("-", Sub);

            return Convert.ToDouble(s_Function[0]);
        }
    }
}
