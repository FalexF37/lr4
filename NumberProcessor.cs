using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RandomStringNumbers;

public class TaskResult
{
    public List<string> LinqResult { get; set; } = new();
    public long LinqTimeMs { get; set; }
    public List<string> PlinqResult { get; set; } = new();
    public long PlinqTimeMs { get; set; }
}

public class NumberProcessor
{
    private readonly Random _rnd = new();

    // Генерируем строки длиной от 1 до 10 цифр включительно
    public List<string> Generate(int count)
    {
        var result = new HashSet<string>();

        // Генерируем, пока не наберем нужное количество уникальных чисел
        while (result.Count < count)
        {
            int length = _rnd.Next(1, 11);
            var sb = new StringBuilder(length);
            sb.Append((char)('1' + _rnd.Next(9)));
            for (int j = 1; j < length; j++)
                sb.Append((char)('0' + _rnd.Next(10)));

            result.Add(sb.ToString());
        }

        return result.ToList();
    }

    // Вспомогательный метод для правильной сортировки числовых строк любой длины
    private static IEnumerable<string> SortDescending(IEnumerable<string> source)
    {
        // Сначала сортируем по длине (кто длиннее - тот больше), 
        // затем по значению строки (по убыванию).
        return source.OrderByDescending(n => n.Length)
                     .ThenByDescending(n => n);
    }

    // 1. Четные числа (проверяем последнюю цифру, чтобы избежать переполнения)
    public TaskResult GetEvens(List<string> source)
    {
        var sw = Stopwatch.StartNew();
        var linq = SortDescending(source.Where(n => (n[^1] - '0') % 2 == 0)).ToList();
        sw.Stop();

        var result = new TaskResult { LinqResult = linq, LinqTimeMs = sw.ElapsedMilliseconds };

        sw.Restart();
        var plinq = SortDescending(source.AsParallel().Where(n => (n[^1] - '0') % 2 == 0)).ToList();
        sw.Stop();

        result.PlinqResult = plinq;
        result.PlinqTimeMs = sw.ElapsedMilliseconds;
        return result;
    }

    // 2. Нечетные числа
    public TaskResult GetOdds(List<string> source)
    {
        var sw = Stopwatch.StartNew();
        var linq = SortDescending(source.Where(n => (n[^1] - '0') % 2 != 0)).ToList();
        sw.Stop();

        var result = new TaskResult { LinqResult = linq, LinqTimeMs = sw.ElapsedMilliseconds };

        sw.Restart();
        var plinq = SortDescending(source.AsParallel().Where(n => (n[^1] - '0') % 2 != 0)).ToList();
        sw.Stop();

        result.PlinqResult = plinq;
        result.PlinqTimeMs = sw.ElapsedMilliseconds;
        return result;
    }

    // 3. Сумма второй и предпоследней цифры == 6 (строго больше 3 цифр)
    public TaskResult GetSpecialSum(List<string> source)
    {
        // n.Length > 3 гарантирует, что в строке есть как минимум 4 символа.
        // Значит n[1] и n[^2] (предпоследний) точно существуют.
        static bool Condition(string n) => n.Length > 3 && ((n[1] - '0') + (n[^2] - '0') == 6);

        var sw = Stopwatch.StartNew();
        var linq = SortDescending(source.Where(Condition)).ToList();
        sw.Stop();

        var result = new TaskResult { LinqResult = linq, LinqTimeMs = sw.ElapsedMilliseconds };

        sw.Restart();
        var plinq = SortDescending(source.AsParallel().Where(Condition)).ToList();
        sw.Stop();

        result.PlinqResult = plinq;
        result.PlinqTimeMs = sw.ElapsedMilliseconds;
        return result;
    }

    // 4. Сумма всех цифр == 13
    public TaskResult GetDigitSum13(List<string> source)
    {
        static bool Condition(string n) => n.Sum(c => c - '0') == 13;

        var sw = Stopwatch.StartNew();
        var linq = SortDescending(source.Where(Condition)).ToList();
        sw.Stop();

        var result = new TaskResult { LinqResult = linq, LinqTimeMs = sw.ElapsedMilliseconds };

        sw.Restart();
        var plinq = SortDescending(source.AsParallel().Where(Condition)).ToList();
        sw.Stop();

        result.PlinqResult = plinq;
        result.PlinqTimeMs = sw.ElapsedMilliseconds;
        return result;
    }
}