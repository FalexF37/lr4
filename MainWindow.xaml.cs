using System.Collections.Generic;
using System.Windows;

namespace RandomStringNumbers;

public partial class MainWindow : Window
{
    private readonly NumberProcessor _processor = new();
    private List<string> _sourceData = new(); // Здесь храним сгенерированный массив

    public MainWindow()
    {
        InitializeComponent();
    }

    // Шаг 1: Генерация
    private void BtnGenerate_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(TxtCount.Text, out int count) || count <= 0)
        {
            MessageBox.Show("Введите корректное число N.");
            return;
        }

        TxtStatus.Text = "Генерация...";
        BtnProcess.IsEnabled = false;

        // Генерируем массив
        _sourceData = _processor.Generate(count);

        // Показываем его пользователю
        LstSource.ItemsSource = null;
        LstSource.ItemsSource = _sourceData;
        TxtSourceInfo.Text = $"Исходный массив ({_sourceData.Count} элементов):";

        // Очищаем старые результаты (если были)
        ClearResults();

        BtnProcess.IsEnabled = true;
        TxtStatus.Text = "Массив сгенерирован. Можно выполнять анализ.";
    }

    // Шаг 2: Анализ (LINQ и PLINQ)
    private void BtnProcess_Click(object sender, RoutedEventArgs e)
    {
        if (_sourceData.Count == 0)
        {
            MessageBox.Show("Сначала сгенерируйте массив!");
            return;
        }

        TxtStatus.Text = "Обработка запросов...";
        BtnProcess.IsEnabled = false; // Блокируем кнопку, чтобы не нажать дважды

        // Выполняем запросы
        var res1 = _processor.GetEvens(_sourceData);
        var res2 = _processor.GetOdds(_sourceData);
        var res3 = _processor.GetSpecialSum(_sourceData);
        var res4 = _processor.GetDigitSum13(_sourceData);

        // Выводим результаты в UI
        UpdateUI(res1, Lst1Linq, TxtTime1Linq, Lst1Plinq, TxtTime1Plinq);
        UpdateUI(res2, Lst2Linq, TxtTime2Linq, Lst2Plinq, TxtTime2Plinq);
        UpdateUI(res3, Lst3Linq, TxtTime3Linq, Lst3Plinq, TxtTime3Plinq);
        UpdateUI(res4, Lst4Linq, TxtTime4Linq, Lst4Plinq, TxtTime4Plinq);

        BtnProcess.IsEnabled = true;
        TxtStatus.Text = "Анализ завершен! Переключите вкладки для просмотра.";
    }

    private void UpdateUI(TaskResult result,
        System.Windows.Controls.ListBox lstLinq, System.Windows.Controls.TextBlock txtLinq,
        System.Windows.Controls.ListBox lstPlinq, System.Windows.Controls.TextBlock txtPlinq)
    {
        lstLinq.ItemsSource = result.LinqResult;
        txtLinq.Text = $"LINQ: {result.LinqTimeMs} мс (Найдено: {result.LinqResult.Count})";

        lstPlinq.ItemsSource = result.PlinqResult;
        txtPlinq.Text = $"PLINQ: {result.PlinqTimeMs} мс (Найдено: {result.PlinqResult.Count})";
    }

    private void ClearResults()
    {
        Lst1Linq.ItemsSource = Lst1Plinq.ItemsSource = null;
        TxtTime1Linq.Text = TxtTime1Plinq.Text = "";

        Lst2Linq.ItemsSource = Lst2Plinq.ItemsSource = null;
        TxtTime2Linq.Text = TxtTime2Plinq.Text = "";

        Lst3Linq.ItemsSource = Lst3Plinq.ItemsSource = null;
        TxtTime3Linq.Text = TxtTime3Plinq.Text = "";

        Lst4Linq.ItemsSource = Lst4Plinq.ItemsSource = null;
        TxtTime4Linq.Text = TxtTime4Plinq.Text = "";
    }
}