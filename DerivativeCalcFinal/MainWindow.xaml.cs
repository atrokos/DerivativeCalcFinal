using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using WpfMath.Controls;
using CSharpMath.Differentiation;
using AngouriMath;

namespace TutorialWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            char c = 'a';
            for (int i = 0; i < 26; i++)
            {
                DiffVar.Items.Add(Char.ToString(c));
                c++;
            }
            DiffVar.SelectedItem = "x";
            inputexpression.Focus();
        }

        private string prev_input = "";
        private bool prev_state = true;
        private string prev_var = "x";
        private static readonly List<SolidColorBrush> bcolors = new() {Brushes.PaleGreen, Brushes.PaleTurquoise, Brushes.PaleGoldenrod, Brushes.LightSalmon, Brushes.Linen };

        private async void GoButton_Click(object sender, RoutedEventArgs e)
        {
            await Execute();
        }
        private async Task Execute()
        {
            if (prev_input == inputexpression.Text && prev_state == WithSteps.IsChecked && prev_var == DiffVar.Text)
                return;
            prev_input = inputexpression.Text;
            prev_var = DiffVar.Text;
            prev_state = (bool)WithSteps.IsChecked;

            if (WithSteps.IsChecked == false)
            {
                StepScroller.Visibility = Visibility.Hidden;
                StepBox.Visibility = Visibility.Hidden;
                await DifferentiateAsync();
            }
            else
            {
                StepScroller.Visibility = Visibility.Visible;
                StepBox.Visibility = Visibility.Visible;
                await DifferentiateStepsAsync();
            }
        }
        private async void inputexpression_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await Execute();
            }
        }
        private async Task DifferentiateAsync()
        {
            if (inputexpression.Text == "")
                return;
            Progress.Value = 0;
            MathExpression expression = new(inputexpression.Text, DiffVar.Text, false);
            if (!expression.IsDifferentiable())
            {
                StatusBox.Foreground = Brushes.Red;
                StatusBox.Text = "Chyba: Nesprávný syntax";
                return;
            }
            StatusBox.Foreground = Brushes.Black;
            StatusBox.Text = "Derivuji...";
            Progress.Value = 40;
            expression.Differentiate();
            StatusBox.Text = "Zjednodušuji vzorec...";
            Progress.Value = 70;
            await Task.Run(() => expression.Simplify());
            StatusBox.Text = "Sázím...";
            Progress.Value = 90;
            OutputFormula.Formula = await Task.Run(() => expression.ToString());
            Progress.Value = 100;
            StatusBox.Text = "Hotovo";
        }
        private async Task DifferentiateStepsAsync()
        {
            if (inputexpression.Text == "")
                return;


            Progress.Value = 0;
            MathExpression expression = new(inputexpression.Text, DiffVar.Text, true);
            if (!expression.IsDifferentiable())
            {
                StatusBox.Foreground = Brushes.Red;
                StatusBox.Text = "Chyba: Nesprávný syntax";
                return;
            }
            StatusBox.Foreground = Brushes.Black;
            StatusBox.Text = "Derivuji...";
            Progress.Value = 40;
            expression.DifferentiateSteps(); //Change to Differentiate after debugging!
            StatusBox.Text = "Zjednodušuji vzorec...";
            Progress.Value = 55;
            await Task.Run(() => expression.Simplify());
            await Task.Run(() => Dispatcher.Invoke(() => GenerateSteps()));
            OutputFormula.Formula = await Task.Run(() => expression.ToString());
            Progress.Value = 100;
            StatusBox.Text = "Hotovo";
        }

        private void inputexpression_TextChanged(object sender, TextChangedEventArgs e)
        {
            StepPanel.Children.Clear();
            OutputFormula.Formula = "";
            StatusBox.Foreground = Brushes.Black;
            Progress.Value = 0;
            StatusBox.Text = "Připraven";

            string result;
            try
            {
                Entity entity = inputexpression.Text;
                result = entity.Latexise();
            }
            catch
            {
                result = "";
            }
            InputFormula.Formula = result;
        }

        private void GenerateSteps()
        {
            StepGenerator generator = new(DiffVar.Text);
            List<string> steps = generator.StartGenerating();
            int margin = 0;
            int color = 0;

            foreach(string step in steps)
            {
                string[] command = step.Split(' ');
                switch (command[0])
                {
                    case "/math":
                        StepPanel.Children.Add(new FormulaControl() { Formula = step[5..], Margin = new Thickness(margin, 0, 0, 0) , Background = bcolors[color] }); //x,5,0,0
                        break;
                    case "/margin":
                        margin = int.Parse(command[1]);
                        break;
                    case "/background":
                        color = int.Parse(command[1]) % bcolors.Count;
                        break;
                    default:
                        StepPanel.Children.Add(new TextBlock() { Text = step, FontSize = 15, Margin = new Thickness(margin, 0, 0, 0), Background = bcolors[color] }); //x,5,0,10
                        break;
                }
            }
        }
    }
}
