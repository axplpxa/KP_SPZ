using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace SLB_Zakorko_KP
{
    public partial class LSB_ : Form
    {
        public LSB_()
        {
            InitializeComponent();
        }

        const int hidden_size = 1;
        const int hidden_text_size = 3;
        const int hidden_text_maxsize = 999;

        private BitArray ByteToBit(byte src)
        {
            BitArray bitArray = new BitArray(8);
            bool st = false;
            for (int i = 0; i < 8; i++)
            {
                if ((src >> i & 1) == 1)
                {
                    st = true;
                }
                else st = false;
                bitArray[i] = st;
            }
            return bitArray;
        }

        private byte BitToByte(BitArray scr)
        {
            byte num = 0;
            for (int i = 0; i < scr.Count; i++)
                if (scr[i] == true)
                    num += (byte)Math.Pow(2, i);
            return num;
        }

        /*Перевіряє, чи файл зашифрований, повертає true, якщо символ у першому пікселі дорівнює / інакше false */
        private bool Сoncealment(Bitmap scr)
        {
            byte[] rez = new byte[1];
            Color color = scr.GetPixel(0, 0);
            BitArray colorArray = ByteToBit(color.R); //Байт кольору в массив бітів
            BitArray messageArray = ByteToBit(color.R); ;//Ініціалізація
            messageArray[0] = colorArray[0];
            messageArray[1] = colorArray[1];

            colorArray = ByteToBit(color.G);
            messageArray[2] = colorArray[0];
            messageArray[3] = colorArray[1];
            messageArray[4] = colorArray[2];

            colorArray = ByteToBit(color.B);
            messageArray[5] = colorArray[0];
            messageArray[6] = colorArray[1];
            messageArray[7] = colorArray[2];
            rez[0] = BitToByte(messageArray); // Байт символу, з 1 пікселю
            string m = Encoding.GetEncoding(1251).GetString(rez);
            if (m == "/")
            {
                return true;
            }
            else return false;
        }

        /*Приведення кількості символів до розміру hidden_text_size байтів*/
        private byte[] NormalizeWriteCount(byte[] CountSymbols)
        {
            int PaddingByte = hidden_text_size - CountSymbols.Length;

            byte[] WriteCount = new byte[hidden_text_size];

            for (int j = 0; j < PaddingByte; j++)
            {
                WriteCount[j] = 0x30;
            }

            for (int j = PaddingByte; j < hidden_text_size; j++)
            {
                WriteCount[j] = CountSymbols[j - PaddingByte];
            }
            return WriteCount;
        }

        /*Записує кількість символів для приховування в перші біти зображення */
        private void WriteCountText(int count, Bitmap src)
        {
            byte[] CountSymbols = Encoding.GetEncoding(1251).GetBytes(count.ToString());

            if (CountSymbols.Length < hidden_text_size)
            {
                CountSymbols = NormalizeWriteCount(CountSymbols);
            }

            for (int i = 0; i < hidden_text_size; i++)
            {
                BitArray bitCount = ByteToBit(CountSymbols[i]); //біти кількості символів
                Color pColor = src.GetPixel(0, i + 1);
                BitArray bitsCurColor = ByteToBit(pColor.R); //біт кольорів поточного пікселя
                bitsCurColor[0] = bitCount[0];
                bitsCurColor[1] = bitCount[1];
                byte nR = BitToByte(bitsCurColor); //новий біт кольору пікселя

                bitsCurColor = ByteToBit(pColor.G);
                bitsCurColor[0] = bitCount[2];
                bitsCurColor[1] = bitCount[3];
                bitsCurColor[2] = bitCount[4];
                byte nG = BitToByte(bitsCurColor);

                bitsCurColor = ByteToBit(pColor.B);
                bitsCurColor[0] = bitCount[5];
                bitsCurColor[1] = bitCount[6];
                bitsCurColor[2] = bitCount[7];
                byte nB = BitToByte(bitsCurColor);

                Color nColor = Color.FromArgb(nR, nG, nB); //новий колір з отриманих бітів
                src.SetPixel(0, i + 1, nColor); //запис отриманого кольору у картинку
            }
        }

        /*Читає кількість символів для дешифрування з перших біт картинки*/
        private int ReadCountText(Bitmap src)
        {
            byte[] rez = new byte[hidden_text_size];
            for (int i = 0; i < hidden_text_size; i++)
            {
                Color color = src.GetPixel(0, i + 1);
                BitArray colorArray = ByteToBit(color.R); 
                BitArray bitCount = ByteToBit(color.R); ; 
                bitCount[0] = colorArray[0];
                bitCount[1] = colorArray[1];

                colorArray = ByteToBit(color.G);
                bitCount[2] = colorArray[0];
                bitCount[3] = colorArray[1];
                bitCount[4] = colorArray[2];

                colorArray = ByteToBit(color.B);
                bitCount[5] = colorArray[0];
                bitCount[6] = colorArray[1];
                bitCount[7] = colorArray[2];
                rez[i] = BitToByte(bitCount);
            }
            string m = Encoding.GetEncoding(1251).GetString(rez);
            return Convert.ToInt32(m, 10);
        }

        /* Відкрити файл для приховування */
        private void buttonwrite_Click(object sender, EventArgs e)
        {
            string FilePic;
            string FileText;
            OpenFileDialog dPic = new OpenFileDialog();
            dPic.Filter = "Файлы изображений (*.bmp)|*.bmp|Все файлы (*.*)|*.*";
            if (dPic.ShowDialog() == DialogResult.OK)
            {
                FilePic = dPic.FileName;
            }
            else
            {
                FilePic = "";
                return;
            }

            FileStream rFile;
            try
            {
                rFile = new FileStream(FilePic, FileMode.Open); 
            }
            catch (IOException)
            {
                MessageBox.Show("Помилка відкриття файлу", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Bitmap bPic = new Bitmap(rFile);

            OpenFileDialog dText = new OpenFileDialog();
            dText.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (dText.ShowDialog() == DialogResult.OK)
            {
                FileText = dText.FileName;
            }
            else
            {
                FileText = "";
                return;
            }

            FileStream rText;
            try
            {
                rText = new FileStream(FileText, FileMode.Open); 
            }
            catch (IOException)
            {
                MessageBox.Show("Помилка відкриття файлу", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            BinaryReader bText = new BinaryReader(rText, Encoding.ASCII);

            List<byte> bList = new List<byte>();
            while (bText.PeekChar() != -1)
            { 
                bList.Add(bText.ReadByte());
            }
            int CountText = bList.Count; // CountText - кількість байтів тексту
            bText.Close();
            rFile.Close();

            //Перевірка, на розмір інформації
            if (CountText > (hidden_text_maxsize - hidden_size - hidden_text_size))
            {
                MessageBox.Show("Розмір тексту великий для даного алгоритму, зменшіть розмір", "Інформація", MessageBoxButtons.OK);
                return;
            }

            //Перевірка, чи не мала картинка для обсягу інформації
            if (CountText > (bPic.Width * bPic.Height))
            {
                MessageBox.Show("Вибрана картинка мала для розміщення вибраного тексту", "Інформація", MessageBoxButtons.OK);
                return;
            }

            //Перевірка, чи є інформація у картинці
            if (Сoncealment(bPic))
            {
                MessageBox.Show("У файлі вже приховано інформацію", "Інформація", MessageBoxButtons.OK);
                return;
            }

            byte[] Symbol = Encoding.GetEncoding(1251).GetBytes("/");
            BitArray ArrBeginSymbol = ByteToBit(Symbol[0]);
            Color curColor = bPic.GetPixel(0, 0);
            BitArray tempArray = ByteToBit(curColor.R);
            tempArray[0] = ArrBeginSymbol[0];
            tempArray[1] = ArrBeginSymbol[1];
            byte nR = BitToByte(tempArray);

            tempArray = ByteToBit(curColor.G);
            tempArray[0] = ArrBeginSymbol[2];
            tempArray[1] = ArrBeginSymbol[3];
            tempArray[2] = ArrBeginSymbol[4];
            byte nG = BitToByte(tempArray);

            tempArray = ByteToBit(curColor.B);
            tempArray[0] = ArrBeginSymbol[5];
            tempArray[1] = ArrBeginSymbol[6];
            tempArray[2] = ArrBeginSymbol[7];
            byte nB = BitToByte(tempArray);

            Color nColor = Color.FromArgb(nR, nG, nB);
            bPic.SetPixel(0, 0, nColor);
            //пероий піксель буде символ /,  що вказує на приховану інформацію в картинці

            WriteCountText(CountText, bPic); //записуємо кількість символів для приховування

            int index = 0;
            bool st = false;
            for (int i = hidden_text_size + 1; i < bPic.Width; i++)
            {
                for (int j = 0; j < bPic.Height; j++)
                {
                    Color pixelColor = bPic.GetPixel(i, j);
                    if (index == bList.Count)
                    {
                        st = true;
                        break;
                    }
                    BitArray colorArray = ByteToBit(pixelColor.R);
                    BitArray messageArray = ByteToBit(bList[index]);
                    colorArray[0] = messageArray[0]; 
                    colorArray[1] = messageArray[1]; 
                    byte newR = BitToByte(colorArray);

                    colorArray = ByteToBit(pixelColor.G);
                    colorArray[0] = messageArray[2];
                    colorArray[1] = messageArray[3];
                    colorArray[2] = messageArray[4];
                    byte newG = BitToByte(colorArray);

                    colorArray = ByteToBit(pixelColor.B);
                    colorArray[0] = messageArray[5];
                    colorArray[1] = messageArray[6];
                    colorArray[2] = messageArray[7];
                    byte newB = BitToByte(colorArray);

                    Color newColor = Color.FromArgb(newR, newG, newB);
                    bPic.SetPixel(i, j, newColor);
                    index++;
                }
                if (st)
                {
                    break;
                }
            }
            pictureBox1.Image = bPic;

            String sFilePic;
            SaveFileDialog dSavePic = new SaveFileDialog();
            dSavePic.Filter = "Файлы изображений (*.bmp)|*.bmp|Все файлы (*.*)|*.*";
            if (dSavePic.ShowDialog() == DialogResult.OK)
            {
                sFilePic = dSavePic.FileName;
            }
            else
            {
                sFilePic = "";
                return;
            };

            FileStream wFile;
            try
            {
                wFile = new FileStream(sFilePic, FileMode.Create); 
            }
            catch (IOException)
            {
                MessageBox.Show("Помилка відкриття файлу на запис", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bPic.Save(wFile, System.Drawing.Imaging.ImageFormat.Bmp);
            wFile.Close();
        }

        /*Відкриття фалу для дешифрування */
        private void buttonread_Click(object sender, EventArgs e)
        {
            string FilePic;
            OpenFileDialog dPic = new OpenFileDialog();
            dPic.Filter = "Файлы изображений (*.bmp)|*.bmp|Все файлы (*.*)|*.*";
            if (dPic.ShowDialog() == DialogResult.OK)
            {
                FilePic = dPic.FileName;
            }
            else
            {
                FilePic = "";
                return;
            }

            FileStream rFile;
            try
            {
                rFile = new FileStream(FilePic, FileMode.Open); 
            }
            catch (IOException)
            {
                MessageBox.Show("Помилка відкриття файлу", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Bitmap bPic = new Bitmap(rFile);
            if (!Сoncealment(bPic))
            {
                MessageBox.Show("У файлі не прихована інформація", "Інформація", MessageBoxButtons.OK);
                rFile.Close();
                return;
            }

            int countSymbol = ReadCountText(bPic); //кількість прихованих символів
            byte[] message = new byte[countSymbol];
            int index = 0;
            bool st = false;
            for (int i = hidden_text_size + 1; i < bPic.Width; i++)
            {
                for (int j = 0; j < bPic.Height; j++)
                {
                    Color pixelColor = bPic.GetPixel(i, j);
                    if (index == message.Length)
                    {
                        st = true;
                        break;
                    }
                    BitArray colorArray = ByteToBit(pixelColor.R);
                    BitArray messageArray = ByteToBit(pixelColor.R); ;
                    messageArray[0] = colorArray[0];
                    messageArray[1] = colorArray[1];

                    colorArray = ByteToBit(pixelColor.G);
                    messageArray[2] = colorArray[0];
                    messageArray[3] = colorArray[1];
                    messageArray[4] = colorArray[2];

                    colorArray = ByteToBit(pixelColor.B);
                    messageArray[5] = colorArray[0];
                    messageArray[6] = colorArray[1];
                    messageArray[7] = colorArray[2];
                    message[index] = BitToByte(messageArray);
                    index++;
                }
                if (st)
                {
                    break;
                }
            }
            string strMessage = Encoding.GetEncoding(1251).GetString(message);

            string sFileText;
            SaveFileDialog dSaveText = new SaveFileDialog();
            dSaveText.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (dSaveText.ShowDialog() == DialogResult.OK)
            {
                sFileText = dSaveText.FileName;
            }
            else
            {
                sFileText = "";
                rFile.Close();
                return;
            };

            FileStream wFile;
            try
            {
                wFile = new FileStream(sFileText, FileMode.Create); 
            }
            catch (IOException)
            {
                MessageBox.Show("Помилка відкриття фалу для запису", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rFile.Close();
                return;
            }
            StreamWriter wText = new StreamWriter(wFile, Encoding.Default);
            wText.Write(strMessage);
            MessageBox.Show("Текст записаний у файл", "Інформація", MessageBoxButtons.OK);
            wText.Close();
            wFile.Close(); 
            rFile.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
