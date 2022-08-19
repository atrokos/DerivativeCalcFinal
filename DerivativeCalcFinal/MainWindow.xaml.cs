using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private int diff_pos = 1;
        private MathExpression expression = null;
        private static readonly List<SolidColorBrush> bcolors = new() {Brushes.PaleGreen, Brushes.PaleTurquoise, Brushes.PaleGoldenrod, Brushes.LightSalmon, Brushes.Linen };

        private async void GoButton_Click(object sender, RoutedEventArgs e)
        {
            string input = inputexpression.Text, var = DiffVar.Text;
            bool isChecked = (bool)WithSteps.IsChecked;
            await Task.Run(() => Execute(input, isChecked, var));
        }
        private void Execute(string input, bool isChecked, string var)
        {
            if (prev_input == input && prev_state == isChecked && prev_var == var || input == "")
                return;
            prev_input = input;
            prev_var = var;
            prev_state = isChecked;

            Dispatcher.Invoke(() =>
            {
                if (WithSteps.IsChecked == false)
                {
                    StepScroller.Visibility = Visibility.Hidden;
                    StepBox.Visibility = Visibility.Hidden;
                }
                else
                {
                    StepScroller.Visibility = Visibility.Visible;
                    StepBox.Visibility = Visibility.Visible;
                }
            });
            Differentiate(input, isChecked, var);
        }
        private async void inputexpression_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await Task.Run(() => Execute(inputexpression.Text, (bool)WithSteps.IsChecked, DiffVar.Text));
            }
        }
        private void Differentiate(string input, bool isChecked, string var)
        {
            Dispatcher.Invoke(() => Progress.Value = 0);
            expression = new(input, var);
            if (!expression.IsDifferentiable())
            {
                Dispatcher.Invoke(() =>
                {
                    StatusBox.Foreground = Brushes.Red;
                    StatusBox.Text = "Chyba: Nesprávný vstup";
                });
                return;
            }
            Dispatcher.Invoke(() =>
            {
                StatusBox.Foreground = Brushes.Black;
                StatusBox.Text = "Derivuji...";
                Progress.Value = 40;
            });
            expression.Differentiate(isChecked);
            Dispatcher.Invoke(() =>
            {
                StatusBox.Text = "Zjednodušuji vzorec...";
                Progress.Value = 70;
            });
            expression.Simplify();
            if (isChecked)
            {
                Dispatcher.Invoke(() =>
                {
                    StepPanel.Children.Clear();
                    Progress.IsIndeterminate = true;
                    StatusBox.Text = "Generuji kroky...";
                });
                GenerateSteps(var);
            }
            Dispatcher.Invoke(() =>
            {
                Progress.IsIndeterminate = false;
                StatusBox.Text = "Sázím...";
                Progress.Value = 90;
                OutputFormula.Formula = expression.ToString();
                Progress.Value = 100;
                StatusBox.Text = "Hotovo";
                NextDiff.IsEnabled = true;
            });
        }
        private void DifferentiateFurther(bool isChecked, string var)
        {
            Dispatcher.Invoke(() =>
            {
                StepPanel.Children.Clear();
                StatusBox.Text = "Derivuji...";
                Progress.Value = 40;
            });
            expression.Differentiate(isChecked);
            Dispatcher.Invoke(() =>
            {
                StatusBox.Text = "Zjednodušuji vzorec...";
                Progress.Value = 70;
            });
            expression.Simplify();
            diff_pos++;
            if (isChecked)
            {
                Dispatcher.Invoke(() =>
                {
                    Progress.IsIndeterminate = true;
                    StatusBox.Text = "Generuji kroky...";
                });
                GenerateSteps(var);
            }
            Dispatcher.Invoke(() =>
            {
                Progress.IsIndeterminate = false;
                StatusBox.Text = "Sázím...";
                Progress.Value = 90;
                OutputFormula.Formula = expression.ToStringSpec(diff_pos, true);
                Progress.Value = 100;
                StatusBox.Text = "Hotovo";
                PrevDiff.IsEnabled = true;
            });
        }

        private void inputexpression_TextChanged(object sender, TextChangedEventArgs e)
        {
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

        private void GenerateSteps(string diffvar)
        {
            StepGenerator generator = new(diffvar);
            List<string> steps = generator.StartGenerating();
            int margin = 0;
            int color = 0;

            foreach (string step in steps)
            {
                string[] command = step.Split(' ');
                switch (command[0])
                {
                    case "/math":
                        Dispatcher.Invoke(() => StepPanel.Children.Add(new FormulaControl() { Formula = step[5..], Margin = new Thickness(margin, 0, 0, 0), Background = bcolors[color] })); //x,5,0,0
                        break;
                    case "/margin":
                        margin = int.Parse(command[1]);
                        break;
                    case "/background":
                        color = int.Parse(command[1]) % bcolors.Count;
                        break;
                    default:
                        Dispatcher.Invoke(() => StepPanel.Children.Add(new TextBlock() { Text = step, FontSize = 15, Margin = new Thickness(margin, 0, 0, 0), Background = bcolors[color] })); //x,5,0,10
                        break;
                }
            }
        }

        private async void NextDiff_Click(object sender, RoutedEventArgs e)
        {
            bool isChecked = (bool)WithSteps.IsChecked;
            string var = DiffVar.Text;

            if (diff_pos < expression.Diff_Count())
            {
                diff_pos++;
                OutputFormula.Formula = expression.ToStringSpec(diff_pos, true);
                PrevDiff.IsEnabled = true;
            }
            else if (diff_pos == expression.Diff_Count())
            {
                await Task.Run(() =>
                {
                    DifferentiateFurther(isChecked, var);
                });
            }
            
            if (expression.ToStringSpec(diff_pos, true) == "0")
            {
                NextDiff.IsEnabled = false;
            }
        }

        private void PrevDiff_Click(object sender, RoutedEventArgs e)
        {
            if (diff_pos <= expression.Diff_Count() && diff_pos > 2)
            {
                diff_pos--;
                OutputFormula.Formula = expression.ToStringSpec(diff_pos, true);
            }
            else if (diff_pos == 2)
            {
                diff_pos--;
                OutputFormula.Formula = expression.ToStringSpec(diff_pos, true);
                PrevDiff.IsEnabled = false;
            }
            NextDiff.IsEnabled = true;
        }

    }
}
