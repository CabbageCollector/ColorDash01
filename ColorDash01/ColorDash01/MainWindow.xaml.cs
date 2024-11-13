using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace ColorDash01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random random = new Random();

        Color[] colors = new Color[] { Colors.Red, Colors.Blue, Colors.Green, Colors.Yellow, Colors.Purple };
        Color targetColor;
        bool isTargetColorGenerated = false;

        Border[] colorBoardEasy;

        int round = 1; // = correct clicks/5 if %5 = 0
        int correctClicks = 0;

        private DispatcherTimer countdownTimer;
        private static int maxTime = 5; // max time goes down each round
        private int timeLeft = maxTime;  // The countdown starts at 10 seconds

        int points = 0;
        int pointMultiplier = 1; // goes up each round
        public MainWindow()
        {
            InitializeComponent();
            colorBoardEasy = new Border[] { a1, a2, a3, b1, b2, b3, c1, c2, c3 };

            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);  // Set the interval to 1 second
            countdownTimer.Tick += CountdownTimer_Tick;  // Event handler for each tick
        }
        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            // Decrease the time by 1 second
            timeLeft--;

            // Update the TextBlock showing the time left
            tbTimer.Text = timeLeft.ToString();

            // When the timer reaches zero, stop the timer
            if (timeLeft <= 0)
            {
                countdownTimer.Stop();
                // You can add logic here to end the game or show a message
                MessageBox.Show("Time's up! Game Over.");
                // Optionally restart or stop the game
            }
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            startScreen.Visibility = Visibility.Collapsed;
            gameScreen.Visibility = Visibility.Visible;

            // Reset the timer and start it
            timeLeft = 10;  // Reset to the starting time
            tbTimer.Text = timeLeft.ToString();  // Display the starting time in the TextBlock
            countdownTimer.Start();  // Start the countdown timer

            targetColorChange();  // Set the target color
            ChangeGameBoardColors();  // Change the game board colors

        }

        private void startNewRound()
        {

        }

        private void color_Click(object sender, RoutedEventArgs e)
        {
            Border btn = sender as Border;
            if (((SolidColorBrush)btn.Background).Color == targetColor)
            {
                if (timeLeft >= maxTime / 2)
                    // make points more if you click it fast
                    points += 10;
                tbPoints.Text = points.ToString();
            }

            // Reset the timer and start it again
            timeLeft = 10;  // Reset to the starting time
            tbTimer.Text = timeLeft.ToString();  // Update the TextBlock
            countdownTimer.Start();  // Restart the countdown timer

            targetColorChange();
            ChangeGameBoardColors();
        }

        private void targetColorChange()
        {
            targetColor = colors[random.Next(0, colors.Length)];

            bTargetColor.Background = new SolidColorBrush(targetColor);
        }

        private void ChangeGameBoardColors()
        {
            // Reset the flag before generating colors for the new round
            isTargetColorGenerated = false;

            for (int i = 0; i < colorBoardEasy.Length; i++) // Looping through each button
            {
                // Generate a random color for the button
                var generatedColor = colors[random.Next(0, colors.Length)];

                // Set the color for the button
                colorBoardEasy[i].Background = new SolidColorBrush(generatedColor);

                // If the target color has already been generated, ensure no more target colors are generated
                if (((SolidColorBrush)colorBoardEasy[i].Background).Color == targetColor)
                {
                    // If the target color is already set, enter the loop to generate a new color
                    if (isTargetColorGenerated)
                    {
                        // Keep generating colors until the current color is not the target color
                        while (((SolidColorBrush)colorBoardEasy[i].Background).Color == targetColor)
                        {
                            generatedColor = colors[random.Next(0, colors.Length)];
                            colorBoardEasy[i].Background = new SolidColorBrush(generatedColor);
                        }
                    }
                    else
                    {
                        // Mark the target color as generated
                        isTargetColorGenerated = true;
                    }
                }

                // Ensure that the target color is assigned to the last element if it's not already set
                if (!isTargetColorGenerated && i == colorBoardEasy.Length - 1)
                {
                    colorBoardEasy[i].Background = new SolidColorBrush(targetColor); // Set the last color to the target color
                    isTargetColorGenerated = true; // Ensure the target color is marked as generated
                }
            }
        }


        private void genNewColors_Click(object sender, RoutedEventArgs e)
        {
            // Reset the timer and start it again
            timeLeft = 10;  // Reset to the starting time
            tbTimer.Text = timeLeft.ToString();  // Update the TextBlock
            countdownTimer.Start();  // Restart the countdown timer

            targetColorChange();
            ChangeGameBoardColors();
        }

        private void displayRound_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            tbDisplayRound.Foreground = new SolidColorBrush(Colors.Lime);
            tbDisplayRound.Text = "Start";
        }

        private void displayRound_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            tbDisplayRound.Foreground = new SolidColorBrush(Colors.Black);
            tbDisplayRound.Text = "Round " + round.ToString();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            bdrDisplayRound.Visibility = Visibility.Collapsed;

            // Reset the timer and start it again
            timeLeft = maxTime;  // Reset to the starting time
            tbTimer.Text = timeLeft.ToString();  // Update the TextBlock
            countdownTimer.Start();  // Restart the countdown timer

            targetColorChange();
            ChangeGameBoardColors();
        }

        private void color_Leave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = sender as Border;
            border.BorderThickness = new Thickness(0);
        }

        private void color_Enter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border border = sender as Border;
            border.BorderThickness = new Thickness(2);
        }
    }
}
