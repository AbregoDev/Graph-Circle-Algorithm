using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CircleFirstAlgorithm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool onSelection;
        private Label pxl1, pxl2;
        private Label[,] pixels;

        public MainWindow()
        {
            InitializeComponent();

            onSelection = false;

            pixels = new Label[48, 24];
            for (byte i = 0; i < pixels.GetLength(0); i++)
                for (byte k = 0; k < pixels.GetLength(1); k++)
                    pixels[i, k] = new Label();

            foreach (Label lbl in pixels)
            {
                screen.Children.Add(lbl);
                //lbl.MouseEnter += Lbl_MouseEnter;
                //lbl.MouseLeave += Lbl_MouseLeave;
                //lbl.MouseLeftButtonDown += Lbl_MouseLeftButtonDown;
            }

            for (int i = 0; i < pixels.GetLength(0); i++)
            {
                for (int k = 0; k < pixels.GetLength(1); k++)
                {
                    Grid.SetColumn(pixels[i, k], i);
                    Grid.SetRow(pixels[i, k], k);
                }
            }
        }

        //En el modo de selección, al señalar la casilla ésta se ilumina de rojito
        private void Lbl_MouseEnter(object sender, MouseEventArgs e)
        {
            //Sólo se iluminan las casillas cuando se está en el modo selección
            if (onSelection)
            {
                if (pxl1 == null)
                {
                    Label lbl = (Label)sender;
                    lbl.Background = new SolidColorBrush()
                    {
                        Color = Color.FromRgb(255, 121, 121)
                    };
                }
                else if (pxl2 == null && Grid.GetColumn((Label)sender) != Grid.GetColumn(pxl1))
                {
                    Label lbl = (Label)sender;
                    pixels[Grid.GetColumn(lbl), Grid.GetRow(pxl1)].Background = new SolidColorBrush()
                    {
                        Color = Color.FromRgb(255, 121, 121)
                    };
                }
            }
        }

        //En el modo selección, al retirar el cursor de la casilla, vuelve a color transparente
        //excepto si ya fue seleccionado (clic)
        private void Lbl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (onSelection)
            {
                if (pxl1 == null)
                {
                    var lbl = (Label)sender;
                    lbl.Background = Brushes.Transparent;
                }
                else if (pxl2 == null && Grid.GetColumn((Label)sender) != Grid.GetColumn(pxl1))
                {
                    pixels[Grid.GetColumn((Label)sender), Grid.GetRow(pxl1)].Background = Brushes.Transparent;
                }
            }
        }

        //En el modo selección, al hacer clic sobre una celda se guarda la referencia
        private void Lbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Se debe estar en el modo selección
            if (onSelection)
            {
                //Si pxl1 es null, significa que no se ha seleccionado ningún pixel
                if (pxl1 == null)
                {
                    pxl1 = (Label)sender;
                    pxl1.Background = new SolidColorBrush()
                    {
                        Color = Color.FromRgb(235, 77, 75)
                    };
                }
                //Si pxl2 es nulo, significa que el punto inicial ya fue seleccionado
                //Ahora se seleccionará el punto final
                else if (pxl2 == null)
                {
                    //Ambos puntos deben ser diferentes entre sí
                    //Si lo son, se asigna el punto final y se termina el modo selección
                    if (pxl1 != pixels[Grid.GetColumn((Label)sender), Grid.GetRow(pxl1)])
                    {
                        pxl2 = pixels[Grid.GetColumn((Label)sender), Grid.GetRow(pxl1)];
                        pxl2.Background = new SolidColorBrush()
                        {
                            Color = Color.FromRgb(235, 77, 75)
                        };

                        onSelection = false;
                        btnStart.IsEnabled = true;
                    }
                    //En caso de ser los mismos puntos, se mandará una alerta
                    else
                    {
                        MessageBox.Show("No puede elegirse el mismo punto.\nSeleccione un punto diferente, ploxi plox");
                    }
                }
            }
        }

        //Grafica la figura seleccionada
        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                byte cx = byte.Parse(tbxCentroX.Text);
                byte cy = byte.Parse(tbxCentroY.Text);
                byte radius = byte.Parse(tbxRadio.Text);

                byte x, y;

                for (byte i = 0; i < pixels.GetLength(1); i++)
                    for (byte k = 0; k < pixels.GetLength(0); k++)
                        pixels[k, i].Background = Brushes.Transparent;


                //Center
                pixels[cx, cy].Background = Brushes.OrangeRed;

                //Circunference
                for(byte angle = 0; angle < 45; angle += 5)
                {
                    x = (byte)Math.Round(radius * Math.Cos(angle * Math.PI / 180));
                    y = (byte)Math.Round(radius * Math.Sin(angle * Math.PI / 180));

                    pixels[cx + x, cy - y].Background = Brushes.DodgerBlue;
                    pixels[cx + y, cy - x].Background = Brushes.DodgerBlue;
                    pixels[cx - y, cy - x].Background = Brushes.DodgerBlue;
                    pixels[cx - x, cy - y].Background = Brushes.DodgerBlue;
                    pixels[cx - x, cy + y].Background = Brushes.DodgerBlue;
                    pixels[cx - y, cy + x].Background = Brushes.DodgerBlue;
                    pixels[cx + y, cy + x].Background = Brushes.DodgerBlue;
                    pixels[cx + x, cy + y].Background = Brushes.DodgerBlue;

                    await Task.Delay(100);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Rellene los campos correctamente\n\nError: " + ex.Message);
            }
        }
    }
}
